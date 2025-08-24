import Gemma from "./Gemma.ts";
import Camera from "../world/Camera.ts";
import {Point, Rectangle, Texture, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";

type GemmaState = "idle" | "throwing" | "gone";

class Hamburger extends Gemma {

    private _state: GemmaState = "idle";

    private _vx = 0;          // px/ms
    private _vy = 0;          // px/ms
    private _ax = 0;          // px/ms^2
    private _ay = 0;          // px/ms^2
    private _spin = 0;        // rad/ms
    private _fadePerMs = 0;   // alpha/ms
    private _shrinkPerMs = 0; // scale/ms
    private _onGone?: () => void;

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
        if (this._state !== "idle") return new Rectangle(0, 0, 0, 0);
        return super.collisionRectangle;
    }

    override update(time: Ticker) {
        const dt = time.deltaMS;

        if (this._state === "idle") {
            super.update(time);
            return;
        }

        if (this._state === "throwing") {
            // Cinematica base
            this._vx += this._ax * dt;
            this._vy += this._ay * dt;

            this.sprite.x += this._vx * dt;
            this.sprite.y += this._vy * dt;

            this.sprite.rotation += this._spin * dt;

            if (this._fadePerMs > 0) {
                this.sprite.alpha = Math.max(0, this.sprite.alpha - this._fadePerMs * dt);
            }
            if (this._shrinkPerMs > 0) {
                const s = Math.max(0, this.sprite.scale.x - this._shrinkPerMs * dt);
                this.sprite.scale.set(s, s);
            }

            const alphaGone = this.sprite.alpha <= 0.001;
            const farAway = this.sprite.y > 2000; // soglia ampia

            if (alphaGone || farAway) {
                this._state = "gone";
                this.sprite.visible = false;
                this._onGone?.();
            }
        }
    }

    /**
     * Lancia l'hamburger in basso a destra e lo fa svanire/ruotare.
     */
    public throwAway(opts?: {
        velocityX?: number;     // px/ms
        velocityY?: number;     // px/ms (positiva = verso il basso)
        accelX?: number;        // px/ms^2
        accelY?: number;        // px/ms^2
        spin?: number;          // rad/ms
        fadePerMs?: number;     // alpha/ms
        shrinkPerMs?: number;   // scale/ms
        onGone?: () => void;
    }) {
        if (this._state !== "idle") return;

        const o = opts ?? {};
        this._vx = o.velocityX ?? Numbers.randomBetween(0.35, 0.55);
        this._vy = o.velocityY ?? Numbers.randomBetween(0.45, 0.65);
        this._ax = o.accelX ?? 0.000;
        this._ay = o.accelY ?? 0.0012; // gravità leggera
        this._spin = o.spin ?? Numbers.randomBetween(0.003, 0.006) * (Math.random() < 0.5 ? -1 : 1);
        this._fadePerMs = o.fadePerMs ?? 0.0016;
        this._shrinkPerMs = o.shrinkPerMs ?? 0.0007;
        this._onGone = o.onGone;

        this._state = "throwing";
        this.sprite.visible = true;
        this.sprite.alpha = 1;
        // anchor già impostato in Gemma a (0.5, 0)
    }

    /**
     * Opzionale: reset per pooling/riutilizzo.
     */
    public resetTo(position: Point) {
        this._state = "idle";
        this.sprite.position.set(position.x, position.y);
        this.sprite.rotation = 0;
        this.sprite.alpha = 1;
        this.sprite.scale.set(1, 1);
        this.sprite.visible = true;
    }

}

export default Hamburger;