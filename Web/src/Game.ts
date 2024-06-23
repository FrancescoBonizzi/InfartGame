import InfartAssets from "./assets/InfartAssets.ts";
import {Application, Point, Ticker} from "pixi.js";
import Camera from "./world/Camera.ts";
import BackgroundLandscape from "./background/BackgroundLandscape.ts";
import Controller from "./interaction/Controller.ts";
import Foreground from "./background/Foreground.ts";
import Player from "./player/Player.ts";

class Game {
    
    private readonly _camera: Camera;
    private _backgroundLandscape: BackgroundLandscape;
    private _controller: Controller;
    private _foreground: Foreground;
    private _player: Player;
    
    constructor(
        assets: InfartAssets,
        app: Application) {

        this._camera = new Camera(app);
        this._controller = new Controller();

        this._backgroundLandscape = new BackgroundLandscape(
            this._camera,
            assets);
        this._foreground = new Foreground(
            this._camera,
            assets);
        this._player = new Player(
            new Point(240, -600),
            assets,
            this._camera,
            this._foreground);
    }

    update(time: Ticker) {

        this._backgroundLandscape.update(time);
        this._foreground.update(time);

        // TODO TMP -> per i salti del player
        if (this._controller.Keys.up.pressed) {
            this._camera.y -= 20;
        }
        if (this._controller.Keys.down.pressed) {
            this._camera.y += 20;
        }

        if (this._controller.Keys.right.pressed) {
            // Muovo il mondo a sinistra,
            // quindi sposto la telecamera a destra
            this._camera.x += 20;
        }

        if (this._controller.Keys.left.pressed) {
            this._camera.x -= 20;
        }

        this._player.update(time);
    }

}

export default Game;