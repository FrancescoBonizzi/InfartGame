import {Application, Renderer, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";

class GrattacieliGroup {

    private _grattacieliBack: GrattacieliAutogeneranti;
    private _grattacieliMid: GrattacieliAutogeneranti;
    private _grattacieliGround: GrattacieliAutogeneranti;


    constructor(
        app: Application<Renderer>,
        infartAssets: InfartAssets) {

        this._grattacieliBack = new GrattacieliAutogeneranti(
            app,
            infartAssets.sprites.buildings.back);

        this._grattacieliMid = new GrattacieliAutogeneranti(
            app,
            infartAssets.sprites.buildings.mid);

        this._grattacieliGround = new GrattacieliAutogeneranti(
            app,
            infartAssets.sprites.buildings.ground);

    }

    update(time: Ticker) {
        const dx = -time.deltaTime;

        this._grattacieliBack.moveX(dx * 0.1);
        this._grattacieliBack.update();

        this._grattacieliMid.moveX(dx * 0.3);
        this._grattacieliMid.update();

        this._grattacieliGround.moveX(dx * 0.5);
        this._grattacieliGround.update();
    }
}

export default GrattacieliGroup;