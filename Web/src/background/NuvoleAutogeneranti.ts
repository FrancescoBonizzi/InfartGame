import Nuvola from "./Nuvola.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";
import {ColorSource, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import Interval from "../primitives/Interval.ts";

class NuvoleAutogeneranti {

    private _nuvole: Nuvola[];
    private readonly _camera: Camera;
    private readonly _ySpawnRange: Interval;
    private readonly _speedRange: Interval;

    constructor(
        camera: Camera,
        assets: InfartAssets,
        startingScale: number,
        tint: ColorSource,
        speedRange: Interval,
        shouldAnimateScale: boolean,
        count: number) {

        this._camera = camera;
        this._ySpawnRange = {
            min: -300,
            max: -600
        };
        this._speedRange = speedRange;

        const nuvoleTextures = [
            assets.textures.nuvola1,
            assets.textures.nuvola2,
            assets.textures.nuvola3,
        ];

        this._nuvole = [];
        for (let i = 0; i < count; i++) {
            const nuvola = new Nuvola(
                camera,
                nuvoleTextures[i % nuvoleTextures.length],
                startingScale,
                tint,
                shouldAnimateScale);
            this.repositionNuvola(nuvola);
            this._nuvole.push(nuvola);
        }
    }

    repositionNuvola(nuvola: Nuvola) {
        const y = Numbers.randomBetween(
            this._ySpawnRange.min,
            this._ySpawnRange.max);

        // Per non farle spawnare tutte sullo stesso asse
        const randomDistance = Numbers.randomBetween(0, 250);

        const direction = Numbers.headOrTail() ? 1 : -1;
        const x = this._camera.x
            + this._camera.width
            + nuvola.width
            + randomDistance;

        const speed = Numbers.randomBetween(
                this._speedRange.min,
                this._speedRange.max)
            * direction;

        nuvola.x = x;
        nuvola.y = y;
        nuvola.speed = speed;
    }

    update(time: Ticker) {
        this._nuvole.forEach(nuvola => {
            if (this._camera.isOutOfCameraLeft(nuvola)) {
                this.repositionNuvola(nuvola);
            }
            else {
                nuvola.update(time);
            }
        });
    }

}

export default NuvoleAutogeneranti;