import Gemma from "./Gemma.ts";
import PowerUpTypes from "./PowerUpTypes.ts";
import Camera from "../world/Camera.ts";
import {ColorSource, Point, Texture} from "pixi.js";

abstract class PowerUp extends Gemma {

    private readonly _powerUpType: PowerUpTypes;

    protected constructor(
        world: Camera,
        texture: Texture,
        powerUpType: PowerUpTypes,
        position: Point) {

        super(world, texture, position);
        this._powerUpType = powerUpType;

    }

    get powerUpType() {
        return this._powerUpType;
    }

    abstract getJumpForce() : number;
    abstract getHorizontalMoveSpeedIncrease(): number;
    abstract getFillColor(): ColorSource;
    abstract getDurationMilliseconds(): number;
}

export default PowerUp;