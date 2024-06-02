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

    const anim = new AnimatedSprite(playerSpriteSheet.animations.playerMerda);
    anim.animationSpeed = 0.4;
    anim.play();

    return {
        menu: {
            background: menuBackground,
        },
        player: {
            run: anim,
        }
    }
};