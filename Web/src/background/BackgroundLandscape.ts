import {Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import Camera from "../world/Camera.ts";
import BackgroundSky from "./BackgroundSky.ts";
import NuvoleAutogeneranti from "./NuvoleAutogeneranti.ts";
import StarField from "../particleEmitters/StarField.ts";

class BackgroundLandscape {

    private readonly _backgroundSky: BackgroundSky;
    private readonly _starfield: StarField;

    private readonly _grattacieliBack: GrattacieliAutogeneranti;
    private readonly _nuvolificioBack: NuvoleAutogeneranti;

    private readonly _grattacieliMid: GrattacieliAutogeneranti;
    private readonly _nuvolificioMid: NuvoleAutogeneranti;

    constructor(
        camera: Camera,
        infartAssets: InfartAssets) {

        this._backgroundSky = new BackgroundSky(
            infartAssets,
            camera);

        this._starfield = new StarField(
            infartAssets,
            camera);

        this._nuvolificioBack = new NuvoleAutogeneranti(
            camera,
            infartAssets,
            0.2,
            "#051728",
            {
                min: 2 / 100,
                max: 5 / 100
            },
            false,
            8);
        this._grattacieliBack = new GrattacieliAutogeneranti(
            camera,
            infartAssets.textures.buildings.back,
            0.05);

        this._nuvolificioMid = new NuvoleAutogeneranti(
            camera,
            infartAssets,
            0.4,
            "#093243",
            {
                min: 12 / 100,
                max: 15 / 100
            },
            false,
            8);
        this._grattacieliMid = new GrattacieliAutogeneranti(
            camera,
            infartAssets.textures.buildings.mid,
            0.1);
    }

    update(time: Ticker) {
        this._starfield.update(time);
        this._backgroundSky.update();
        this._grattacieliBack.update();
        this._grattacieliMid.update();
        this._nuvolificioBack.update(time);
        this._nuvolificioMid.update(time);
    }

}

export default BackgroundLandscape;