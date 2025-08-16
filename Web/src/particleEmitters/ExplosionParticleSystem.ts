import ParticleSystem from "./ParticleSystem.ts";
import Camera from "../world/Camera.ts";
import {Texture} from "pixi.js";

class ExplosionParticleSystem extends ParticleSystem {

    constructor(
        texture: Texture,
        camera: Camera) {

        super(
            texture,
            camera,
            2,
            {min: 12, max: 24},
            { min: 20, max: 60 },
            { min: 50, max: 100 },
            {min: 1, max: 5},
            { min: 2.0, max: 4.0 },
            {min: 0.5, max: 1.0},
            { min: 0, max: 360 },
            null,
            undefined,
            true,
            {
                z0: 200,
                vz: 200,
                focalLen: 600,
                towardsCamera: null
            }
        );
    }
}

export default ExplosionParticleSystem;