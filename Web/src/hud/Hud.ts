import ScoreText from "./ScoreText.ts";
import {Application, Renderer, Ticker} from "pixi.js";
import HamburgerStatusBar from "./HamburgerStatusBar.ts";

class Hud {

    private readonly _scoreText: ScoreText;
    private readonly _hamburgerStatusBar: HamburgerStatusBar;

    constructor(
        app: Application<Renderer>,
        hamburgerStatusBar: HamburgerStatusBar
    ) {
        this._scoreText = new ScoreText(app);
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