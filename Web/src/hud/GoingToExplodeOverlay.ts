import InfartAssets from "../assets/InfartAssets.ts";
import {Application, Renderer, Sprite} from "pixi.js";

class GoingToExplodeOverlay {

    private readonly _sprite: Sprite;

    constructor(
        assets: InfartAssets,
        app: Application<Renderer>
    ) {

        this._sprite = new Sprite(assets.textures.deathScreen);
        this._sprite.scale.set(
            1,
            app.screen.height / this._sprite.height);
        this._sprite.zIndex = 10000;
        this._sprite.position.set(0, 0)

        app.stage.addChild(this._sprite);
    }
}

export default GoingToExplodeOverlay;