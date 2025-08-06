import Nuvola from "./Nuvola.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";
import {ColorSource, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import Interval from "../primitives/Interval.ts";

class NuvoleAutogeneranti {

    private _nuvole: Nuvola[];
    private readonly _camera: Camera;
    private readonly _ySpawnRange: Interval = {min: -800, max: -300};
    private readonly _spawnHorizontalSpacing: Interval = {min: 200, max: 300};
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
            this._nuvole.push(nuvola);
        }

        this.setNuvoleFirstPositions();
    }

    private setNuvoleFirstPositions() {

        let spawnCursorX: number | null = null;

        this._nuvole.forEach(nuvola => {
            nuvola.y = this.generateNuvolaY();
            nuvola.x = this.generateFirstSpawnX(
                nuvola.width,
                spawnCursorX);
            spawnCursorX = nuvola.x;
        })
    }


    private generateFirstSpawnX(nuvolaWidth: number, spawnCursorX: number | null): number {
        const rightEdge = this._camera.x + this._camera.width;
        if (spawnCursorX == null || spawnCursorX < rightEdge) {
            spawnCursorX = rightEdge;
        }
        const spacing = Numbers.randomBetween(
            this._spawnHorizontalSpacing.min,
            this._spawnHorizontalSpacing.max);
        spawnCursorX += nuvolaWidth + spacing;
        return spawnCursorX;
    }

    private repositionNuvola(nuvola: Nuvola) {
        const y = this.generateNuvolaY();

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

    private generateNuvolaY(minGap = 40): number {
        // Faccio n tentativi per distribuire meglio verticalmente le nuvole
        let tries = 6;
        while (tries-- > 0) {
            const y = Numbers.randomBetween(this._ySpawnRange.min, this._ySpawnRange.max);
            const tooClose = this._nuvole.some(n => Math.abs(n.y - y) < minGap);
            if (!tooClose)
                return y;
        }

        // Fallback
        return Numbers.randomBetween(this._ySpawnRange.min, this._ySpawnRange.max);
    }

    update(time: Ticker) {
        this._nuvole.forEach(nuvola => {
            if (this._camera.isOutOfCameraLeft(nuvola)) {
                this.repositionNuvola(nuvola);
            } else {
                nuvola.update(time);
            }
        });
    }

}

export default NuvoleAutogeneranti;