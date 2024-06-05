import {Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import World from "../world/World.ts";

class GrattacieliGroup {

    private _grattacieliBack: GrattacieliAutogeneranti;
    private _grattacieliMid: GrattacieliAutogeneranti;
    private _grattacieliGround: GrattacieliAutogeneranti;

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

        this._grattacieliGround = new GrattacieliAutogeneranti(
            world,
            infartAssets.sprites.buildings.ground,
            0.4);
    }

    update(time: Ticker) {
        this._grattacieliBack.update(time);
        this._grattacieliMid.update(time);
        this._grattacieliGround.update(time);
    }
}

export default GrattacieliGroup;