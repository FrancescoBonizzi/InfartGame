import Nuvola from "./Nuvola.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import World from "../world/World.ts";
import {ColorSource, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";
import Interval from "../primitives/Interval.ts";

class NuvoleAutogeneranti {

    private _nuvole: Nuvola[];
    private readonly _nuvoleCount = 5;
    private readonly _world: World;
    private readonly _ySpawnRange: Interval;
    private readonly _speedRange: Interval;

    constructor(
        world: World,
        assets: InfartAssets,
        startingScale: number,
        tint: ColorSource,
        speedRange: Interval) {

        this._world = world;
        this._ySpawnRange = {
            min: -300,
            max: -600
        };
        this._speedRange = speedRange;
        
        const nuvoleSprites = [
            assets.textures.nuvola1,
            assets.textures.nuvola2,
            assets.textures.nuvola3,
        ];

        this._nuvole = [];
        for (let i = 0; i < this._nuvoleCount; i++) {
            const nuvola = new Nuvola(
                world,
                nuvoleSprites[i % nuvoleSprites.length],
                startingScale,
                tint);
            this.repositionNuvola(nuvola);
            this._nuvole.push(nuvola);
        }
    }
    
    isOutOfScreen(nuvola: Nuvola) {
        const outOfScreenMargin = nuvola.width * 2;
        const globalX = this._world.worldToScreenX(nuvola.x);
        return globalX + nuvola.width <= -outOfScreenMargin
            || globalX - nuvola.width >= this._world.viewPortWidth + outOfScreenMargin;
    }

    repositionNuvola(nuvola: Nuvola) {
        const y = Numbers.randomBetween(
            this._ySpawnRange.min,
            this._ySpawnRange.max);

        const outOfScreenDistance = 0;//200;
        const randomDistance = 0;//Numbers.randomBetween(1, 20);
        let direction: number;
        let x: number;

        if (Numbers.headOrTail()) {
            x = this._world.x - outOfScreenDistance - randomDistance;
            direction = 1;
        } else {
            x = this._world.x + this._world.viewPortWidth + outOfScreenDistance + randomDistance;
            direction = -1;
        }

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
            if (this.isOutOfScreen(nuvola)) {
                this.repositionNuvola(nuvola);
            } else {
                nuvola.update(time);
            }
        });
    }

}

export default NuvoleAutogeneranti;