import { AnimatedSprite, Sprite } from "pixi.js";

interface InfartAssets {
    sprites: {
        menu: {
            background: Sprite;
            gameTitle: Sprite;
            scoreBackground: Sprite;
            gameOverBackground: Sprite;
        },
        player: {
            run: AnimatedSprite;
            idle: AnimatedSprite;
            fart: AnimatedSprite;
            fall: AnimatedSprite;
            merda: AnimatedSprite;
        },
        buildings: {
            back: Sprite[];
            mid: Sprite[];
            ground: Sprite[];
        },
        bang: Sprite;
        broccoloParticle: Sprite;
        bean: Sprite;
        burger: Sprite;
        gameOver: Sprite;
        jalapenoParticle: Sprite;
        jalapenos: Sprite;
        merda: Sprite,
        pause: Sprite;
        play: Sprite;
        record: Sprite;
        scoreggiaParticle: Sprite;
        stella: Sprite;
        verdura: Sprite;
        background: Sprite;
        deathScreen: Sprite;
        nuvola1: Sprite;
        nuvola2: Sprite;
        nuvola3: Sprite;
    }
    sounds: {

    }
}

export default InfartAssets;