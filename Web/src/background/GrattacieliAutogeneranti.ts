import {Texture} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import Camera from "../world/Camera.ts";
import Grattacielo from "./Grattacielo.ts";

class GrattacieliAutogeneranti {

    private _maxGrattacieloPositionOffset = 20;
    private _lastGrattacieloX = 0;
    private _lastGrattacieloWidth = 0;
    private _previousCameraX = 0;
    private _pendingExtraGap = 0;

    private readonly _camera: Camera;
    private readonly _grattacieli: Grattacielo[];
    private readonly _parallaxFactor: number;

    constructor(
        camera: Camera,
        grattacieli: Texture[],
        parallaxFactor: number) {

        this._camera = camera;
        this._previousCameraX = this._camera.x;
        this._grattacieli = grattacieli.map(texture => new Grattacielo(
            texture,
            camera));
        this._lastGrattacieloX = 0;
        this._lastGrattacieloWidth = 0;
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

        const baseDistance = this._lastGrattacieloX + this._lastGrattacieloWidth;
        const randomDistance = Numbers.randomBetween(0, this._maxGrattacieloPositionOffset);
        grattacielo.x = baseDistance + randomDistance + this._pendingExtraGap;

        this._lastGrattacieloX = grattacielo.x;
        this._lastGrattacieloWidth = grattacielo.width;
        this._pendingExtraGap = 0;
        this._onGrattacieloGeneratoHandler?.(grattacielo);
    }

    grattacieli() {
        return this._grattacieli;
    }

    update() {

        const cameraDeltaX = this._camera.x - this._previousCameraX;

        // Camera ferma => non muovere
        if (cameraDeltaX === 0) {
            return;
        }

        const moveX = cameraDeltaX * this._parallaxFactor;

        this._grattacieli.forEach(grattacielo => {
            grattacielo.x -= moveX;
            if (this._camera.isOutOfCameraLeft(grattacielo)) {
                this.repositionGrattacielo(grattacielo);
            }
        });

        this._previousCameraX = this._camera.x;
    }

    addExtraGap(space: number) {
        this._pendingExtraGap = space;
    }
}

export default GrattacieliAutogeneranti;