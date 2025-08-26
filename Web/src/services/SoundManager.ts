import {Howl} from 'howler';

class SoundManager {
    private sounds: Record<string, Howl> = {};

    private paths = {
        musicMenu: "/assets/sounds/music/menu.mp3",
        musicGame: "/assets/sounds/music/game.mp3",
        fart: (n: number) => `/assets/sounds/farts/fart${n}.mp3`,
        bite: "/assets/sounds/effects/bite.mp3",
        fall: "/assets/sounds/effects/fall.mp3",
        heartbeat: "/assets/sounds/effects/heartbeat.mp3",
        explosion: "/assets/sounds/effects/explosion.mp3",
        thunder: "/assets/sounds/effects/thunder.m4a",
        truck: "/assets/sounds/effects/truck.m4a",
        jalapeno: "/assets/sounds/effects/jalapeno.m4a",
    };

    constructor() {
        this.preload();
    }

    private preload() {
        // carico i suoni principali
        this.sounds["musicMenu"] = new Howl({src: [this.paths.musicMenu], loop: true, volume: 0.4});
        this.sounds["musicGame"] = new Howl({src: [this.paths.musicGame], loop: true, volume: 0.4});
        this.sounds["bite"] = new Howl({src: [this.paths.bite]});
        this.sounds["fall"] = new Howl({src: [this.paths.fall]});
        this.sounds["explosion"] = new Howl({src: [this.paths.explosion]});
        this.sounds["heartbeat"] = new Howl({src: [this.paths.heartbeat], loop: true});
        this.sounds["thunder"] = new Howl({src: [this.paths.thunder], loop: true});
        this.sounds["truck"] = new Howl({src: [this.paths.truck], loop: true});
        this.sounds["jalapeno"] = new Howl({src: [this.paths.jalapeno], loop: true});
    }

    // ---------- MUSICHE ----------
    playMenuSoundTrack() {
        if (this.sounds["musicMenu"]!.playing())
            return;

        this.stopAllMusic();
        this.sounds["musicMenu"]!.play();
    }

    playGameSoundTrack() {
        this.stopAllMusic();
        this.sounds["musicGame"]!.play();
    }

    private stopAllMusic() {
        this.sounds["musicMenu"]!.stop();
        this.sounds["musicGame"]!.stop();
    }

    // ---------- EFFETTI ----------
    playFall() {
        this.sounds["fall"]!.play();
    }

    playFart() {
        const n = Math.floor(Math.random() * 7) + 1;
        const key = `fart${n}`;
        if (!this.sounds[key]) {
            this.sounds[key] = new Howl({src: [this.paths.fart(n)]});
        }
        this.sounds[key].play();
    }

    stopFart() {
        for (let n = 1; n <= 7; n++) {
            const key = `fart${n}`;
            this.sounds[key]?.stop();
        }
    }

    playBite() {
        this.sounds["bite"]!.play();
    }

    playExplosion() {
        this.sounds["explosion"]!.play();
    }

    playBean() {
        this.sounds["thunder"]!.play();
    }

    stopBean() {
        this.sounds["thunder"]!.stop();
    }

    playBroccolo() {
        this.sounds["truck"]!.play();
    }

    stopBroccolo() {
        this.sounds["truck"]!.stop();
    }

    playJalapeno() {
        this.sounds["jalapeno"]!.play();
    }

    stopJalapeno() {
        this.sounds["jalapeno"]!.stop();
    }

    async playHeartBeat() {
        if (!this.sounds["heartbeat"]!.playing()) {
            this.sounds["heartbeat"]!.play();
        }
    }

    stopHeartBeat() {
        this.sounds["heartbeat"]!.stop();
    }
}

export default SoundManager;