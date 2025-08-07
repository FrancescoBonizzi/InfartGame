import {Texture} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import Camera from "../world/Camera.ts";
import Grattacielo from "./Grattacielo.ts";

class GrattacieliAutogeneranti {

    private _maxGrattacieloPositionOffset = 20;
    private _lastGrattacieloX = 0;
    private _lastGrattacieloWidth = 0;
    private _previousCamX = 0;

    private readonly _camera: Camera;
    private readonly _grattacieli: Grattacielo[];
    private readonly _parallaxFactor: number;

    constructor(
        camera: Camera,
        grattacieli: Texture[],
        parallaxFactor: number) {

        this._camera = camera;
        this._previousCamX = this._camera.x;
        this._grattacieli = grattacieli.map(texture => new Grattacielo(
            texture,
            camera));
        this._lastGrattacieloX = 0;

        this._lastGrattacieloWidth = this._grattacieli[0].width;
        this._grattacieli.forEach(grattacielo => {
            this.repositionGrattacielo(grattacielo);
        });

        this._parallaxFactor = parallaxFactor;
    }

    private _onGrattacieloGeneratoHandler?: (grattacielo: Grattacielo) => void;
    public set onGrattacieloGeneratoHandler(handler: (grattacielo: Grattacielo) => void) {
        this._onGrattacieloGeneratoHandler = handler;
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
        } else {
            grattacielo.x = newX;
        }

        this._lastGrattacieloX = grattacielo.x;
        this._lastGrattacieloWidth = grattacielo.width;
        this._onGrattacieloGeneratoHandler?.(grattacielo);
    }

    grattacieli() {
        return this._grattacieli;
    }


    update() {

        const dx = this._camera.x - this._previousCamX;        // >0 se la camera va a destra
        if (dx === 0) {                                     // camera ferma => non muovere
            return;
        }
        const moveX = dx * this._parallaxFactor;

        this._grattacieli.forEach(g => {
            g.x -= moveX;                                     // parallax contro-movimento
            if (this._camera.isOutOfCameraLeft(g)) {
                this.repositionGrattacielo(g);
            }
        });

        this._previousCamX = this._camera.x;
    }

    set lastGrattacieloX(value: number) {
        this._lastGrattacieloX = value;
    }

    get lastGrattacieloX() {
        return this._lastGrattacieloX;
    }
}

export default GrattacieliAutogeneranti;