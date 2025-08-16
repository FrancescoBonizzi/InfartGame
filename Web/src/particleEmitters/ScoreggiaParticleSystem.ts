import ParticleSystem from "./ParticleSystem.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";

class ScoreggiaParticleSystem extends ParticleSystem {

    constructor(
        infartAssets: InfartAssets,
        camera: Camera) {

        super(
            infartAssets.textures.particles.scoreggiaParticle,
            camera,
            5,
            {min: 4, max: 8},
            {min: 50, max: 80},
            {min: 30, max: 50},
            {min: -Math.PI / 4 / 2, max: Math.PI / 4 / 2},
            {min: 0.5, max: 0.7},
            {min: 0.3, max: 1.0},
            {min: 210, max: 215},
            null,
            'screen',
            true,
            {
                z0: 200,
                vz: 300,
                focalLen: 600,
                towardsCamera: null
            }
        );
    }

}

export default ScoreggiaParticleSystem;