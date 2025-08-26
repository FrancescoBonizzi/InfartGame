import {ColorSource, Point, Text, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";

class PopupText {
    private readonly _text: Text;
    private readonly _camera: Camera;

    private _elapsedMilliseconds = 0;
    private readonly _durationMilliseconds = 3000;
    private readonly _riseSpeed = -0.2;
    private readonly _driftSpeed = 0.5;
    private readonly _scaleFrom = 0.9;
    private readonly _scaleTo = 1.4;

    constructor(
        camera: Camera,
        assets: InfartAssets,
        position: Point,
        message: string,
        color: ColorSource,
    ) {
        this._camera = camera;

        this._text = new Text({
            text: message,
            style: {
                fontFamily: assets.fontName,
                fontSize: 48,
                fill: { color: color },
                stroke: { color: "#000000", width: 3 },
                align: "center",
            },
        });

        this._text.x = position.x;
        this._text.y = position.y;

        this._camera.addToWorld(this._text);
    }

    update(ticker: Ticker) {

        const deltaMilliseconds = ticker.deltaMS;
        this._elapsedMilliseconds += ticker.deltaMS;

        // Calcola quanto tempo è passato in percentuale (0 = appena nato, 1 = finito)
        const lifetimeProgress = Math.min(1, this._elapsedMilliseconds / this._durationMilliseconds);

        if (lifetimeProgress >= 1) {
            return;
        }

        // Easing per rendere più fluido lo scaling
        const easedProgress = 1 - Math.pow(1 - lifetimeProgress, 3);

        // Movimento: verso l’alto e leggermente a destra
        this._text.y += this._riseSpeed * deltaMilliseconds;
        this._text.x += this._driftSpeed * deltaMilliseconds;

        // Scaling progressivo
        const currentScale = this._scaleFrom + (this._scaleTo - this._scaleFrom) * easedProgress;
        this._text.scale.set(currentScale);

        // Dissolvenza progressiva
        this._text.alpha = 1 - lifetimeProgress;
    }

    destroy() {
        this._camera.removeFromWorld(this._text);
        this._text.destroy();
    }
}

export default PopupText;