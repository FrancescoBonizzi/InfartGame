import {Sprite, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import World from "../world/World.ts";

class GrattacieliAutogeneranti {
    private _maxGrattacieloPositionOffset = 20;
    private _grattacieli: Sprite[];
    private _lastGrattacieloX = 0;
    private _world: World;
    private _parallaxSpeed: number;

    constructor(
        world: World,
        grattacieli: Sprite[],
        parallaxSpeed: number) {

        this._world = world;
        this._grattacieli = grattacieli;
        this._lastGrattacieloX = 0;
        this._parallaxSpeed = parallaxSpeed;

        let previousGrattacieloWidth = 0;
        this._grattacieli.forEach(grattacielo => {

            world.addChild(grattacielo);

            grattacielo.anchor.set(0, 1);
            grattacielo.y = 0;

            grattacielo.x =
                this._lastGrattacieloX
                + previousGrattacieloWidth
                + Numbers.randomBetween(1, this._maxGrattacieloPositionOffset);
            this._lastGrattacieloX = grattacielo.x
            previousGrattacieloWidth = grattacielo.width;
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
        return grattacielo.x + grattacielo.width <= this._world.x;
    }

    update(time: Ticker) {
        this._grattacieli.forEach(grattacielo => {
            // When the grattacielo is out of the screen,
            // reposition it at the end of the last grattacielo on the right
            if (this.hasToBeRepositioned(grattacielo)) {
                this.repositionGrattacielo(grattacielo);
            } else {
                grattacielo.x -= time.deltaTime * this._parallaxSpeed;
            }
        });
    }
}

export default GrattacieliAutogeneranti;