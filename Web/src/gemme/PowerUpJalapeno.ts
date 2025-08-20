import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {Point, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import JalapenoParticleSystem from "../particleEmitters/JalapenoParticleSystem.ts";

class PowerUpJalapeno extends PowerUp {

    // TODO: Forse anche qui dovrei fare un particle system array come per la scoreggia del player
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

    override addParticles(where: Point): void {
        this._particleSystem.addParticles(where);
    }

}

export default PowerUpJalapeno;