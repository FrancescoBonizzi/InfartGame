import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import Camera from "../world/Camera.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import {Ticker} from "pixi.js";
import NuvoleAutogeneranti from "./NuvoleAutogeneranti.ts";
import DynamicGameParameters from "../services/DynamicGameParameters.ts";
import Numbers from "../services/Numbers.ts";

class Foreground {
    private readonly _grattacieliGround: GrattacieliAutogeneranti;
    private readonly _nuvolificioGround: NuvoleAutogeneranti;
    private readonly _dynamicGameParameters: DynamicGameParameters;

    constructor(
        world: Camera,
        infartAssets: InfartAssets,
        dynamicGameParameters: DynamicGameParameters) {

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
                min: 22 / 100,
                max: 25 / 100
            },
            true,
            6);
        this._dynamicGameParameters = dynamicGameParameters;
    }

    generateBuco() {
        const startingX = this._grattacieliGround.lastGrattacieloX;
        const space = Numbers.randomBetweenInterval(this._dynamicGameParameters.larghezzaBuchi);
        this._grattacieliGround.lastGrattacieloX = startingX + space;
    }

    update(time: Ticker) {
        this._grattacieliGround.update(time);
        this._nuvolificioGround.update(time);
    }

    get drawnGrattacieli() {
        return this._grattacieliGround.grattacieli();
    }

}

export default Foreground;