import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {Point, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import JalapenoParticleSystem from "../particleEmitters/JalapenoParticleSystem.ts";

class PowerUpJalapeno extends PowerUp {

    private readonly _particleSystem: JalapenoParticleSystem;

    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.jalapenos,
            position);

        this._particleSystem = new JalapenoParticleSystem(assets, world);
    }

    override update(time: Ticker) {
        super.update(time);
        this._particleSystem.update(time);
    }

    override getJumpForce(): number {
        return 800;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 100;
    }

    override getFillColor(): string {
        return '#8B0000';
    }

    override getDurationMilliseconds(): number {
        return 1000;
    }

    override getMaxConsecutiveJumps(): number {
        return 3;
    }

}

export default PowerUpJalapeno;