import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {Point} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import BroccoloParticleSystem from "../particleEmitters/BroccoloParticleSystem.ts";

class PowerUpBroccolo extends PowerUp {

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
        return 600;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 50;
    }

    override getFillColor(): string {
        return '#6B8E23';
    }

    override getDurationMilliseconds(): number {
        return 3500;
    }

    override getMaxConsecutiveJumps(): number {
        return 3;
    }
}

export default PowerUpBroccolo;