import {Container, Text, Point} from "pixi.js";

class ScoreText {
    private readonly _text: Text;

    constructor(
        container: Container,
        position: Point) {

        this._text = new Text({
            style: {
                fontSize: 20,
                fontWeight: 'bold',
                fill: {color: '#ffffff'},
            }
        });
        this._text.anchor.set(1, 0.5);
        this._text.x = position.x;
        this._text.y = position.y;

        container.addChild(this._text);
        this.updateScore(0);
    }

    updateScore(score: number) {
        this._text.text = `${score} metri`;
    }
}

export default ScoreText;