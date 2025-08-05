import {Container, Text} from "pixi.js";

class ScoreText {
    private readonly _text: Text;

    constructor(
        container: Container) {

        this._text = new Text({
            style: {
                fontSize: 20,
                fontWeight: 'bold',
                fill: {color: '#ffffff'},
            }
        });
        this._text.anchor.set(1, 1);
        this._text.x = container.width - 20;
        this._text.y = container.height / 2;

        container.addChild(this._text);

        this.updateScore(0);
    }

    updateScore(score: number) {
        this._text.text = `${score} metri`;
    }
}

export default ScoreText;