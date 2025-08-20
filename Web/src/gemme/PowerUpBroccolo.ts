import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {Point, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import BroccoloParticleSystem from "../particleEmitters/BroccoloParticleSystem.ts";

class PowerUpBroccolo extends PowerUp {

    private readonly _particleSystem: BroccoloParticleSystem;

    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.verdura,
            position);

        this._particleSystem = new BroccoloParticleSystem(assets, world);
    }

    override update(time: Ticker) {
        super.update(time);
        this._particleSystem.update(time);
    }

    override getJumpForce(): number {
        return 600;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 50;
    }

    override getFillColor(): string {
        return '#6B8E23';
    }

    override getDurationMilliseconds(): number {
        return 3500;
    }

    override getMaxConsecutiveJumps(): number {
        return 3;
    }

    override addParticles(where: Point): void {
        this._particleSystem.addParticles(where);
    }
}

export default PowerUpBroccolo;