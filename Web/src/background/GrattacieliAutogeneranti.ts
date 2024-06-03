import {Application, Renderer, Sprite} from "pixi.js";
import Numbers from "../services/Numbers.ts";

class GrattacieliAutogeneranti {
    private _maxGrattacieloPositionOffset = 20;
    private _grattacieli: Sprite[];
    private _lastGrattacieloX = 0;

    constructor(
        app: Application<Renderer>,
        grattacieli: Sprite[]) {

        this._grattacieli = grattacieli;
        this._lastGrattacieloX = 0;

        let previousGrattacieloWidth = 0;
        this._grattacieli.forEach(grattacielo => {

            // TODO: tutto questo in una classe Grattacielo
            app.stage.addChild(grattacielo);

            grattacielo.anchor.set(0, 1);
            grattacielo.y = app.screen.height;

            grattacielo.x =
                this._lastGrattacieloX
                + previousGrattacieloWidth
                + Numbers.randomBetween(1, this._maxGrattacieloPositionOffset);
            this._lastGrattacieloX = grattacielo.x
            previousGrattacieloWidth = grattacielo.width;
        });

    }

    moveX(x: number) {
        this._grattacieli.forEach(grattacielo => {
            grattacielo.x += x;
        });
    }

    repositionGrattacielo(grattacielo: Sprite) {
        grattacielo.x =
            this._lastGrattacieloX
            + grattacielo.width
            + Numbers.randomBetween(1, this._maxGrattacieloPositionOffset);
        this._lastGrattacieloX = grattacielo.x;
    }

    hasToBeRepositioned(grattacielo: Sprite) {
        return grattacielo.x + grattacielo.width <= 0;
    }

    update() {
        this._grattacieli.forEach(grattacielo => {
            if (this.hasToBeRepositioned(grattacielo)) {
                this.repositionGrattacielo(grattacielo);
            }
        });
    }
}

export default GrattacieliAutogeneranti;