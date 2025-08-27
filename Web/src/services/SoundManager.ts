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
        this.preload();
    }

    private preload() {
        this.sounds["musicMenu"] = new Howl({src: [this.paths.musicMenu], loop: true, volume: 0.4});
        this.sounds["musicGame"] = new Howl({src: [this.paths.musicGame], loop: true, volume: 0.4});
        this.sounds["bite"] = new Howl({src: [this.paths.bite]});
        this.sounds["fall"] = new Howl({src: [this.paths.fall]});
        this.sounds["explosion"] = new Howl({src: [this.paths.explosion]});
        this.sounds["heartbeat"] = new Howl({src: [this.paths.heartbeat], loop: true});
        this.sounds["powerup"] = new Howl({src: [this.paths.powerup]});
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