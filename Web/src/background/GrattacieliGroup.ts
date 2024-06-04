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
            infartAssets.sprites.buildings.back);

        this._grattacieliMid = new GrattacieliAutogeneranti(
            world,
            infartAssets.sprites.buildings.mid);

        this._grattacieliGround = new GrattacieliAutogeneranti(
            world,
            infartAssets.sprites.buildings.ground);
    }

    update(time: Ticker) {
        const dx = -time.deltaTime;

        this._grattacieliBack.moveX(dx * 0.05);
        this._grattacieliBack.update();

        this._grattacieliMid.moveX(dx * 0.2);
        this._grattacieliMid.update();

        this._grattacieliGround.moveX(dx * 0.4);
        this._grattacieliGround.update();
    }
}

export default GrattacieliGroup;