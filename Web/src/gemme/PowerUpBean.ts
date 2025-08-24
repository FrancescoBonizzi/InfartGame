import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {AnimatedSprite, Point} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import BeanParticleSystem from "../particleEmitters/BeanParticleSystem.ts";
import PowerUpTypes from "./PowerUpTypes.ts";

class PowerUpBean extends PowerUp {

    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.bean,
            position,
            () => new BeanParticleSystem(assets, world));
    }

    override getPopupText(): string {
        return "Tornado intestinale!";
    }

    override getPowerUpType(): PowerUpTypes {
        return PowerUpTypes.Bean;
    }

    override getJumpForce(): number {
        return 500;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 100;
    }

    override getFillColor(): string {
        return '#D2691E';
    }

    override getDurationMilliseconds(): number {
        return 3500;
    }

    override getMaxConsecutiveJumps(): number {
        return 1000;
    }

    override getParticleGenerationIntervalMilliseconds(): number {
        return 0;
    }

    override getPlayerAnimation(): AnimatedSprite | null {
        return null;
    }

}

export default PowerUpBean;