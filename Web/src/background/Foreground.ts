import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import World from "../world/World.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import {Ticker} from "pixi.js";

class Foreground {
    private _grattacieliGround: GrattacieliAutogeneranti;

    constructor(
        world: World,
        infartAssets: InfartAssets) {

        this._grattacieliGround = new GrattacieliAutogeneranti(
            world,
            infartAssets.sprites.buildings.ground,
            0.4);
    }

    update(time: Ticker) {
        this._grattacieliGround.update(time);
    }

}

export default Foreground;