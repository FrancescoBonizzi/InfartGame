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
}

export default PowerUpBean;