import ScoreText from "./ScoreText.ts";
import {Application, Container, Graphics, Point, Renderer, Ticker} from "pixi.js";
import HamburgerStatusBar from "./HamburgerStatusBar.ts";

class Hud {

    private readonly _container: Container;
    private readonly _background: Graphics;
    private readonly _scoreText: ScoreText;
    private readonly _hamburgerStatusBar: HamburgerStatusBar;

    private readonly _statusBarHeight = 50;

    constructor(
        app: Application<Renderer>,
        hamburgerStatusBar: HamburgerStatusBar
    ) {
        this._container = new Container();
        app.stage.addChild(this._container);

        this._background = new Graphics();
        this._background.rect(
            0,
            app.screen.height - this._statusBarHeight,
            app.screen.width,
            this._statusBarHeight
        );
        this._background.fill({color: 0x000000, alpha: 0.6});
        this._container.addChild(this._background);

        this._scoreText = new ScoreText(
            this._container,
            new Point(
                this._background.width - 20,
                app.screen.height - (this._statusBarHeight / 2)
            ));
        this._hamburgerStatusBar = hamburgerStatusBar;
    }

    updateScore(score: number) {
        this._scoreText.updateScore(score);
    }

    update(time: Ticker) {
        this._hamburgerStatusBar.update(time);
    }
}

export default Hud;