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
            20,
            { min: 12, max: 24 },
            { min: 20, max: 120 },
            { min: 100, max: 200 },
            { min: 0, max: 360 },      
            { min: 1.4, max: 2.0 },
            { min: 0.3, max: 0.5 },
            { min: 0, max: 360 },
            null
        );
    }
}

export default ExplosionParticleSystem;