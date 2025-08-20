import PowerUp from "./PowerUp.ts";
import PowerUpTypes from "./PowerUpTypes.ts";
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
            PowerUpTypes.Broccolo,
            position);

    }

    override getJumpForce(): number {
        return 200;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 400;
    }

    override getFillColor(): string {
        return '#00FF00';
    }

    override getDurationMilliseconds(): number {
        return 3500;
    }
}

export default PowerUpBroccolo;