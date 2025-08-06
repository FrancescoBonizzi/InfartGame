import ParticleSystem from "./ParticleSystem.ts";
import Camera from "../world/Camera.ts";
import {Texture} from "pixi.js";
import Interval from "../primitives/Interval.ts";

class ExplosionParticleSystem extends ParticleSystem {

    constructor(
        texture: Texture,
        camera: Camera,
        rotationSpeed: Interval,
        numParticles: Interval,
        density: number,
        scale: Interval,
        speed: Interval,
        randomizedSpawnAngle: boolean) {

        super(
            texture,
            camera,
            density,
            numParticles,
            speed,
            { min: 50, max: 100 },
            rotationSpeed,
            { min: 2.0, max: 4.0 },
            scale,
            { min: 0, max: 360 },
            null,
            undefined,
            randomizedSpawnAngle
        );
    }
}

export default ExplosionParticleSystem;