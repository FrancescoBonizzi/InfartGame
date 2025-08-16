import InfartAssets from "../assets/InfartAssets.ts";
import {Application, Renderer, Sprite, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";

class GoingToExplodeOverlay {

    private readonly _sprite: Sprite;
    private readonly _fadeDurationSeconds = 0.5;

    private _targetOpacity: number | null;
    private _elapsed: number;

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
        this._sprite.alpha = 0;

        app.stage.addChild(this._sprite);
        this._elapsed = 0;
        this._targetOpacity = null;
    }

    update(ticker: Ticker) {
        if (this._targetOpacity === null)
            return;

        this._elapsed += ticker.deltaMS;
        const t = Math.min(this._elapsed / this._fadeDurationSeconds, 1);
        this._sprite.alpha = Numbers.easeInCubic(t) * this._targetOpacity;
    }

    show() {
        this._targetOpacity = 1;
        this._elapsed = 0;
    }

    hide() {
        this._targetOpacity = 0;
        this._elapsed = 0;
    }

}

export default GoingToExplodeOverlay;