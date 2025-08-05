import {Application, ColorMatrixFilter, Point, Renderer, Sprite} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";

enum HamburgerState {
    Active,
    Idle,
}

class HudHamburger {

    private _state: HamburgerState;
    private _sprite: Sprite;

    constructor(
        app: Application<Renderer>,
        position: Point,
        assets: InfartAssets) {

        this._sprite = new Sprite(assets.textures.burger);
        this._sprite.anchor.set(0.5, 1);
        this._sprite.x = position.x;
        this._sprite.y = position.y;
        app.stage.addChild(this._sprite);
        this.deactivate();

        this._state = HamburgerState.Idle;
    }

    activate() {
        this._state = HamburgerState.Active;
        this._sprite.filters = [];
    }

    deactivate() {
        this._state = HamburgerState.Idle;
        const colorMatrix = new ColorMatrixFilter();
        colorMatrix.desaturate();
        this._sprite.filters = [colorMatrix];
    }

    // TODO: update per animazione activate in e out
}

export default HudHamburger;