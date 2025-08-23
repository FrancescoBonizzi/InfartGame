import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {Point} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import BroccoloParticleSystem from "../particleEmitters/BroccoloParticleSystem.ts";

class PowerUpBroccolo extends PowerUp {

    // TODO: cambio aniamzione player

    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.verdura,
            position,
            new BroccoloParticleSystem(assets, world));
    }

    override getJumpForce(): number {
        return 500;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 400;
    }

    override getFillColor(): string {
        return '#a7ef17';
    }

    override getDurationMilliseconds(): number {
        return 3500;
    }

    override getMaxConsecutiveJumps(): number {
        return 1000;
    }

    override getParticleGenerationIntervalMilliseconds(): number {
        return 80;
    }
}

export default PowerUpBroccolo;