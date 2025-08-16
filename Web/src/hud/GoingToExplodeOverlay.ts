import InfartAssets from "../assets/InfartAssets.ts";
import {Application, Renderer, Sprite, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import SoundManager from "../services/SoundManager.ts";

class GoingToExplodeOverlay {

    private readonly _sprite: Sprite;
    private _soundManager: SoundManager;

    private _animation: {
        elapsedMs: number;
        durationMs: number;
        easing: (t: number) => number;
        from: number;
        to: number
    } | null = null;

    constructor(
        assets: InfartAssets,
        app: Application<Renderer>,
        soundManager: SoundManager) {

        this._soundManager = soundManager;
        this._sprite = new Sprite(assets.textures.deathScreen);
        this._sprite.scale.set(1, app.screen.height / this._sprite.height);
        this._sprite.position.set(0, 0);
        this._sprite.zIndex = 10000;
        this._sprite.alpha = 0;
        app.stage.addChild(this._sprite);
    }

    update(ticker: Ticker) {
        if (!this._animation)
            return;

        const a = this._animation;
        a.elapsedMs = Math.min(a.elapsedMs + ticker.deltaMS, a.durationMs);

        const animationProgress = a.elapsedMs / a.durationMs;
        const animationProgressEased = a.easing(animationProgress);
        this._sprite.alpha = Numbers.lerp(a.from, a.to, animationProgressEased);

        if (animationProgress >= 1)
            this._animation = null;
    }

    show() {
        this.startTween(0, 1, 500, t => t);
        this._soundManager.playHeartBeat();
    }

    hide() {
        this.startTween(1, 0, 500, t => t);
        this._soundManager.stopHeartBeat();
    }

    private startTween(
        from: number,
        to: number,
        durationMs: number,
        easing: (t: number) => number) {

        this._animation = {
            elapsedMs: 0,
            durationMs,
            easing,
            from,
            to
        };

    }
}

export default GoingToExplodeOverlay;