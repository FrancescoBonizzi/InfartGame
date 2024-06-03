import {Application, Renderer, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";

class GrattacieliGroup {

    private _grattacieliGround: GrattacieliAutogeneranti;

    constructor(
        app: Application<Renderer>,
        infartAssets: InfartAssets) {

        this._grattacieliGround = new GrattacieliAutogeneranti(
            app,
            infartAssets.sprites.buildings.ground);
    }

    update(time: Ticker) {
        const dx = time.deltaTime * 0.5;
        this._grattacieliGround.moveX(-dx);
        this._grattacieliGround.update();
    }
}

export default GrattacieliGroup;