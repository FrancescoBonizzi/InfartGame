import {Texture, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import Camera from "../world/Camera.ts";
import Grattacielo from "./Grattacielo.ts";
import DynamicGameParameters from "../services/DynamicGameParameters.ts";

class GrattacieliAutogeneranti {

    private _maxGrattacieloPositionOffset = 20;
    private _lastGrattacieloX = 0;
    private _lastGrattacieloWidth = 0;

    private readonly _camera: Camera;
    private readonly _grattacieli: Grattacielo[];
    private readonly _parallaxFactor: number;
    private readonly _dynamicGameParameters: DynamicGameParameters;

    constructor(
        camera: Camera,
        grattacieli: Texture[],
        dynamicGameParameters: DynamicGameParameters,
        parallaxFactor: number) {

        this._camera = camera;
        this._grattacieli = grattacieli.map(texture => new Grattacielo(
            texture,
            camera));
        this._lastGrattacieloX = 0;
        this._dynamicGameParameters = dynamicGameParameters;

        this._lastGrattacieloWidth = this._grattacieli[0].width;
        this._grattacieli.forEach(grattacielo => {
            this.repositionGrattacielo(grattacielo);
        });

        this._parallaxFactor = parallaxFactor;
    }

    repositionGrattacielo(grattacielo: Grattacielo) {

        const newX = this._lastGrattacieloX
            + this._lastGrattacieloWidth
            + Numbers.randomBetween(1, this._maxGrattacieloPositionOffset);
        const distance = newX - (this._lastGrattacieloX + this._lastGrattacieloWidth);

        if (distance > this._maxGrattacieloPositionOffset) {
            grattacielo.x = this._lastGrattacieloX
                + this._lastGrattacieloWidth
                + this._maxGrattacieloPositionOffset;
        }
        else {
            grattacielo.x = newX;
        }

        this._lastGrattacieloX = grattacielo.x;
        this._lastGrattacieloWidth = grattacielo.width;
    }

    grattacieli() {
        return this._grattacieli;
    }

    update(time: Ticker) {

        const moveX = this.getMoveX(time);
        this._lastGrattacieloX -= moveX;

        this._grattacieli.forEach(grattacielo => {
            grattacielo.x -= moveX;

            if (this._camera.isOutOfCameraLeft(grattacielo)) {
                this.repositionGrattacielo(grattacielo);
            }
        });
    }

    private getMoveX(time: Ticker) {
        return time.deltaTime
            * (this._dynamicGameParameters.playerHorizontalSpeed / 1000)
            * this._parallaxFactor;
    }

    set lastGrattacieloX(value: number) {
        this._lastGrattacieloX = value;
    }

    get lastGrattacieloX() {
        return this._lastGrattacieloX;
    }
}

export default GrattacieliAutogeneranti;