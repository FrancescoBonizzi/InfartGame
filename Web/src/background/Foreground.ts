import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import Camera from "../world/Camera.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import {Ticker} from "pixi.js";
import NuvoleAutogeneranti from "./NuvoleAutogeneranti.ts";
import DynamicGameParameters from "../services/DynamicGameParameters.ts";
import Grattacielo from "./Grattacielo.ts";

class Foreground {
    private readonly _grattacieliGround: GrattacieliAutogeneranti;
    private readonly _nuvolificioGround: NuvoleAutogeneranti;

    constructor(
        world: Camera,
        infartAssets: InfartAssets,
        dynamicGameParameters: DynamicGameParameters) {

        this._grattacieliGround = new GrattacieliAutogeneranti(
            world,
            infartAssets.textures.buildings.ground,
            0,  // Attenzione! Se cambiamo questo, si rompono le posizioni delle gemme
            true,
            dynamicGameParameters);
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
        this._grattacieliGround.onGrattacieloGeneratoHandler = (grattacielo) =>
            this._onGrattacieloGeneratoHandler?.(grattacielo);
    }

    private _onGrattacieloGeneratoHandler?: (grattacielo: Grattacielo) => void;
    public set onGrattacieloGeneratoHandler(handler: (grattacielo: Grattacielo) => void) {
        this._onGrattacieloGeneratoHandler = handler;
    }

    update(time: Ticker) {
        this._grattacieliGround.update();
        this._nuvolificioGround.update(time);
    }

    get drawnGrattacieli() {
        return this._grattacieliGround.grattacieli();
    }

}

export default Foreground;