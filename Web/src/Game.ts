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
            assets,
            this._dynamicGameParameters);
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
            assets,
            this._soundManager);
        this._player = new Player(
            new Point(600, -500),
            assets,
            this._camera,
            this._foreground,
            this._soundManager,
            this._dynamicGameParameters,
            this._infartExplosion,
            this._hud);
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

    private _heightK = 0;

    private repositionCamera() {

        const REF_Y     = 1000;
        const MIN_Z     = 0.5;
        const MAX_Z     = 1.0;
        const Z_SMOOTH  = 0.04;
        const DEADZONE_K = 0.02; // deadzone vicino al suolo

        let newCameraX = this._player.position.x - 150;
        let newCameraY = this._player.position.y + 200;

        // altezza sopra il suolo -> 0..1
        const heightAbove = Math.max(0, -this._player.position.y);
        const t = Numbers.clamp01(heightAbove / REF_Y);

        // smoothing di t per evitare jitter
        this._heightK = Numbers.lerp(this._heightK, t, 0.06);

        // deadzone: se molto basso, azzera
        const k0 = (this._heightK < DEADZONE_K) ? 0 : this._heightK;

        // easing (smoothstep)
        const k = k0 * k0 * (3 - 2 * k0);

        const HARD_MIN_Z = 0.7;
        const HARD_MAX_Z = 1.0;

        const unclamped = MAX_Z - (MAX_Z - MIN_Z) * k;
        const targetZoom = Numbers.clamp(unclamped, HARD_MIN_Z, HARD_MAX_Z);

        // zoom proporzionale all'altezza
        const nextZoom   = Numbers.lerp(this._camera.getZoom(), targetZoom, Z_SMOOTH);
        this._camera.setZoomAround(nextZoom, this._player.position.x, this._player.position.y);

        const FALL_BIAS_FRAC = 0.45; // 45% dell’altezza visibile
        newCameraY += this._camera.height * FALL_BIAS_FRAC * k;

        // bias verticale verso il basso
        //newCameraY += FALL_BIAS_MAX * k;

        // clamp verticale (usa camera.height in unità mondo)
        const minY = -(this._camera.worldHeight - this._camera.height);
        const maxY = 0;
        if (newCameraY < minY)
            newCameraY = minY;
        if (newCameraY > maxY)
            newCameraY = maxY;

        // applica posizione
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

        if (this._score % 30 === 0) {

            if (this._dynamicGameParameters.playerHorizontalSpeed < 600)
                this._dynamicGameParameters.playerHorizontalSpeed += 50;
            if (this._dynamicGameParameters.larghezzaBuchi.max < 1000) {
                this._dynamicGameParameters.larghezzaBuchi.min += 80;
                this._dynamicGameParameters.larghezzaBuchi.max += 80;
            }
        }
    }

}

export default Game;