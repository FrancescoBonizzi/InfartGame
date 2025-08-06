import {Container, Point, Text} from "pixi.js";

class HudText {
    private readonly _text: Text;

    constructor(
        container: Container,
        position: Point,
        anchor: Point) {

        this._text = new Text({
            style: {
                fontSize: 20,
                fontWeight: 'bold',
                fill: {color: '#ffffff'},
            }
        });

        this._text.anchor.set(anchor.x, anchor.y);
        this._text.x = position.x;
        this._text.y = position.y;

        container.addChild(this._text);
        this.updateText('');
    }

    updateText(text: string) {
        this._text.text = text;
    }
}

export default HudText;