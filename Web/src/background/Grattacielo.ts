import {Rectangle, Sprite, Texture} from "pixi.js";
import Camera from "../world/Camera.ts";

class Grattacielo {

    private readonly _sprite: Sprite;

    constructor(
        texture: Texture,
        camera: Camera) {

        this._sprite = new Sprite(texture);
        camera.addToWorld(this._sprite);
        this._sprite.anchor.set(0, 1);
        this._sprite.y = 0;
    }

    get width() {
        return this._sprite.width;
    }

    get height() {
        return this._sprite.height;
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

    get collisionRectangle() {
        return new Rectangle(
            this._sprite.x,
            this._sprite.y - this._sprite.height,
            this._sprite.width,
            this._sprite.height);
    }

}

export default Grattacielo;