import InfartAssets from "../assets/InfartAssets.ts";
import {Sprite} from "pixi.js";
import Camera from "../world/Camera.ts";

class BackgroundSky {

    private readonly _sprite: Sprite;
    private readonly _zoom1Scale = 1.8;

    private _camera: Camera;

    constructor(
        assets: InfartAssets,
        camera: Camera) {

        this._camera = camera;
        this._sprite = new Sprite(assets.textures.background);
        this._sprite.anchor.set(0, 1);
        this._sprite.y = 0;
        this._sprite.scale.set(this._zoom1Scale);

        camera.addToWorld(this._sprite);
    }

    update() {
        this._sprite.x = this._camera.x;
        this._sprite.y = this._camera.y;

       // const currentZoom = this._camera.getZoom();
       // this._sprite.scale.set(this._zoom1Scale / currentZoom);
    }
}

export default BackgroundSky;