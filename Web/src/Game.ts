import InfartAssets from "./assets/InfartAssets.ts";
import {Application, Point, Ticker} from "pixi.js";
import Camera from "./world/Camera.ts";
import BackgroundLandscape from "./background/BackgroundLandscape.ts";
import Controller from "./interaction/Controller.ts";
import Foreground from "./background/Foreground.ts";
import Player from "./player/Player.ts";
import Numbers from "./services/Numbers.ts";

class Game {

    private readonly _camera: Camera;
    private readonly _backgroundLandscape: BackgroundLandscape;
    private readonly _controller: Controller;
    private readonly _foreground: Foreground;
    private readonly _player: Player;
    private _isPaused: boolean = false;

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

    set isPaused(value: boolean) {
        this._isPaused = value;
    }

    get isPaused(): boolean {
        return this._isPaused;
    }

    private repositionCamera() {
        let newCameraX = this._player.position.x - 150;
        let newCameraY = this._player.position.y + 200;
        const heightLimit = -this._camera.worldHeight - this._camera.height;

        if (newCameraY < heightLimit) {
            newCameraY = heightLimit;
        }
        else if (newCameraY > 0) {
            newCameraY = 0;
        }

        this._camera.x = Numbers.lerp(this._camera.x, newCameraX, 0.08);
        this._camera.y = Numbers.lerp(this._camera.y, newCameraY, 0.08);
    }

    update(time: Ticker) {

        if (this._controller.Keys.KeyP.pressed) {
            this._isPaused = !this._isPaused;
            console.log('Game paused:', this._isPaused);
        }

        if (this._isPaused) {
            return;
        }

        this.repositionCamera();

        this._backgroundLandscape.update(time);
        this._foreground.update(time);

        if (this._controller.Keys.space.pressed) {
            this._player.jump(650);
        }

        this._player.update(time);
    }

}

export default Game;