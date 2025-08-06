import HudHamburger from "./HudHamburger.ts";
import FixedGameParamters from "../services/FixedGameParameters.ts";
import {Container, Point, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import HudText from "./HudText.ts";
import Numbers from "../services/Numbers.ts";

class HamburgerStatusBar {

    private readonly _hudHamburgers: HudHamburger[];
    private readonly _hamburgersText: HudText;

    private _currentActiveHamburgerIndex: number | null;

    constructor(
        container: Container,
        startingPosition: Point,
        assets: InfartAssets) {

        this._hudHamburgers = [];
        this._currentActiveHamburgerIndex = null;
        const hamburgerSpriteWidth = assets.textures.burger.width;

        let hamburgerPosition: Point | null;

        for (let h = 0; h <= FixedGameParamters.MaxEatenHamburgers; h++) {
            hamburgerPosition = new Point(
                hamburgerSpriteWidth + h * hamburgerSpriteWidth,
                startingPosition.y
            );
            this._hudHamburgers.push(new HudHamburger(
                container,
                hamburgerPosition,
                assets
            ))
        }

        this._hamburgersText = new HudText(
            container,
            Numbers.addPoints(hamburgerPosition!, new Point(hamburgerSpriteWidth, 0)),
                new Point(0, 0.5)
        );

        this.updateHamburgersText();
    }

    hamburgerEaten() {
        if (this._currentActiveHamburgerIndex !== null) {
            if (this._currentActiveHamburgerIndex < FixedGameParamters.MaxEatenHamburgers)
                this._currentActiveHamburgerIndex++;
        } else {
            this._currentActiveHamburgerIndex = 0;
        }

        this._hudHamburgers[this._currentActiveHamburgerIndex].activate();
        this.updateHamburgersText();
    }

    farted() {
        if (this._currentActiveHamburgerIndex !== null) {
            this._hudHamburgers[this._currentActiveHamburgerIndex].deactivate();
            this._currentActiveHamburgerIndex--;
        }
        this.updateHamburgersText();
    }

    update(time: Ticker) {
        this._hudHamburgers.forEach(h => h.update(time));
    }

    private updateHamburgersText() {
        this._hamburgersText.updateText(
            this._currentActiveHamburgerIndex !== null
                ? HamburgersMessages[this._currentActiveHamburgerIndex]
                : "",

        );
    }

}

const HamburgersMessages: Record<number, string> = {
    0: "Parametri ok. CO₂ bassa.",
    1: "Colesterolo in riscaldamento. CO₂ su.",
    2: "Trigliceridi in decollo. Emissioni alte.",
    3: "Pressione all’erta. Impronta idrica alta.",
    4: "Evento avverso imminente. Impatto climatico critico.",
};

export default HamburgerStatusBar;