import ParticleSystem from "./ParticleSystem.ts";
import Camera from "../world/Camera.ts";
import InfartAssets from "../assets/InfartAssets.ts";

class JalapenoParticleSystem extends ParticleSystem {

    constructor(
        assets: InfartAssets,
        camera: Camera) {

        super(
            assets.textures.particles.jalapenoParticle,
            camera,
            10,
            {min: 4, max: 8},
            {min: 50, max: 80},
            {min: 30, max: 50},
            {min: -Math.PI / 8, max: Math.PI / 8},
            {min: 0.5, max: 0.7},
            {min: 0.5, max: 1.0},
            {min: 195, max: 280},
            'screen',
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

export default JalapenoParticleSystem;