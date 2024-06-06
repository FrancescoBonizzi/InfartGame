import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import World from "../world/World.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import {Ticker} from "pixi.js";
import NuvoleAutogeneranti from "./NuvoleAutogeneranti.ts";

class Foreground {
    private _grattacieliGround: GrattacieliAutogeneranti;
    private _nuvolificioGround: NuvoleAutogeneranti;

    constructor(
        world: World,
        infartAssets: InfartAssets) {

        this._grattacieliGround = new GrattacieliAutogeneranti(
            world,
            infartAssets.textures.buildings.ground,
            null);
        this._nuvolificioGround = new NuvoleAutogeneranti(
            world,
            infartAssets,
            0.6,
            "#ffffff",
            {
                min: 20 / 100,
                max: 25 / 100
            });
    }

    update(time: Ticker) {
        this._grattacieliGround.update(time);
        this._nuvolificioGround.update(time);
    }

}

export default Foreground;