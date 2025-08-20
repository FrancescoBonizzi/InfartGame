import Gemma from "./Gemma.ts";
import Camera from "../world/Camera.ts";
import {ColorSource, Point, Texture} from "pixi.js";

abstract class PowerUp extends Gemma {

    protected constructor(
        world: Camera,
        texture: Texture,
        position: Point) {

        super(world, texture, position);

    }

    abstract getJumpForce() : number;
    abstract getHorizontalMoveSpeedIncrease(): number;
    abstract getFillColor(): ColorSource;
    abstract getDurationMilliseconds(): number;
    abstract getMaxConsecutiveJumps(): number;
}

export default PowerUp;