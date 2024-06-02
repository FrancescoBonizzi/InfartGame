import {Application, Renderer, Text} from "pixi.js";

class LoadingText {
    private readonly _loadingText: Text;

    private _isShown: boolean;
    private _app: Application<Renderer>;

    constructor(
        app: Application<Renderer>,
        text: string = 'Loading...') {
        this._app = app;
        this._isShown = false;
        this._loadingText = new Text({
            text: text,
            style: {
                fontSize: 40,
                fontWeight: 'bold',
                fill: { color: '#ffffff' },
            }
        });
        this._loadingText.anchor.set(0.5);
        this._loadingText.x = app.screen.width / 2;
        this._loadingText.y = app.screen.height / 2;
    }

    show(): void {
        if (!this._isShown) {
            this._app.stage.addChild(this._loadingText);
            this._isShown = true;
        }
    }

    hide(): void {
        if (this._isShown) {
            this._app.stage.removeChild(this._loadingText);
            this._isShown = false;
        }
    }
}

export default LoadingText;