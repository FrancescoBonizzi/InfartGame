import {Point, Rectangle, Sprite, Texture, Ticker} from "pixi.js";
import Camera from "../world/Camera.ts";
import IHasCollisionRectangle from "../IHasCollisionRectangle.ts";
import Numbers from "../services/Numbers.ts";

class Gemma implements IHasCollisionRectangle {

    private readonly _sprite: Sprite;
    private readonly _startingPosition: Point;
    protected _elapsedMilliseconds: number;

    private readonly _amplitude: number;
    private readonly _angularSpeed: number = 0.004;
    private readonly _randomOffset: number;

    constructor(
        world: Camera,
        texture: Texture,
        position: Point) {

        const sprite = new Sprite(texture);
        world.addToWorld(sprite);
        this._startingPosition = position;
        this._elapsedMilliseconds = 0;
        this._randomOffset = Numbers.randomBetween(0, 50);

        this._sprite = sprite;
        this._sprite.x = position.x;
        this._sprite.y = position.y;
        this._sprite.anchor.set(0.5, 0);

        this._amplitude = Numbers.randomBetween(1, 12);
    }

    get sprite() {
        return this._sprite;
    }

    get x() {
        return this._sprite.x;
    }

    get width() {
        return this._sprite.width;
    }

    get collisionRectangle() {
        return new Rectangle(
            this._sprite.x - this._sprite.width / 2,
            this._sprite.y,
            this._sprite.width,
            this._sprite.height
        );
    }

    update(time: Ticker) {
        this._elapsedMilliseconds += time.deltaMS;
        this._sprite.y = this._startingPosition.y
            + Math.sin((this._elapsedMilliseconds + this._randomOffset) * this._angularSpeed)
            * this._amplitude;
    }
}

export default Gemma;