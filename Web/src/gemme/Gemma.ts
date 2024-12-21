import {Rectangle, Sprite, Texture, Ticker} from "pixi.js";
import Camera from "../world/Camera.ts";
import IHasCollisionRectangle from "../IHasCollisionRectangle.ts";

class Gemma implements IHasCollisionRectangle {

    private readonly _sprite: Sprite;
    private _elapsed: number;
    private _ySpeed: number;

    constructor(
        world: Camera,
        texture: Texture) {

        const sprite = new Sprite(texture);
        world.addToWorld(sprite);
        this._sprite = sprite;

        this._elapsed = 0;
        this._ySpeed = 1;
    }

    get x() {
        return this._sprite.x;
    }

    set x(x: number) {
        this._sprite.x = x;
    }

    get y() {
        return this._sprite.y;
    }

    set y(y: number) {
        this._sprite.y = y;
    }

    get width() {
        return this._sprite.width;
    }

    get height() {
        return this._sprite.height;
    }

    get collisionRectangle() {
        return new Rectangle(
            this._sprite.x,
            this._sprite.y - this._sprite.height,
            this._sprite.width,
            this._sprite.height);
    }

    update(time: Ticker) {
        this._elapsed += time.deltaTime;

        if (this._elapsed >= 0.4) {
            this._ySpeed *= -1;
            this._elapsed = 0.0;
        }

        this._sprite.y += time.deltaTime * this._ySpeed;

        // TODO: usare Seno per non dover gestire manualmente le oscillazioni
        //  this._sprite.y += Math.sin(this._elapsed) * (time.deltaTime * 3);
        //  Regola il moltiplicatore per la velocità
    }
}

export default Gemma;