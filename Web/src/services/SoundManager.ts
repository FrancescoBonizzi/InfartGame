// SoundManager.ts
type OneShot = { stop: () => void; tag?: string };

class SoundManager {
    private ctx: AudioContext;
    private readonly master: GainNode;
    private readonly musicBus: GainNode;
    private readonly sfxBus: GainNode;

    private loops = new Map<string, { src: AudioBufferSourceNode; gain: GainNode; tag: string | undefined }>();
    private unlocked = false;

    private buffers = new Map<string, AudioBuffer>();
    private activeSfx = new Set<AudioNode>();
    private heartbeat?: { src: AudioBufferSourceNode; gain: GainNode } | null = null;

    private paths = {
        musicMenu: "/assets/sounds/music/menu.mp3",
        musicGame: "/assets/sounds/music/game.mp3",
        fart: (n: number) => `/assets/sounds/farts/fart${n}.mp3`,
        bite: "/assets/sounds/effects/bite.mp3",
        fall: "/assets/sounds/effects/fall.mp3",
        heartbeat: "/assets/sounds/effects/heartbeat.mp3",
        explosion: "/assets/sounds/effects/explosion.mp3",
        thunder: "/assets/sounds/effects/thunder.mp3",
        truck: "/assets/sounds/effects/truck.mp3",
        jalapeno: "/assets/sounds/effects/jalapeno.mp3",
    };

    constructor() {
        this.ctx = new (window.AudioContext || (window as any).webkitAudioContext)();
        this.master = this.ctx.createGain();
        this.musicBus = this.ctx.createGain();
        this.sfxBus = this.ctx.createGain();

        this.musicBus.connect(this.master);
        this.sfxBus.connect(this.master);
        this.master.connect(this.ctx.destination);

        this.master.gain.value = 1;
        this.musicBus.gain.value = 1;
        this.sfxBus.gain.value = 1;
    }

    // Sblocca l'audio al primo gesto utente
    async unlock() {
        if (this.unlocked) return;
        if (this.ctx.state !== "running")
            await this.ctx.resume();
        this.unlocked = true;
    }

    // ---------- MUSICHE ----------
    playMenuSoundTrack() {
        void this.playLoop(this.paths.musicMenu);
    }

    playGameSoundTrack() {
        void this.playLoop(this.paths.musicGame);
    }


    // ---------- EFFETTI (SFX) ----------
    playFall() {
        void this.playOneShot(this.paths.fall, {volume: 1, tag: "fall"});
    }

    playFart() {
        const n = (Math.floor(Math.random() * 7) + 1);
        void this.playOneShot(this.paths.fart(n), {volume: 1, tag: "fart"});
    }

    stopFart() {
        // interrompe tutti i fart attivi
        this.stopByTag("fart");
    }

    playBite() {
        void this.playOneShot(this.paths.bite, {volume: 1, tag: "bite"});
    }

    playExplosion() {
        void this.playOneShot(this.paths.explosion, {volume: 1, tag: "explosion"});
    }

    playBean() {
        void this.playLoop(this.paths.thunder, {volume: 0.5, tag: "bean"});
    }

    playBroccolo() {
        void this.playLoop(this.paths.truck, {volume: 0.5, tag: "truck"});
    }

    playJalapeno() {
        void this.playLoop(this.paths.jalapeno, {volume: 0.5, tag: "jalapeno"});
    }


    async playHeartBeat() {
        if (this.heartbeat) return; // già attivo
        const buf = await this.getBuffer(this.paths.heartbeat);
        const src = this.ctx.createBufferSource();
        src.buffer = buf;
        src.loop = true;

        const gain = this.ctx.createGain();
        gain.gain.value = 0.9;

        src.connect(gain);
        gain.connect(this.sfxBus);
        src.start();

        this.heartbeat = {src, gain};
    }

    stopHeartBeat() {
        if (!this.heartbeat) return;
        try {
            this.heartbeat.src.stop();
        } catch {
        }
        this.heartbeat.src.disconnect();
        this.heartbeat.gain.disconnect();
        this.heartbeat = null;
    }

    // ---------- Utility interne ----------
    private async playOneShot(url: string, opts: { volume?: number; tag?: string } = {}): Promise<OneShot> {
        const buf = await this.getBuffer(url);
        const src = this.ctx.createBufferSource();
        src.buffer = buf;

        const gain = this.ctx.createGain();
        gain.gain.value = opts.volume ?? 1;

        src.connect(gain);
        gain.connect(this.sfxBus);
        src.start();

        // tracking per stopByTag/cleanup
        this.activeSfx.add(gain);
        const onEnded = () => {
            this.activeSfx.delete(gain);
            src.removeEventListener("ended", onEnded);
            src.disconnect();
            gain.disconnect();
        };
        src.addEventListener("ended", onEnded);

        // Attacca la tag al nodo gain via (any) per lookup
        (gain as any).__tag = opts.tag;

        return {
            tag: opts.tag!,
            stop: () => {
                try {
                    src.stop();
                } catch {
                }
            }
        };
    }

    private async playLoop(url: string, opts: { volume?: number; tag?: string } = {}): Promise<{ stop: () => void }> {
        const buf = await this.getBuffer(url);
        const src = this.ctx.createBufferSource();
        src.buffer = buf;
        src.loop = true;

        const gain = this.ctx.createGain();
        gain.gain.value = opts.volume ?? 1;

        src.connect(gain);
        gain.connect(this.sfxBus);
        src.start();

        // chiave per identificare univocamente il loop
        const key = `${opts.tag ?? url}::${performance.now()}`;
        this.loops.set(key, { src, gain, tag: opts.tag });

        const stop = () => {
            const entry = this.loops.get(key);
            if (!entry) return;
            try { entry.src.stop(); } catch {}
            entry.src.disconnect();
            entry.gain.disconnect();
            this.loops.delete(key);
        };

        // se il browser dovesse terminare il source (non comune con loop), cleanup
        src.addEventListener("ended", stop);

        return { stop };
    }

    private stopByTag(tag: string) {
        for (const node of Array.from(this.activeSfx)) {
            if ((node as any).__tag === tag) {
                try { /* il nodo precedente è il BufferSource; fermiamo via gain non possibile, quindi no-op */
                } catch {
                }
                // non abbiamo riferimento diretto alla source qui; lasciamo che scada naturalmente
                // alternativa: salvare anche la source; per semplicità, riduci a zero subito:
                (node as GainNode).gain.setTargetAtTime(0, this.ctx.currentTime, 0.01);
                this.activeSfx.delete(node);
            }
        }
    }

    private async getBuffer(url: string): Promise<AudioBuffer> {
        const cached = this.buffers.get(url);
        if (cached) return cached;
        const res = await fetch(url);
        const arr = await res.arrayBuffer();
        const buf = await this.ctx.decodeAudioData(arr);
        this.buffers.set(url, buf);
        return buf;
    }
}

export default SoundManager;