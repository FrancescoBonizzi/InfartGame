import {ColorSource, Point, Sprite, Texture, Ticker} from "pixi.js";
import Camera from "../world/Camera.ts";

class Particle {

    private readonly _sprite: Sprite;

    private _speed: Point;
    private _acceleration: Point;
    private _timeSinceStart: number;
    private _lifeTime: number;
    private _rotationSpeed: number;

    constructor(texture: Texture, camera: Camera) {
        this._sprite = new Sprite(texture);
        this._rotationSpeed = 0;
        this._speed = new Point(0, 0);
        this._acceleration = new Point(0, 0);
        this._timeSinceStart = 0;
        this._lifeTime = 0;

        camera.addToWorld(this._sprite);
    }

    public initialize(
        position: Point,
        speed: Point,
        acceleration: Point,
        rotationSpeed: number,
        color: ColorSource,
        scale: number,
        lifetime: number
    ) {

        this._speed = speed;
        this._acceleration = acceleration;
        this._rotationSpeed = rotationSpeed;
        this._lifeTime = lifetime;
        this._timeSinceStart = 0.0;

        this._sprite.position = position;
        this._sprite.rotation = Math.random() * Math.PI * 2;
        this._sprite.tint = color;
        this._sprite.scale.set(scale);
    }

    public get isActive(): boolean {
        return this._timeSinceStart < this._lifeTime
            || this._sprite.alpha < 0;
    }

    public update(time: Ticker) {

        const elapsedSeconds = time.elapsedMS / 1000;

        this._speed.x += this._acceleration.x * elapsedSeconds;
        this._speed.y += this._acceleration.y * elapsedSeconds;

        this._sprite.position.x += this._speed.x * elapsedSeconds;
        this._sprite.position.y += this._speed.y * elapsedSeconds
        this._sprite.rotation += this._rotationSpeed * elapsedSeconds;

        const normalizedLifeTime = this._timeSinceStart / this._lifeTime;
  //      this._sprite.alpha = 4 * normalizedLifeTime * (1 - normalizedLifeTime);

        const scale = this._sprite.scale.x * (.75 + .25 * normalizedLifeTime);
        this._sprite.scale.set(scale);

        this._timeSinceStart += elapsedSeconds;
    }
}

export default Particle;