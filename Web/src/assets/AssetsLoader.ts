import {AnimatedSprite, Assets, Sprite} from "pixi.js";
import InfartAssets from "./InfartAssets";

export const loadAssets = async (): Promise<InfartAssets> => {

    // Menu
    const texture = await Assets.load(
        "/assets/images/menuBackground.png"
    );
    const menuBackground = new Sprite(texture);

    // SpriteSheet
    const playerSpriteSheet = await Assets.load('/assets/images/spriteSheets/playerTexturesSpriteSheet.json');

    const anim = new AnimatedSprite(playerSpriteSheet.animations.playerRun);
    anim.animationSpeed = 0.3;
    anim.play();

    const singleFrame = new Sprite(playerSpriteSheet.textures['jalapenos']);

    return {
        menu: {
            background: menuBackground
        },
        jalapeno: singleFrame,
        player: {
            run: anim,
        }
    }
};