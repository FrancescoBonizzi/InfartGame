import {AnimatedSprite, Assets, Sprite} from "pixi.js";
import InfartAssets from "./InfartAssets";

export const loadAssets = async (): Promise<InfartAssets> => {

    // Menu
    const texture = await Assets.load(
        "/assets/images/menuBackground.png"
    );
    const menuBackground = new Sprite(texture);

    // SpriteSheet
    const spriteSheet = await Assets.load('/assets/images/spriteSheet.json');

    const anim = new AnimatedSprite(spriteSheet.animations.playerMerda);
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