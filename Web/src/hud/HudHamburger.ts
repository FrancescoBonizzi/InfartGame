import {Application, Point, Renderer, Sprite, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import Numbers from "../services/Numbers.ts";

enum HamburgerState {
    Active,
    Idle,
}

interface Animation {
    elapsedMs: number;
    fromOpacity: number;
    toOpacity: number;
    fromScale: number;
    toScale: number;
}

class HudHamburger {
    private _app: Application<Renderer>;
    private readonly _sprite: Sprite;

    private readonly _animationDurationMs = 180;

    private readonly _scaleIdle = 0.92;
    private readonly _scaleActive = 1.0;
    private _currentScale = this._scaleIdle;

    private readonly _opacityIdle = 0.5;
    private readonly _opacityActive = 1;
    private readonly _currencyOpacity = this._opacityIdle;

    private _currentAnimation: Animation | null;

    constructor(app: Application<Renderer>, position: Point, assets: InfartAssets) {
        this._app = app;

        this._sprite = new Sprite(assets.textures.burger);
        this._sprite.anchor.set(0.5, 1);
        this._sprite.position.copyFrom(position);

        this._currentAnimation = null;
        this._sprite.scale.set(this._currentScale);
        this._sprite.alpha = this._currencyOpacity;

        this._app.stage.addChild(this._sprite);
    }

    update(time: Ticker) {

        if (this._currentAnimation) {
            this._currentAnimation.elapsedMs = Math.min(
                this._currentAnimation.elapsedMs + time.deltaMS,
                this._animationDurationMs);
            const animationProgress = this._currentAnimation.elapsedMs / this._animationDurationMs;
            const animationProgressEased = Numbers.easeOutCubic(animationProgress);
            this._currentScale = Numbers.lerp(
                this._currentAnimation.fromScale,
                this._currentAnimation.toScale,
                animationProgressEased);

            this._sprite.scale.set(this._currentScale);
            this._sprite.alpha = Numbers.lerp(
                this._currentAnimation.fromOpacity,
                this._currentAnimation.toOpacity,
                animationProgressEased);

            if (animationProgress >= 1)
                this._currentAnimation = null;
        }
    }

    activate() {
        this.setState(HamburgerState.Active);
    }

    deactivate() {
        this.setState(HamburgerState.Idle);
    }

    private setState(next: HamburgerState) {

        const fromScale = next === HamburgerState.Active
            ? this._scaleIdle
            : this._scaleActive;
        const toScale = next === HamburgerState.Active
            ? this._scaleActive
            : this._scaleIdle;
        const fromOpacity = next === HamburgerState.Active
            ? this._opacityIdle
            : this._opacityActive;
        const toOpacity = next === HamburgerState.Active
            ? this._opacityActive
            : this._opacityIdle;

        this._currentAnimation = {
            elapsedMs: 0,
            fromScale: fromScale,
            toScale: toScale,
            fromOpacity: fromOpacity,
            toOpacity: toOpacity
        };
    }
}

export default HudHamburger;