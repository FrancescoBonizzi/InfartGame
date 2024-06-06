import World from "../world/World.ts";
import {ColorSource, Sprite, Texture, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";

class Nuvola {

    private readonly _sprite: Sprite;
    private readonly _scaleFactor: number;
    private readonly _startingScale: number;

    private _speed: number;
    private _elapsed: number;

    constructor(
        world: World,
        texture: Texture,
        startingScale: number,
        tint: ColorSource
    ) {
        const sprite = new Sprite(texture);
        world.addChild(sprite);
        sprite.tint = tint;

        this._sprite = sprite;
        this._speed = 0;
        this._elapsed = 0;
        this._scaleFactor = Numbers.randomBetween(0.02, 0.1);
        this._startingScale = startingScale;
    }

    get x() {
        return this._sprite.x;
    }

    set x(x: number) {
        this._sprite.x = x;
    }

    get y() {
        return this._sprite.y;
    }

    set y(y: number) {
        this._sprite.y = y;
    }

    set speed(speed: number) {
        this._speed = speed;
    }

    get width() {
        return this._sprite.width;
    }

    update(time: Ticker) {
        this._sprite.x += time.deltaTime * this._speed;

        this._elapsed += time.deltaTime;
        const scaleVariation = this._startingScale
            + this._scaleFactor
            * Math.sin(this._elapsed / 70);
        this._sprite.scale.set(scaleVariation);
    }

    // TODO:
    // -> La varziaione di scala è un po' troppo veloce e bruttina,
    // in infart originale era molto meno invadente
    // -> Ancora errori di spawn, sopratutto a destra


}

export default Nuvola;