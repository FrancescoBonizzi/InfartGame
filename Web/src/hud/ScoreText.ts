import {Container, Point} from "pixi.js";
import HudText from "./HudText.ts";

class ScoreText extends HudText {

    constructor(
        container: Container,
        position: Point) {

        super(
            container,
            position,
            new Point(1, 0.5));
    }

    updateScore(score: number) {
        this.updateText(`Score: ${score}`);
    }
}

export default ScoreText;