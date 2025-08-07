import InfartAssets from "./assets/InfartAssets.ts";
import {Application, Point, Ticker} from "pixi.js";
import Camera from "./world/Camera.ts";
import BackgroundLandscape from "./background/BackgroundLandscape.ts";
import Controller from "./interaction/Controller.ts";
import Foreground from "./background/Foreground.ts";
import Player from "./player/Player.ts";
import Numbers from "./services/Numbers.ts";
import SoundManager from "./services/SoundManager.ts";
import DynamicGameParameters from "./services/DynamicGameParameters.ts";
import GemmeManager from "./gemme/GemmeManager.ts";
import InfartExplosion from "./particleEmitters/InfartExplosion.ts";
import Hud from "./hud/Hud.ts";

class Game {

    private readonly _camera: Camera;
    private readonly _backgroundLandscape: BackgroundLandscape;
    private readonly _gemmeManager: GemmeManager;
    private readonly _controller: Controller;
    private readonly _foreground: Foreground;
    private readonly _player: Player;
    private readonly _soundManager: SoundManager;
    private readonly _dynamicGameParameters: DynamicGameParameters;
    private readonly _infartExplosion: InfartExplosion;
    private readonly _hud: Hud;

    private _score: number = 0;
    private _isPaused: boolean = false;

    constructor(
        assets: InfartAssets,
        app: Application) {

        this._camera = new Camera(app);
        this._controller = new Controller();
        this._soundManager = new SoundManager();
        this._dynamicGameParameters = new DynamicGameParameters();
        this._backgroundLandscape = new BackgroundLandscape(
            this._camera,
            assets);
        this._foreground = new Foreground(
            this._camera,
            assets,
            this._dynamicGameParameters);
        this._infartExplosion = new InfartExplosion(
            assets,
            this._camera,
            this._soundManager);
        this._hud = new Hud(
            app,
            assets);
        this._player = new Player(
            new Point(240, -600),
            assets,
            this._camera,
            this._foreground,
            this._soundManager,
            this._dynamicGameParameters,
            this._infartExplosion,
            this._hud.getHamburgerStatusBar());
        this._gemmeManager = new GemmeManager(
            assets,
            this._camera,
            this._player,
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

        if (this._controller.consumeKeyPress('space')) {
            this._player.jump();
        } else if (this._controller.consumeKeyPress('KeyP')) {
            this._isPaused = !this._isPaused;
        }

        if (this._isPaused) {
            return;
        }

        this.repositionCamera();
        this._backgroundLandscape.update(time);
        this._foreground.update(time);

        this._player.update(time);
        this._hud.update(time);
        this.updateScore();

        this._gemmeManager.update(time);
        this._infartExplosion.update(time);
    }

    updateScore() {

        const currentScore = this._score;

        if (!this._player.isDead) {
            this._score = Math.floor(this._player.position.x / 100);
        }

        if (currentScore === this._score)
            return;

        this._hud.updateScore(currentScore);

        if (this._score % 30 === 0)
        {
            this._dynamicGameParameters.playerHorizontalSpeed += 40;
            if (this._dynamicGameParameters.larghezzaBuchi.max < 1000) {
                this._dynamicGameParameters.larghezzaBuchi.min += 80;
                this._dynamicGameParameters.larghezzaBuchi.max += 80;
            }
        }
    }

}

export default Game;