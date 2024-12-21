import Gemma from "./Gemma.ts";
import PowerUpTypes from "./PowerUpTypes.ts";
import Camera from "../world/Camera.ts";
import {Texture} from "pixi.js";

class PowerUp extends Gemma {

    private readonly _powerUpType: PowerUpTypes;

    constructor(
        world: Camera,
        texture: Texture,
        powerUpType: PowerUpTypes) {

        super(world, texture);
        this._powerUpType = powerUpType;

    }

    get powerUpType() {
        return this._powerUpType;
    }

}

export default PowerUp;