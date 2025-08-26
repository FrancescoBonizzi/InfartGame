import {Application, Renderer, Text} from "pixi.js";
import StringHelper from "../services/StringHelper.ts";

class LoadingThing {
    private readonly _loadingText: Text;
    private _app: Application<Renderer>;

    constructor(
        app: Application<Renderer>,
        text: string = 'Loading...') {

        if (StringHelper.isNullOrWhitespace(text)) {
            text = 'Loading...';
        }

        this._app = app;
        this._loadingText = new Text({
            text: text,
            style: {
                fontSize: 44,
                fontWeight: 'bold',
                fill: {color: '#ffffff'},
            }
        });
        this._loadingText.anchor.set(0.5);
        this._loadingText.x = app.screen.width / 2;
        this._loadingText.y = app.screen.height / 2;
    }

    show(): void {
        // I don't use something like _isShown because addChild is idempotent
        this._app.stage.addChild(this._loadingText);
    }

    hide(): void {
        // I don't use something like _isShown because removeChild is idempotent
        this._app.stage.removeChild(this._loadingText);
    }
}

export default LoadingThing;