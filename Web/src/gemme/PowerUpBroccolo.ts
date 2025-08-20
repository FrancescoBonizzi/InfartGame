import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {Point} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";

class PowerUpBroccolo extends PowerUp {
    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.verdura,
            position);

    }

    override getJumpForce(): number {
        return 500;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 50;
    }

    override getFillColor(): string {
        return '#00FF00';
    }

    override getDurationMilliseconds(): number {
        return 3500;
    }

    override getMaxConsecutiveJumps(): number {
        return 2;
    }
}

export default PowerUpBroccolo;