import {BLEND_MODES, Point, Texture, Ticker} from "pixi.js";
import Particle from "./Particle.ts";
import Numbers from "../services/Numbers.ts";
import Interval from "../primitives/Interval.ts";
import Camera from "../world/Camera.ts";

export interface PerspectiveEffect {
    z0: number;
    vz: number;
    focalLen: number;
    towardsCamera: boolean | null;
}

abstract class ParticleSystem {

    private readonly _activeParticles: Particle[];

    protected _freeParticles: Particle[];
    private readonly _numParticles: Interval;
    private readonly _speed: Interval;
    private readonly _acceleration: Interval;
    private readonly _rotationSpeed: Interval;
    private readonly _lifetimeSeconds: Interval;
    private readonly _scale: Interval;
    private readonly _randomizedSpawnAngle: boolean;
    private readonly _spawnAngleDegrees: Interval;
    private readonly _perspectiveEffect: PerspectiveEffect | null;

    protected constructor(
        texture: Texture,
        camera: Camera,
        density: number,
        numParticles: Interval,
        speed: Interval,
        acceleration: Interval,
        rotationSpeed: Interval,
        lifetimeSeconds: Interval,
        scale: Interval,
        spawnAngleInDegrees: Interval,
        textureBlendMode: BLEND_MODES | undefined = undefined,
        randomizedSpawnAngle: boolean = false,
        perspectiveEffect: PerspectiveEffect | null = null) {

        this._numParticles = numParticles;
        this._speed = speed;
        this._acceleration = acceleration;
        this._rotationSpeed = rotationSpeed;
        this._lifetimeSeconds = lifetimeSeconds;
        this._scale = scale;
        this._spawnAngleDegrees = spawnAngleInDegrees;
        this._randomizedSpawnAngle = randomizedSpawnAngle;
        this._perspectiveEffect = perspectiveEffect;

        this._activeParticles = new Array(density * numParticles.max)
            .fill(null)
            .map(() => new Particle(
                texture,
                camera,
                textureBlendMode));

        this._freeParticles = [...this._activeParticles];
    }

    public update(time: Ticker) {
        this._activeParticles.forEach(particle => {
            if (particle.isActive) {
                particle.update(time);

                if (!particle.isActive) {
                    this._freeParticles.push(particle);
                }
            }
        });
    }

    public addParticles(position: Point) {

        const numParticles = Numbers.randomBetweenInterval(this._numParticles);
        for (let i = 0; i < numParticles && this._freeParticles.length > 0; i++) {
            const p = this._freeParticles.shift();
            if (p) {
                this.initializeParticle(p, position);
            }
        }

    }

    private initializeParticle(
        particle: Particle,
        position: Point) {

        const direction = this.pickRandomDirection()

        const speedScalar = Numbers.randomBetweenInterval(this._speed);
        const speed = new Point(
            direction.x * speedScalar,
            direction.y * speedScalar);

        const accelerationScalar = Numbers.randomBetweenInterval(this._acceleration);
        const acceleration = new Point(
            direction.x * accelerationScalar,
            direction.y * accelerationScalar);

        const usePerspective = this._perspectiveEffect !== null;
        const towardsCamera = usePerspective
            ? this._perspectiveEffect.towardsCamera !== null
                ? this._perspectiveEffect.towardsCamera
                : Math.random() < 0.5
            : false;
        const z0 = usePerspective && towardsCamera
            ? this._perspectiveEffect!.z0
            : 0;
        const vz = !usePerspective
            ? 0
            : towardsCamera
                ? -this._perspectiveEffect!.vz // NEGATIVO = verso camera
                : this._perspectiveEffect!.vz;

        particle.initialize(
            position,
            speed,
            acceleration,
            Numbers.randomBetweenInterval(this._rotationSpeed),
            "#FFFFFF",
            Numbers.randomBetweenInterval(this._scale),
            Numbers.randomBetweenInterval(this._lifetimeSeconds),
            this._randomizedSpawnAngle,
            this._perspectiveEffect !== null,
            z0,
            vz,
            600
        );
    }

    private pickRandomDirection(): Point {

        const randomAngle = Numbers.randomBetweenInterval(this._spawnAngleDegrees);
        const randomAngleInRadians = Numbers.toRadians(randomAngle);

        return new Point(
            Math.cos(randomAngleInRadians),
            -Math.sin(randomAngleInRadians));
    }

    isActive() {
        return this._freeParticles.length != this._activeParticles.length;
    }
}

export default ParticleSystem;