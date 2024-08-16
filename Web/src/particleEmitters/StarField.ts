import ParticleSystem from "./ParticleSystem.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";

class StarField extends ParticleSystem {

    constructor(
        infartAssets: InfartAssets,
        camera: Camera) {

        super(
            infartAssets.textures.particles.starParticle,
            camera,
            8,
            {min: 4, max: 8},
            {min: 10, max: 15},
            {min: 30, max: 40},
            {min: -Math.PI / 2, max: Math.PI / 2},
            {min: 1.5, max: 3.0},
            {min: 0.2, max: 1.0},
            {min: 0, max: 360},
            20
        );
    }
}

export default StarField;