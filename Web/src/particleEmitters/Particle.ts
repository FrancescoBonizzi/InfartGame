import {BLEND_MODES, ColorSource, Point, Sprite, Texture, Ticker} from "pixi.js";
import Camera from "../world/Camera.ts";

class Particle {

    private readonly _sprite: Sprite;

    private _speed: Point;
    private _acceleration: Point;
    private _timeSinceStartSeconds: number;
    private _lifeTimeSeconds: number;
    private _rotationSpeed: number;
    private _initialScaleScalar: number;

    constructor(texture: Texture, camera: Camera, blendMode?: BLEND_MODES | undefined) {
        this._sprite = new Sprite({
            texture,
            blendMode: blendMode
        });
        this._rotationSpeed = 0;
        this._speed = new Point(0, 0);
        this._acceleration = new Point(0, 0);
        this._timeSinceStartSeconds = 0;
        this._lifeTimeSeconds = 0;
        this._initialScaleScalar = this._sprite.scale.x;

        camera.addToWorld(this._sprite);
    }

    public initialize(
        position: Point,
        speed: Point,
        acceleration: Point,
        rotationSpeed: number,
        color: ColorSource,
        scale: number,
        lifetimeSeconds: number,
        randomizedSpawnAngle: boolean
    ) {

        this._speed = speed;
        this._acceleration = acceleration;
        this._rotationSpeed = rotationSpeed;
        this._lifeTimeSeconds = lifetimeSeconds;
        this._timeSinceStartSeconds = 0.0;
        this._initialScaleScalar = scale;

        this._sprite.position = position;
        this._sprite.rotation = randomizedSpawnAngle
            ? Math.random() * Math.PI * 2
            : 0;
        this._sprite.tint = color;
        this._sprite.anchor.set(0.5, 0.5);
        this._sprite.scale.set(this._initialScaleScalar);
    }

    public get isActive(): boolean {
        return this._timeSinceStartSeconds < this._lifeTimeSeconds;
    }

    public update(time: Ticker) {

        let elapsedSeconds = time.elapsedMS / 1000;

        this._speed.x += this._acceleration.x * elapsedSeconds;
        this._speed.y += this._acceleration.y * elapsedSeconds;

        this._sprite.position.x += this._speed.x * elapsedSeconds;
        this._sprite.position.y += this._speed.y * elapsedSeconds
        this._sprite.rotation += this._rotationSpeed * elapsedSeconds;

        const normalizedLifeTime = this._timeSinceStartSeconds / this._lifeTimeSeconds;
        this._sprite.alpha = 4 * normalizedLifeTime * (1 - normalizedLifeTime);

        const scale = this._initialScaleScalar * (.75 + .25 * normalizedLifeTime);
        this._sprite.scale.set(scale);

        this._timeSinceStartSeconds += elapsedSeconds;

        if (!this.isActive) {
            this._sprite.alpha = 0;
        }
    }
}

export default Particle;