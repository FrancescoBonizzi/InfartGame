import PowerUp from "./PowerUp.ts";
import PowerUpTypes from "./PowerUpTypes.ts";
import Camera from "../world/Camera.ts";
import {Point} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";

class PowerUpBean extends PowerUp {
    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.bean,
            PowerUpTypes.Bean,
            position);

    }

    override getJumpForce(): number {
        return 300;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 0;
    }

    override getFillColor(): string {
        return '#D2691E';
    }

    override getDurationMilliseconds(): number {
        return 3500;
    }
}

export default PowerUpBean;