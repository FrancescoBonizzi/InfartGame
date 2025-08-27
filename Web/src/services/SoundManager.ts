import {Howl} from 'howler';
import {AssetsRoot} from "../assets/AssetsLoader.ts";

class SoundManager {
    private sounds: Record<string, Howl> = {};

    private paths = {
        musicMenu: `${AssetsRoot}/sounds/music/menu.mp3`,
        musicGame: `${AssetsRoot}/sounds/music/game.mp3`,
        fart: (n: number) => `${AssetsRoot}/sounds/farts/fart${n}.mp3`,
        bite: `${AssetsRoot}/sounds/effects/bite.mp3`,
        fall: `${AssetsRoot}/sounds/effects/fall.mp3`,
        heartbeat: `${AssetsRoot}/sounds/effects/heartbeat.mp3`,
        explosion: `${AssetsRoot}/sounds/effects/explosion.mp3`,
        powerup: `${AssetsRoot}/sounds/effects/powerup.mp3`,
    };

    constructor() {
        Howler.autoUnlock = true;
        this.preload();
    }

    // Altrimenti sui Browser non funziona l'audio
    public wireAudioUnlockOnce() {
        const unlock = async () => {
            try {
                const audioContext = Howler.ctx;
                if (audioContext && audioContext.state !== 'running' && typeof audioContext.resume === 'function') {
                    await audioContext.resume();
                }
            } finally {
                window.removeEventListener('pointerdown', unlock as EventListener);
                window.removeEventListener('keydown', unlock as EventListener);
                window.removeEventListener('touchend', unlock as EventListener);
            }
        };

        window.addEventListener('pointerdown', unlock as EventListener, { once: true, passive: true });
        window.addEventListener('keydown',     unlock as EventListener, { once: true });
        window.addEventListener('touchend',    unlock as EventListener, { once: true, passive: true });
    }

    private preload() {
        this.sounds["musicMenu"] = new Howl({src: [this.paths.musicMenu], loop: true, volume: 0.4, html5: true});
        this.sounds["musicGame"] = new Howl({src: [this.paths.musicGame], loop: true, volume: 0.4, html5: true});
        this.sounds["bite"] = new Howl({src: [this.paths.bite], html5: true});
        this.sounds["fall"] = new Howl({src: [this.paths.fall], html5: true});
        this.sounds["explosion"] = new Howl({src: [this.paths.explosion], html5: true});
        this.sounds["heartbeat"] = new Howl({src: [this.paths.heartbeat], loop: true, html5: true});
        this.sounds["powerup"] = new Howl({src: [this.paths.powerup], html5: true});
    }

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

    playFall() {
        this.sounds["fall"]!.play();
    }

    playFart() {
        const n = Math.floor(Math.random() * 7) + 1;
        const key = `fart${n}`;
        if (!this.sounds[key]) {
            this.sounds[key] = new Howl({src: [this.paths.fart(n)], html5: true});
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
        this.stopHeartBeat();
    }

    playPowerUp() {
        this.sounds["powerup"]!.play();
    }

    stopPowerUp() {
        this.sounds["powerup"]!.stop();
    }

    playHeartBeat() {
        if (!this.sounds["heartbeat"]!.playing()) {
            this.sounds["heartbeat"]!.play();
        }
    }

    stopHeartBeat() {
        this.sounds["heartbeat"]!.stop();
    }
}

export default SoundManager;