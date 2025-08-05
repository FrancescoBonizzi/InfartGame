import HudHamburger from "./HudHamburger.ts";
import FixedGameParamters from "../services/FixedGameParamters.ts";
import {Application, Point, Renderer} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";

class HamburgerStatusBar {

    private readonly _hudHamburgers: HudHamburger[];
    private _currentActiveHamburgerIndex: number | null;

    constructor(
        app: Application<Renderer>,
        assets: InfartAssets) {

        this._hudHamburgers = [];
        this._currentActiveHamburgerIndex = null;

        for(let h = 0; h < FixedGameParamters.MaxEatenHamburgers; h++) {
            this._hudHamburgers.push(new HudHamburger(
                app,
                new Point(
                    100 + h * 100,
                    app.screen.height - 10
                ),
                assets
            ))
        }

    }

    hamburgerEaten() {
        if (this._currentActiveHamburgerIndex !== null) {
            if (this._currentActiveHamburgerIndex < FixedGameParamters.MaxEatenHamburgers - 1)
                this._currentActiveHamburgerIndex++;
        }
        else {
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

}

export default HamburgerStatusBar;