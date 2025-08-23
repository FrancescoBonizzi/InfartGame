import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {Point} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import JalapenoParticleSystem from "../particleEmitters/JalapenoParticleSystem.ts";

class PowerUpJalapeno extends PowerUp {

    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.jalapenos,
            position,
            new JalapenoParticleSystem(assets, world));
    }

    override getJumpForce(): number {
        return 800;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 80;
    }

    override getFillColor(): string {
        return '#C41E3A';
    }

    override getDurationMilliseconds(): number {
        return 6000;
    }

    override getMaxConsecutiveJumps(): number {
        return 3;
    }

    override getParticleGenerationIntervalMilliseconds(): number {
        return 20;
    }
}

export default PowerUpJalapeno;