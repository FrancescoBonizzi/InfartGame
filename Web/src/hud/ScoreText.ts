import {Container, Point} from "pixi.js";
import HudText from "./HudText.ts";
import InfartAssets from "../assets/InfartAssets.ts";

class ScoreText extends HudText {

    constructor(
        container: Container,
        assets: InfartAssets,
        position: Point) {

        super(
            container,
            assets,
            position,
            new Point(1, 0.5));
    }

    updateScore(score: number) {
        this.updateText(`Score: ${score}`);
    }
}

export default ScoreText;