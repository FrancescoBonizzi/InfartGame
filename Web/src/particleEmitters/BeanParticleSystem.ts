import ParticleSystem from "./ParticleSystem.ts";
import Camera from "../world/Camera.ts";
import InfartAssets from "../assets/InfartAssets.ts";

class BeanParticleSystem extends ParticleSystem {

    constructor(
        assets: InfartAssets,
        camera: Camera) {

        super(
            assets.textures.particles.caccaParticle,
            camera,
            10,
            {min: 8, max: 16},
            {min: 300, max: 400},
            {min: 200, max: 300},
            {min: -Math.PI, max: Math.PI},
            {min: 0.5, max: 0.8},
            {min: 0.5, max: 1.0},
            {min: 270, max: 320},
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

export default BeanParticleSystem;