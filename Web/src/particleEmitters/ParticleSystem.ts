import {Point, Texture, Ticker} from "pixi.js";
import Particle from "./Particle.ts";
import Numbers from "../services/Numbers.ts";
import Interval from "../primitives/Interval.ts";
import Camera from "../world/Camera.ts";

abstract class ParticleSystem {

    private readonly _activeParticles: Particle[];

    protected _freeParticles: Particle[];
    private readonly _numParticles: Interval;
    private readonly _speed: Interval;
    private readonly _acceleration: Interval;
    private readonly _rotationSpeed: Interval;
    private readonly _lifetime: Interval;
    private readonly _scale: Interval;
    private readonly _spawnAngle: Interval;

    protected constructor(
        texture: Texture,
        camera: Camera,
        density: number,
        numParticles: Interval,
        speed: Interval,
        acceleration: Interval,
        rotationSpeed: Interval,
        lifetime: Interval,
        scale: Interval,
        spawnAngle: Interval) {

        this._numParticles = numParticles;
        this._speed = speed;
        this._acceleration = acceleration;
        this._rotationSpeed = rotationSpeed;
        this._lifetime = lifetime;
        this._scale = scale;
        this._spawnAngle = spawnAngle;

        this._freeParticles = new Array(density * numParticles.max)
            .fill(null)
            .map(() => new Particle(texture, camera));

        this._activeParticles = [...this._freeParticles];
    }

    public update(time: Ticker) {
        this._activeParticles.forEach(particle => {
            if (particle.isActive) {
                particle.update(time);

                if(!particle.isActive) {
                    this._freeParticles.push(particle);
                }
            }
        });
    }

    public addParticles(position: Point) {
        const numParticles = Numbers.randomBetweenInterval(this._numParticles);
        for (let i = 0; i < numParticles && this._freeParticles.length > 0; i++) {
            const p = this._freeParticles.pop();
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

        particle.initialize(
            position,
            speed,
            acceleration,
            Numbers.randomBetweenInterval(this._rotationSpeed),
            "#FFFFFF",
            Numbers.randomBetweenInterval(this._scale),
            Numbers.randomBetweenInterval(this._lifetime)
        );
    }

    private pickRandomDirection(): Point {
        return new Point(
            Math.cos(Numbers.randomBetweenInterval(this._spawnAngle)),
            -Math.sin(Numbers.randomBetweenInterval(this._spawnAngle)));
    }
}

export default ParticleSystem;