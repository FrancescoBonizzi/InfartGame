import ScoreText from "./ScoreText.ts";
import {Application, Container, Graphics, Point, Renderer, Ticker} from "pixi.js";
import HamburgerStatusBar from "./HamburgerStatusBar.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import GoingToExplodeOverlay from "./GoingToExplodeOverlay.ts";
import SoundManager from "../services/SoundManager.ts";

class Hud {

    private readonly _container: Container;
    private readonly _background: Graphics;
    private readonly _scoreText: ScoreText;
    private readonly _hamburgerStatusBar: HamburgerStatusBar;
    private readonly _goingToExplodeOverlay: GoingToExplodeOverlay;

    private readonly _statusBarHeight = 50;

    constructor(
        app: Application<Renderer>,
        assets: InfartAssets,
        soundManager: SoundManager
    ) {
        // Ancoro il container al bordo basso e lo posizioni UNA volta
        this._container = new Container();
        this._container.pivot.set(0, this._statusBarHeight);
        this._container.position.set(0, app.screen.height);

        app.stage.addChild(this._container);

        // Tutto il resto in coordinate LOCALI al container
        this._background = new Graphics();
        this._background.rect(
            0,
            0,
            app.screen.width,
            this._statusBarHeight
        );
        this._background.fill({color: 0x000000, alpha: 0.6});
        this._container.addChild(this._background);

        this._scoreText = new ScoreText(
            this._container,
            assets,
            new Point(
                this._background.width - 20,
                this._statusBarHeight / 2
            ));
        this._goingToExplodeOverlay = new GoingToExplodeOverlay(
            assets,
            app,
            soundManager,
        );
        this._hamburgerStatusBar = new HamburgerStatusBar(
            this._container,
            new Point(0, this._statusBarHeight / 2),
            assets,
            this._goingToExplodeOverlay
        );
    }

    updateScore(score: number) {
        this._scoreText.updateScore(score);
    }

    update(time: Ticker) {
        this._hamburgerStatusBar.update(time);
        this._goingToExplodeOverlay.update(time);
    }

    playerDead() {
        this._hamburgerStatusBar.playerDead();
        this._goingToExplodeOverlay.hide();
    }

    getHamburgerStatusBar = () => this._hamburgerStatusBar;
}

export default Hud;