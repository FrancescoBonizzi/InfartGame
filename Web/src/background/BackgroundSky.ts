import InfartAssets from "../assets/InfartAssets.ts";
import {Sprite} from "pixi.js";
import Camera from "../world/Camera.ts";

class BackgroundSky {

    private readonly _sprite: Sprite;
    private _camera: Camera;

    constructor(
        assets: InfartAssets,
        camera: Camera) {

        this._camera = camera;
        this._sprite = new Sprite(assets.textures.background);
        this._sprite.anchor.set(0, 1);
        this._sprite.y = 0;
        this._sprite.scale.set(1.8);

        camera.addToWorld(this._sprite);
    }

    update() {
        this._sprite.x = this._camera.x;
    }
}

export default BackgroundSky;