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
        density: number) {

        super(
            texture,
            camera,
            density,
            numParticles,
            { min: 20, max: 120 },
            { min: 100, max: 200 },
            rotationSpeed,
            { min: 1.4, max: 2.0 },
            { min: 0.3, max: 0.5 },
            { min: 0, max: 360 },
            null
        );
    }
}

export default ExplosionParticleSystem;