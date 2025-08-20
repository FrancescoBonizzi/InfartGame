import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {Point, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import BeanParticleSystem from "../particleEmitters/BeanParticleSystem.ts";

class PowerUpBean extends PowerUp {

    private readonly _particleSystem: BeanParticleSystem;

    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.bean,
            position);

        this._particleSystem = new BeanParticleSystem(assets, world);
    }

    override update(time: Ticker) {
        super.update(time);
        this._particleSystem.update(time);
    }

    override getJumpForce(): number {
        return 300;
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

    override addParticles(where: Point): void {
        this._particleSystem.addParticles(where);
    }
}

export default PowerUpBean;