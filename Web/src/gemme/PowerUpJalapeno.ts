import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {AnimatedSprite, Point} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import JalapenoParticleSystem from "../particleEmitters/JalapenoParticleSystem.ts";
import PowerUpTypes from "./PowerUpTypes.ts";

class PowerUpJalapeno extends PowerUp {

    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.jalapenos,
            assets,
            position,
            () => new JalapenoParticleSystem(assets, world));
    }

    override getPopupText(): string {
        return "Fiammata rettale!";
    }

    override getPowerUpType(): PowerUpTypes {
        return PowerUpTypes.Jalapeno;
    }

    override getJumpForce(): number {
        return 1000;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 250;
    }

    override getFillColor(): string {
        return '#ff0c32';
    }

    override getDurationMilliseconds(): number {
        return 6000;
    }

    override getMaxConsecutiveJumps(): number {
        return 4;
    }

    override getParticleGenerationIntervalMilliseconds(): number {
        return 20;
    }

    override getPlayerAnimation(): AnimatedSprite | null {
        return null;
    }
}

export default PowerUpJalapeno;