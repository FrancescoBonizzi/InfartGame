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

    get x() {
        return this._sprite.x;
    }

    set x(x: number) {
        this._sprite.x = x;
    }

    get collisionRectangle() {
        const bounds = this._sprite.getBounds();
        return new Rectangle(
            bounds.x,
            bounds.y,
            bounds.width,
            bounds.height);
    }

}

export default Grattacielo;