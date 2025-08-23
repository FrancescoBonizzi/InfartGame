import Gemma from "./Gemma.ts";
import Camera from "../world/Camera.ts";
import {ColorSource, Point, Texture, Ticker} from "pixi.js";
import ParticleSystem from "../particleEmitters/ParticleSystem.ts";

abstract class PowerUp extends Gemma {

    private _hasBeenActivatedByPlayer: boolean = false;
    private readonly _particleSystem: ParticleSystem;

    protected constructor(
        world: Camera,
        texture: Texture,
        position: Point,
        particleSystem: ParticleSystem) {

        super(world, texture, position);
        this._particleSystem = particleSystem;

    }

    override update(time: Ticker) {
        super.update(time);
        this._particleSystem.update(time);
    }

    private isPowerUpTimeExpired(): boolean {
        return this._elapsedMilliseconds > this.getDurationMilliseconds();
    }

    public isExpired(): boolean {

        if (!this._hasBeenActivatedByPlayer) {
            return false;
        }

        if (this._particleSystem.isActive())
            return false;

        return this.isPowerUpTimeExpired();
    }

    public activate() {
        this._elapsedMilliseconds = 0;
        this._hasBeenActivatedByPlayer = true;
    }

    public get hasBeenActivatedByPlayer() {
        return this._hasBeenActivatedByPlayer;
    }

    abstract getJumpForce() : number;
    abstract getHorizontalMoveSpeedIncrease(): number;
    abstract getFillColor(): ColorSource;
    abstract getDurationMilliseconds(): number;
    abstract getMaxConsecutiveJumps(): number;

    public addParticles(where: Point) {
        if (this.isPowerUpTimeExpired())
            return;

        this._particleSystem.addParticles(where);
    }
}

export default PowerUp;