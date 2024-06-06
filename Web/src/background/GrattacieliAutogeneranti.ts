import {Sprite, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import World from "../world/World.ts";

class GrattacieliAutogeneranti {
    private _maxGrattacieloPositionOffset = 20;
    private _grattacieli: Sprite[];
    private _lastGrattacieloX = 0;
    private _lastGrattacieloWidth = 0;
    private _world: World;
    private readonly _parallaxSpeed: number;

    constructor(
        world: World,
        grattacieli: Sprite[],
        parallaxSpeed: number) {

        this._world = world;
        this._grattacieli = grattacieli;
        this._lastGrattacieloX = 0;
        this._parallaxSpeed = parallaxSpeed;

        this._lastGrattacieloWidth = this._grattacieli[0].width;
        this._grattacieli.forEach(grattacielo => {
            world.addChild(grattacielo);
            grattacielo.anchor.set(0, 1);
            grattacielo.y = 0;
            this.repositionGrattacielo(grattacielo);
        });
    }

    repositionGrattacielo(grattacielo: Sprite) {
        grattacielo.x =
            this._lastGrattacieloX
            + this._lastGrattacieloWidth
            + Numbers.randomBetween(1, this._maxGrattacieloPositionOffset);
        this._lastGrattacieloX = grattacielo.x;
        this._lastGrattacieloWidth = grattacielo.width;
    }

    isOutOfScreenLeft(grattacielo: Sprite) {
        const globalX = this._world.worldToScreenX(grattacielo.x);
        return globalX + grattacielo.width <= 0;
    }

    update(time: Ticker) {
        this._grattacieli.forEach(grattacielo => {
            if (this.isOutOfScreenLeft(grattacielo)) {
                this.repositionGrattacielo(grattacielo);
            } else {
                grattacielo.x -= time.deltaTime * this._parallaxSpeed;
            }
        });
    }
}

export default GrattacieliAutogeneranti;