import {ColorSource, Point, Text, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";

class PopupText {
    private readonly _text: Text;
    private readonly _camera: Camera;

    private _elapsedMs: number = 0;
    private readonly _durationMs: number = 1000;
    private readonly _riseSpeed: number = -0.05;

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
                fontSize: 32,
                fill: { color: color },
                stroke: { color: "#000000", width: 3 },
                align: "center",
            },
        });

        this._text.anchor.set(0.5, 0.5);
        this._text.x = position.x;
        this._text.y = position.y;

        this._camera.addToWorld(this._text);
    }

    update(ticker: Ticker) {
        this._elapsedMs += ticker.deltaMS;

        this._text.y += this._riseSpeed * ticker.deltaMS;
        const progress = this._elapsedMs / this._durationMs;
        this._text.alpha = 1 - progress;
    }

    destroy() {
        this._camera.removeFromWorld(this._text);
        this._text.destroy();
    }
}

export default PopupText;