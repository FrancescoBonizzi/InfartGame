import InfartAssets from "./assets/InfartAssets.ts";
import {Application, Ticker} from "pixi.js";
import World from "./world/World.ts";
import BackgroundLandscape from "./background/BackgroundLandscape.ts";
import Controller from "./interaction/Controller.ts";
import Foreground from "./background/Foreground.ts";

class Game {
    
    private readonly _world: World;
    private _backgroundLandscape: BackgroundLandscape;
    private _controller: Controller;
    private _foreground: Foreground;
    
    constructor(
        assets: InfartAssets,
        app: Application) {

        this._world = new World(app);
        this._controller = new Controller();

        this._backgroundLandscape = new BackgroundLandscape(
            this._world,
            assets);
        this._foreground = new Foreground(
            this._world,
            assets);
    }

    update(time: Ticker) {

        this._backgroundLandscape.update(time);
        this._foreground.update(time);

        // TODO TMP -> per i salti del player
        if (this._controller.Keys.up.pressed) {
            this._world.y += 20;
        }
        if (this._controller.Keys.down.pressed) {
            this._world.y -= 20;
        }
        if (this._controller.Keys.right.pressed) {
            this._world.x -= 20;
        }
        if (this._controller.Keys.left.pressed) {
            this._world.x += 20;
        }
    }

}

export default Game;