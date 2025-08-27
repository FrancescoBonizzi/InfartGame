import {Texture} from "pixi.js";
import PlayerAnimations from "../player/PlayerAnimations.ts";

interface InfartAssets {
    fontName: string;
    textures: {
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
            caccaParticle: Texture;
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
    player: PlayerAnimations
}

export default InfartAssets;