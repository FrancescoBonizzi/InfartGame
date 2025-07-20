import ScoreText from "./ScoreText.ts";
import {Application, Renderer} from "pixi.js";

class Hud {

    private _scoreText: ScoreText;

    constructor(
        app: Application<Renderer>
    ) {
        this._scoreText = new ScoreText(app);
    }

    updateScore(score: number) {
        this._scoreText.updateScore(score);
    }

    // TODO: gestione hamburger
}

export default Hud;