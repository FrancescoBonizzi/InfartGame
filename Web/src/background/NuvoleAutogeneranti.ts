import Nuvola from "./Nuvola.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import World from "../world/World.ts";
import {ColorSource, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import Interval from "../primitives/Interval.ts";

class NuvoleAutogeneranti {

    private _nuvole: Nuvola[];
    private readonly _world: World;
    private readonly _ySpawnRange: Interval;
    private readonly _speedRange: Interval;

    constructor(
        world: World,
        assets: InfartAssets,
        startingScale: number,
        tint: ColorSource,
        speedRange: Interval,
        shouldAnimateScale: boolean,
        count: number) {

        this._world = world;
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
                world,
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
        const randomDistance = Numbers.randomBetween(0, 200);

        // Lo spawn a sinistra per questo gioco non ha senso, perché la telecamera si muove sempre a destra
        const direction = -1;
        const x = this._world.cameraX
            + this._world.viewPortWidth
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
            if (this._world.isOutOfScreenLeft(nuvola)) {
                console.info("Is out of screen");
                this.repositionNuvola(nuvola);
            }
            else {
                nuvola.update(time);
            }
        });
    }

}

export default NuvoleAutogeneranti;