import {Sprite, Texture, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import Camera from "../world/Camera.ts";

class GrattacieliAutogeneranti {
    private _maxGrattacieloPositionOffset = 20;
    private _grattacieli: Sprite[];
    private _lastGrattacieloX = 0;
    private _lastGrattacieloWidth = 0;
    private _camera: Camera;
    private readonly _parallaxSpeed: number | null;

    constructor(
        camera: Camera,
        grattacieli: Texture[],
        parallaxSpeed: number | null) {

        this._camera = camera;
        this._grattacieli = grattacieli.map(texture => new Sprite(texture));
        this._lastGrattacieloX = 0;
        this._parallaxSpeed = parallaxSpeed;

        this._lastGrattacieloWidth = this._grattacieli[0].width;
        this._grattacieli.forEach(grattacielo => {
            camera.addToWorld(grattacielo);
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

    update(time: Ticker) {
        this._grattacieli.forEach(grattacielo => {
            if (this._camera.isOutOfCameraLeft(grattacielo)) {
                this.repositionGrattacielo(grattacielo);
            }
            else if (this._parallaxSpeed !== null) {
                grattacielo.x -= time.deltaTime * this._parallaxSpeed;
            }
        });
    }
}

export default GrattacieliAutogeneranti;