import {BLEND_MODES, Point, Texture, Ticker} from "pixi.js";
import Particle from "./Particle.ts";
import Numbers from "../services/Numbers.ts";
import Interval from "../primitives/Interval.ts";
import Camera from "../world/Camera.ts";
import PixiJsTimer from "../primitives/PixiJsTimer.ts";

abstract class ParticleSystem {

    private readonly _activeParticles: Particle[];

    protected _freeParticles: Particle[];
    private readonly _numParticles: Interval;
    private readonly _speed: Interval;
    private readonly _acceleration: Interval;
    private readonly _rotationSpeed: Interval;
    private readonly _lifetimeSeconds: Interval;
    private readonly _scale: Interval;
    private readonly _spawnAngleDegrees: Interval;

    private readonly _particlesGenerationTimer: PixiJsTimer | null;

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
        particlesGenerationIntervalMilliseconds: number | null = null,
        textureBlendMode: BLEND_MODES | undefined = undefined) {

        this._numParticles = numParticles;
        this._speed = speed;
        this._acceleration = acceleration;
        this._rotationSpeed = rotationSpeed;
        this._lifetimeSeconds = lifetimeSeconds;
        this._scale = scale;
        this._spawnAngleDegrees = spawnAngleInDegrees;

        this._activeParticles = new Array(density * numParticles.max)
            .fill(null)
            .map(() => new Particle(
                texture,
                camera,
                textureBlendMode));

        this._freeParticles = [...this._activeParticles];

        this._particlesGenerationTimer = particlesGenerationIntervalMilliseconds !== null
            ? new PixiJsTimer(particlesGenerationIntervalMilliseconds)
            : null;
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

        this._particlesGenerationTimer?.update(time);
    }

    public addParticles(position: Point) {

        if (!this._particlesGenerationTimer
            || this._particlesGenerationTimer.canRunManualCallback()) {

            const numParticles = Numbers.randomBetweenInterval(this._numParticles);
            for (let i = 0; i < numParticles && this._freeParticles.length > 0; i++) {
                const p = this._freeParticles.shift();
                if (p) {
                    this.initializeParticle(p, position);
                }
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

        particle.initialize(
            position,
            speed,
            acceleration,
            Numbers.randomBetweenInterval(this._rotationSpeed),
            "#FFFFFF",
            Numbers.randomBetweenInterval(this._scale),
            Numbers.randomBetweenInterval(this._lifetimeSeconds)
        );
    }

    private pickRandomDirection(): Point {

        const randomAngle = Numbers.randomBetweenInterval(this._spawnAngleDegrees);
        const randomAngleInRadians = Numbers.toRadians(randomAngle);

        return new Point(
            Math.cos(randomAngleInRadians),
            -Math.sin(randomAngleInRadians));
    }
}

export default ParticleSystem;