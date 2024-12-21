import {Point, Rectangle, Sprite, Texture, Ticker} from "pixi.js";
import Camera from "../world/Camera.ts";
import IHasCollisionRectangle from "../IHasCollisionRectangle.ts";

class Gemma implements IHasCollisionRectangle {

    private readonly _sprite: Sprite;
    private readonly _collisionRectangleOffsetFactor = 40;
    private readonly _startingPosition: Point;

    constructor(
        world: Camera,
        texture: Texture,
        position: Point) {

        const sprite = new Sprite(texture);
        world.addToWorld(sprite);
        this._sprite = sprite;
        this._startingPosition = position;
        this._sprite.x = position.x;
        this._sprite.y = position.y;
        this._sprite.anchor.set(0.5, 0);
    }

    get x() {
        return this._sprite.x;
    }

    get width() {
        return this._sprite.width;
    }

    get collisionRectangle() {
        return new Rectangle(
            this._sprite.x + 15,
            this._sprite.y,
            this._sprite.width - this._collisionRectangleOffsetFactor,
            this._sprite.height - this._collisionRectangleOffsetFactor);
    }

    update(time: Ticker) {
        // TODO: farli oscillare verticalmente
        //this._sprite.y = this._startingPosition.y * Math.sin(10 * time.deltaTime);
    }
}

export default Gemma;