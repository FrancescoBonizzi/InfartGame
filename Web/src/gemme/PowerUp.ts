import Gemma from "./Gemma.ts";
import Camera from "../world/Camera.ts";
import {ColorSource, Point, Texture} from "pixi.js";

abstract class PowerUp extends Gemma {

    private _hasBeenActivatedByPlayer: boolean = false;

    protected constructor(
        world: Camera,
        texture: Texture,
        position: Point) {

        super(world, texture, position);

    }

    public isExpired(): boolean {

        if (!this._hasBeenActivatedByPlayer) {
            return false;
        }

        return this._elapsedMilliseconds > this.getDurationMilliseconds();
    }

    public activate() {
        this._elapsedMilliseconds = 0;
        this._hasBeenActivatedByPlayer = true;
    }

    public get hasBeenActivatedByPlayer() {
        return this._hasBeenActivatedByPlayer;
    }

    abstract getJumpForce() : number;
    abstract getHorizontalMoveSpeedIncrease(): number;
    abstract getFillColor(): ColorSource;
    abstract getDurationMilliseconds(): number;
    abstract getMaxConsecutiveJumps(): number;
}

export default PowerUp;