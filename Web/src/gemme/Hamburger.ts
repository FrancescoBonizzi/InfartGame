import Gemma from "./Gemma.ts";
import Camera from "../world/Camera.ts";
import {Point, Rectangle, Texture, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";

type GemmaState = "idle" | "throwing" | "gone";

class Hamburger extends Gemma {

    private _state: GemmaState = "idle";

    private _vx = 0;
    private _vy = 0;
    private _ax = 0;
    private _ay = 0;
    private _spin = 0;
    private _fadePerMs = 0;

    constructor(
        world: Camera,
        texture: Texture,
        position: Point) {
        super(world, texture, position);

    }

    get isGone() {
        return this._state === "gone";
    }

    // Durante/ dopo il lancio disattivo le collisioni
    override get collisionRectangle() {
        if (this._state !== "idle")
            return new Rectangle(0, 0, 0, 0);

        return super.collisionRectangle;
    }

    override update(time: Ticker) {
        const dt = time.deltaMS;

        if (this._state === "idle") {
            super.update(time);
            return;
        }

        if (this._state === "throwing") {

            this._vx += this._ax * dt;
            this._vy += this._ay * dt;

            this.sprite.x += this._vx * dt;
            this.sprite.y += this._vy * dt;

            this.sprite.rotation += this._spin * dt;

            if (this._fadePerMs > 0) {
                this.sprite.alpha = Math.max(0, this.sprite.alpha - this._fadePerMs * dt);
            }

            const alphaGone = this.sprite.alpha <= 0.001;
            const farAway = this.sprite.y > 2000;

            if (alphaGone || farAway) {
                this._state = "gone";
                this.sprite.visible = false;
            }
        }
    }

    /**
     * Lancia l'hamburger in basso a destra e lo fa svanire/ruotare.
     */
    public throwAway() {
        if (this._state !== "idle") return;

        this._vx = Numbers.randomBetween(0.25, 0.75);
        this._vy = Numbers.randomBetween(0.35, 0.85);
        this._ax = Numbers.randomBetween(0.0015, 0.003);
        this._ay = Numbers.randomBetween(0.0008, 0.002);
        this._spin = Numbers.randomBetween(0.01, 0.02) * (Math.random() < 0.5 ? -1 : 1);
        this._fadePerMs = Numbers.randomBetween(0.001, 0.0025);

        this._state = "throwing";
        this.sprite.visible = true;
        this.sprite.alpha = 1;
    }

}

export default Hamburger;