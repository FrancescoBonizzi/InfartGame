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
            "green",//"#051728",
            {
                min: 10 / 100,
                max: 20 / 100
            });
        this._grattacieliBack = new GrattacieliAutogeneranti(
            world,
            infartAssets.sprites.buildings.back,
            0.01);

        this._nuvolificioMid = new NuvoleAutogeneranti(
            world,
            infartAssets,
            0.4,
            "yellow", //"#093243",
            {
                min: 30 / 100,
                max: 40 / 100
            });
        this._grattacieliMid = new GrattacieliAutogeneranti(
            world,
            infartAssets.sprites.buildings.mid,
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