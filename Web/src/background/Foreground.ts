import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import Camera from "../world/Camera.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import {Ticker} from "pixi.js";
import NuvoleAutogeneranti from "./NuvoleAutogeneranti.ts";
import DynamicGameParameters from "../services/DynamicGameParameters.ts";
import Numbers from "../services/Numbers.ts";
import PixiJsTimer from "../primitives/PixiJsTimer.ts";
import Grattacielo from "./Grattacielo.ts";

class Foreground {
    private readonly _grattacieliGround: GrattacieliAutogeneranti;
    private readonly _nuvolificioGround: NuvoleAutogeneranti;
    private readonly _dynamicGameParameters: DynamicGameParameters;
    private readonly _bucoTimer: PixiJsTimer;
    private readonly _bucoProbability = 0.5;

    constructor(
        world: Camera,
        infartAssets: InfartAssets,
        dynamicGameParameters: DynamicGameParameters) {

        this._grattacieliGround = new GrattacieliAutogeneranti(
            world,
            infartAssets.textures.buildings.ground,
            0); // Attenzione! Se cambiamo questo, si rompono le posizioni delle gemme
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
        this._bucoTimer = new PixiJsTimer(
            2000,
            () => this.generateBuco());
        this._grattacieliGround.onGrattacieloGeneratoHandler = (grattacielo) =>
            this._onGrattacieloGeneratoHandler?.(grattacielo);
    }

    private _onGrattacieloGeneratoHandler?: (grattacielo: Grattacielo) => void;
    public set onGrattacieloGeneratoHandler(handler: (grattacielo: Grattacielo) => void) {
        this._onGrattacieloGeneratoHandler = handler;
    }

    generateBuco() {
        if (Numbers.randomBetween(0, 1) >= this._bucoProbability)
            return;

        const space = Numbers.randomBetweenInterval(this._dynamicGameParameters.larghezzaBuchi);
        this._grattacieliGround.addExtraGap(space);
    }

    update(time: Ticker) {
        this._grattacieliGround.update();
        this._nuvolificioGround.update(time);
        this._bucoTimer.update(time);
    }

    get drawnGrattacieli() {
        return this._grattacieliGround.grattacieli();
    }

}

export default Foreground;