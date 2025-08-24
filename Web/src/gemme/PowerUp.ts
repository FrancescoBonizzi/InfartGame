import Gemma from "./Gemma.ts";
import Camera from "../world/Camera.ts";
import {AnimatedSprite, ColorSource, Point, Texture, Ticker} from "pixi.js";
import ParticleSystem, {deallocateEmptyParticleSystems} from "../particleEmitters/ParticleSystem.ts";
import PowerUpTypes from "./PowerUpTypes.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import PopupText from "../hud/PopupText.ts";

abstract class PowerUp extends Gemma {

    private readonly _particleSystemFactory: () => ParticleSystem;

    private _particleSystems: ParticleSystem[];
    private _hasBeenActivatedByPlayer: boolean = false;
    private _lastParticleEmissionTime: number = 0;
    private _popupText: PopupText | null = null;
    private readonly _assets: InfartAssets;

    protected constructor(
        camera: Camera,
        texture: Texture,
        assets: InfartAssets,
        position: Point,
        particleSystemFactory: () => ParticleSystem) {

        super(camera, texture, position);

        this._particleSystems = [];
        this._particleSystemFactory = particleSystemFactory;
        this._assets = assets;
    }

    override update(time: Ticker) {
        super.update(time);

        this._particleSystems.forEach(ps => ps.update(time));
        this._particleSystems = deallocateEmptyParticleSystems(this._particleSystems);
        if (this._popupText) {
            this._popupText.update(time);
        }
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

        const isExpired = this.isPowerUpTimeExpired();
        if (isExpired)
            this._popupText?.destroy();
        return isExpired;
    }

    public activate() {
        this._elapsedMilliseconds = 0;
        this._hasBeenActivatedByPlayer = true;
        this._popupText = new PopupText(
            this._camera,
            this._assets,
            this.sprite.position,
            this.getPopupText()
        )
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
    abstract getPowerUpType(): PowerUpTypes;
    abstract getPopupText(): string;

    public addParticles(where: Point) {

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