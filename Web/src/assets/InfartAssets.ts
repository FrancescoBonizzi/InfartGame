import {AnimatedSprite, Texture} from "pixi.js";
import {Sound} from "@pixi/sound";

interface InfartAssets {
    textures: {
        menu: {
            background: Texture;
            gameTitle: Texture;
            scoreBackground: Texture;
            gameOverBackground: Texture;
        },
        player: {
            run: AnimatedSprite;
            idle: AnimatedSprite;
            fart: AnimatedSprite;
            fall: AnimatedSprite;
            merda: AnimatedSprite;
        },
        buildings: {
            back: Texture[];
            mid: Texture[];
            ground: Texture[];
        },
        particles: {
            broccoloParticle: Texture;
            jalapenoParticle: Texture;
            scoreggiaParticle: Texture;
            starParticle: Texture;
        }
        bang: Texture;
        bean: Texture;
        burger: Texture;
        gameOver: Texture;
        jalapenos: Texture;
        merda: Texture,
        pause: Texture;
        play: Texture;
        record: Texture;
        verdura: Texture;
        background: Texture;
        deathScreen: Texture;
        nuvola1: Texture;
        nuvola2: Texture;
        nuvola3: Texture;
    }
    sounds: {
        music: {
            game: Sound,
            menu: Sound
        },
        farts: Sound[],
        effects: {
            bite: Sound,
            explosion: Sound,
            fall: Sound,
            heartBeat: Sound,
            jalapenos: Sound,
            thunder: Sound,
            truck: Sound,
        }
    }
}

export default InfartAssets;