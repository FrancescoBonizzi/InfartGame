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

    private _usePerspective = false;
    private _z = 0;
    private _vz = 0;
    private _focalLen = 600;

    constructor(texture: Texture, camera: Camera, blendMode?: BLEND_MODES) {
        this._sprite = new Sprite({
            texture,
            blendMode: blendMode ?? 'normal'
        });
        this._rotationSpeed = 0;
        this._speed = new Point(0, 0);
        this._acceleration = new Point(0, 0);
        this._timeSinceStartSeconds = 0;
        this._lifeTimeSeconds = 0;
        this._initialScaleScalar = this._sprite.scale.x;

        camera.addToWorld(this._sprite);
    }

    public get sprite() {
        return this._sprite;
    }

    /**
     * Initializes the object with the given parameters.
     *
     * @param {Point} position - Initial position of the object in 2D space.
     * @param {Point} speed - Initial velocity vector.
     * @param {Point} acceleration - Acceleration vector applied to the object.
     * @param {number} rotationSpeed - Speed of rotation in radians per unit time.
     * @param {ColorSource} color - Color applied to the object.
     * @param {number} scale - Initial scaling factor.
     * @param {number} lifetimeSeconds - Lifetime of the object in seconds.
     * @param {boolean} randomizedSpawnAngle - Defines if the initial rotation angle should be randomized.
     * @param {boolean} [usePerspective=false] - Determines whether perspective effects are applied.
     * @param {number} [z0=0] - Initial z-coordinate for perspective effect.
     * @param {number} [vz=0] - Initial velocity along the z-axis.
     * @param {number} [focalLen=600] - Focal length for perspective projection.
     * @return {void} No value is returned.
     */
    public initialize(
        position: Point,
        speed: Point,
        acceleration: Point,
        rotationSpeed: number,
        color: ColorSource,
        scale: number,
        lifetimeSeconds: number,
        randomizedSpawnAngle: boolean,
        usePerspective: boolean,
        z0: number,
        vz: number,
        focalLen: number
    ): void {

        this._speed = speed;
        this._acceleration = acceleration;
        this._rotationSpeed = rotationSpeed;
        this._lifeTimeSeconds = lifetimeSeconds;
        this._timeSinceStartSeconds = 0.0;
        this._initialScaleScalar = scale;

        this._sprite.position.copyFrom(position);
        this._sprite.rotation = randomizedSpawnAngle
            ? Math.random() * Math.PI * 2
            : 0;
        this._sprite.tint = color;
        this._sprite.anchor.set(0.5, 0.5);

        // set prospettiva
        this._usePerspective = usePerspective;
        this._z = z0;
        this._vz = vz;
        this._focalLen = focalLen;

        // scala iniziale
        this._sprite.scale.set(scale);
    }

    public get isActive(): boolean {
        return this._timeSinceStartSeconds < this._lifeTimeSeconds;
    }

    public update(time: Ticker) {
        const dt = time.deltaMS / 1000;

        this._speed.x += this._acceleration.x * dt;
        this._speed.y += this._acceleration.y * dt;
        this._sprite.position.x += this._speed.x * dt;
        this._sprite.position.y += this._speed.y * dt;
        this._sprite.rotation += this._rotationSpeed * dt;

        const t = this._lifeTimeSeconds > 0 ? (this._timeSinceStartSeconds / this._lifeTimeSeconds) : 1;
        const tn = t < 0 ? 0 : t > 1 ? 1 : t;
        this._sprite.alpha = 4 * tn * (1 - tn);
        this._timeSinceStartSeconds += dt;

        let scale = this._initialScaleScalar * (0.75 + 0.25 * tn);

        // prospettiva (opzionale)
        if (this._usePerspective) {
            this._z += this._vz * dt;                       // vz < 0 = verso camera
            const zClamped = Math.max(-this._focalLen * 0.8, Math.min(this._z, 5000));
            const persp = this._focalLen / (this._focalLen + zClamped);
            scale *= persp;
            this._sprite.zIndex = (this._focalLen - zClamped) | 0;
        }

        this._sprite.scale.set(scale);

        if (!this.isActive)
            this._sprite.alpha = 0;
    }
}

export default Particle;