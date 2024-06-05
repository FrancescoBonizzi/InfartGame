import {Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import World from "../world/World.ts";

class BackgroundLandscape {

    private _grattacieliBack: GrattacieliAutogeneranti;
    private _grattacieliMid: GrattacieliAutogeneranti;

    constructor(
        world: World,
        infartAssets: InfartAssets) {

        this._grattacieliBack = new GrattacieliAutogeneranti(
            world,
            infartAssets.sprites.buildings.back,
            0.05);

        this._grattacieliMid = new GrattacieliAutogeneranti(
            world,
            infartAssets.sprites.buildings.mid,
            0.2);

    }

    update(time: Ticker) {
        this._grattacieliBack.update(time);
        this._grattacieliMid.update(time);
    }
}

export default BackgroundLandscape;