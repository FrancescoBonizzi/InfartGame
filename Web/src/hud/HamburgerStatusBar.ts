import HudHamburger from "./HudHamburger.ts";
import FixedGameParamters from "../services/FixedGameParamters.ts";
import {Container, Point, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";

class HamburgerStatusBar {

    private readonly _hudHamburgers: HudHamburger[];
    private _currentActiveHamburgerIndex: number | null;

    constructor(
        container: Container,
        startingPosition: Point,
        assets: InfartAssets) {

        this._hudHamburgers = [];
        this._currentActiveHamburgerIndex = null;
        const hamburgerSpriteWidth = assets.textures.burger.width;

        for (let h = 0; h <= FixedGameParamters.MaxEatenHamburgers; h++) {
            this._hudHamburgers.push(new HudHamburger(
                container,
                new Point(
                    hamburgerSpriteWidth + h * hamburgerSpriteWidth,
                    startingPosition.y
                ),
                assets
            ))
        }

    }

    hamburgerEaten() {
        if (this._currentActiveHamburgerIndex !== null) {
            if (this._currentActiveHamburgerIndex < FixedGameParamters.MaxEatenHamburgers)
                this._currentActiveHamburgerIndex++;
        } else {
            this._currentActiveHamburgerIndex = 0;
        }

        this._hudHamburgers[this._currentActiveHamburgerIndex].activate();
    }

    farted() {
        if (this._currentActiveHamburgerIndex !== null) {
            this._hudHamburgers[this._currentActiveHamburgerIndex].deactivate();
            this._currentActiveHamburgerIndex--;
        }
    }

    update(time: Ticker) {
        this._hudHamburgers.forEach(h => h.update(time));
    }

}

export default HamburgerStatusBar;