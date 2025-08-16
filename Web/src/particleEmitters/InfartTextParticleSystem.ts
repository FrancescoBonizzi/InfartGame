import ParticleSystem from "./ParticleSystem.ts";
import Camera from "../world/Camera.ts";
import InfartAssets from "../assets/InfartAssets.ts";

class InfartTextParticleSystem extends ParticleSystem {

    constructor(
        assets: InfartAssets,
        camera: Camera) {

        super(
            assets.textures.bang,
            camera,
            1,
            {min: 1, max: 1},
            { min: 0, max: 0 },
            { min: 0, max: 0 },
            {min: 3, max: 3},
            { min: 6.0, max: 6.0 },
            {min: 1, max: 2},
            { min: 0, max: 0 },
            null,
            undefined,
            false,
            {
                z0: 200,
                vz: 140,
                focalLen: 600,
                towardsCamera: true
            }
        );
    }
}

export default InfartTextParticleSystem;