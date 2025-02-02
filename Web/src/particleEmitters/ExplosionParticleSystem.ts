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
            {min: 10, max: 20},
            {min: 100, max: 200},
            {min: 50, max: 100},
            {min: 0, max: 2 * Math.PI},
            {min: 0.8, max: 1.0},
            {min: 0.3, max: 0.5},
            {min: 255, max: 255},
            null
        );
    }
}

export default ExplosionParticleSystem;