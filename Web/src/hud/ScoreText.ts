import {Application, Container, Graphics, Renderer, Text} from "pixi.js";

class ScoreText {
    private readonly _text: Text;
    private readonly _container: Container;
    private readonly _rectangle: Graphics;

    constructor(
        app: Application<Renderer>) {

        this._text = new Text({
            style: {
                fontSize: 40,
                fontWeight: 'bold',
                fill: {color: '#ffffff'},
            }
        });
        this._text.anchor.set(1, 1);

        this._rectangle = new Graphics();
        this._container = new Container();
        this._container.addChild(this._rectangle);
        this._container.addChild(this._text);

        this._container.x = app.screen.width - 16;
        this._container.y = app.screen.height - 10;
        this._container.pivot.set(
            this._text.width,
            this._text.height);

        app.stage.addChild(this._container);

        this.updateScore(0);
    }

    updateScore(score: number) {
        this._text.text = `${score} metri`;
        this.updateBackgroundRectangle();
    }

    updateBackgroundRectangle() {
        const bounds = this._text.getLocalBounds();
        const horizontalPadding = 16;
        const verticalPadding = 4;
        this._rectangle.clear();
        this._rectangle.roundRect(
            -bounds.width - horizontalPadding,
            -bounds.height - verticalPadding,
            bounds.width + horizontalPadding * 2,
            bounds.height + verticalPadding * 2,
            16
        );
        this._rectangle.fill({ color: 0x000000, alpha: 0.6 });
    }

}

export default ScoreText;