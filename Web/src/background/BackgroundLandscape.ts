import {Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import World from "../world/World.ts";
import BackgroundSky from "./BackgroundSky.ts";
import NuvoleAutogeneranti from "./NuvoleAutogeneranti.ts";

class BackgroundLandscape {

    private _backgroundSky: BackgroundSky;

    private _grattacieliBack: GrattacieliAutogeneranti;
    private _nuvolificioBack: NuvoleAutogeneranti;

    private _grattacieliMid: GrattacieliAutogeneranti;
    private _nuvolificioMid: NuvoleAutogeneranti;

    constructor(
        world: World,
        infartAssets: InfartAssets) {

        this._backgroundSky = new BackgroundSky(
            infartAssets,
            world);

        this._nuvolificioBack = new NuvoleAutogeneranti(
            world,
            infartAssets,
            0.2,
            "#051728",
            {
                min: 2 / 100,
                max: 5 / 100
            },
            false,
            10);
        this._grattacieliBack = new GrattacieliAutogeneranti(
            world,
            infartAssets.textures.buildings.back,
            0.01);

        this._nuvolificioMid = new NuvoleAutogeneranti(
            world,
            infartAssets,
            0.4,
            "#093243",
            {
                min: 12 / 100,
                max: 15 / 100
            },
            false,
            7);
        this._grattacieliMid = new GrattacieliAutogeneranti(
            world,
            infartAssets.textures.buildings.mid,
            0.18);
    }

    update(time: Ticker) {
        this._backgroundSky.update();
        this._grattacieliBack.update(time);
        this._grattacieliMid.update(time);
        this._nuvolificioBack.update(time);
        this._nuvolificioMid.update(time);
    }
}

export default BackgroundLandscape;