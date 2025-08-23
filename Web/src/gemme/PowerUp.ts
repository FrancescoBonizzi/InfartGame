import Gemma from "./Gemma.ts";
import Camera from "../world/Camera.ts";
import {AnimatedSprite, ColorSource, Point, Texture, Ticker} from "pixi.js";
import ParticleSystem, {deallocateEmptyParticleSystems} from "../particleEmitters/ParticleSystem.ts";

abstract class PowerUp extends Gemma {

    private readonly _particleSystemFactory: () => ParticleSystem;

    private _particleSystems: ParticleSystem[];
    private _hasBeenActivatedByPlayer: boolean = false;
    private _lastParticleEmissionTime: number = 0;

    protected constructor(
        world: Camera,
        texture: Texture,
        position: Point,
        particleSystemFactory: () => ParticleSystem) {

        super(world, texture, position);
        this._particleSystems = [];
        this._particleSystemFactory = particleSystemFactory;
    }

    override update(time: Ticker) {
        super.update(time);

        this._particleSystems.forEach(ps => ps.update(time));
        this._particleSystems = deallocateEmptyParticleSystems(this._particleSystems);
    }

    private isPowerUpTimeExpired(): boolean {
        return this._elapsedMilliseconds > this.getDurationMilliseconds();
    }

    public isExpired(): boolean {

        if (!this._hasBeenActivatedByPlayer) {
            return false;
        }

        if (this._particleSystems.length > 0) {
            return false;
        }

        return this.isPowerUpTimeExpired();
    }

    public activate() {
        this._elapsedMilliseconds = 0;
        this._hasBeenActivatedByPlayer = true;
    }

    public get hasBeenActivatedByPlayer() {
        return this._hasBeenActivatedByPlayer;
    }

    abstract getParticleGenerationIntervalMilliseconds(): number;
    abstract getJumpForce() : number;
    abstract getHorizontalMoveSpeedIncrease(): number;
    abstract getFillColor(): ColorSource;
    abstract getDurationMilliseconds(): number;
    abstract getMaxConsecutiveJumps(): number;
    abstract getPlayerAnimation(): AnimatedSprite | null;

    public addParticles(where: Point) {

        if (this.isPowerUpTimeExpired())
            return;

        const intervalMilliseconds = this.getParticleGenerationIntervalMilliseconds();
        if (this._elapsedMilliseconds - this._lastParticleEmissionTime >= intervalMilliseconds) {
            this._lastParticleEmissionTime = this._elapsedMilliseconds;
            const particleSystem = this._particleSystemFactory();
            particleSystem.addParticles(where);
            this._particleSystems.push(particleSystem);
        }
    }
}

export default PowerUp;