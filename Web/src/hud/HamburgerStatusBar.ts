import HudHamburger from "./HudHamburger.ts";
import FixedGameParamters from "../services/FixedGameParameters.ts";
import {Container, Point, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import HudText from "./HudText.ts";
import Numbers from "../services/Numbers.ts";

class HamburgerStatusBar {

    private readonly _hudHamburgers: HudHamburger[];
    private readonly _hamburgersText: HudText;
    private readonly _container: Container;
    private readonly _blinkIntervalMs = 300;

    private _isBlinking = false;
    private _blinkElapsed = 0;
    private _currentActiveHamburgerIndex: number | null;

    constructor(
        container: Container,
        startingPosition: Point,
        assets: InfartAssets) {

        this._container = new Container();
        container.addChild(this._container);

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
                this._container,
                hamburgerPosition,
                assets
            ))
        }

        this._hamburgersText = new HudText(
            this._container,
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

        if (this._isBlinking) {
            this._blinkElapsed += time.deltaMS;
            if (this._blinkElapsed >= this._blinkIntervalMs) {
                this._blinkElapsed = 0;
                this._container.visible = !this._container.visible;
            }
        }
    }

    private updateHamburgersText() {
        this._hamburgersText.updateText(
            this._currentActiveHamburgerIndex !== null
                ? HamburgersMessages[this._currentActiveHamburgerIndex]
                : "",
        );


        this._isBlinking = this._currentActiveHamburgerIndex === 3;

        if (!this._isBlinking) {
            this._container.visible = true;
            this._blinkElapsed = 0;
        }
    }
}

const HamburgersMessages: Record<number, string> = {
    0: "Parametri in equilibrio",
    1: "Colesterolo in riscaldamento",
    2: "Trigliceridi in ondata di calore",
    3: "Evento estremo imminente!",
};

export default HamburgerStatusBar;