import World from "../world/World.ts";
import {ColorSource, Sprite, Texture, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";

class Nuvola {

    private readonly _sprite: Sprite;
    private readonly _scaleFactor: number;
    private readonly _randomOffset: number;
    private readonly _startingScale: number;
    private readonly _shouldAnimateScale: boolean;

    private _speed: number;
    private _elapsed: number;

    constructor(
        world: World,
        texture: Texture,
        startingScale: number,
        tint: ColorSource,
        shouldAnimateScale: boolean
    ) {
        const sprite = new Sprite(texture);
        world.addChild(sprite);
        sprite.tint = tint;

        this._sprite = sprite;
        this._speed = 0;
        this._elapsed = 0;

        this._shouldAnimateScale = shouldAnimateScale;

        // The same cloud can be scaled differently
        this._scaleFactor = Numbers.randomBetween(0, 0.1);
        this._randomOffset = Numbers.randomBetween(0, 50);
        this._startingScale = startingScale;
        this._sprite.scale.set(startingScale);
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

        if (this._shouldAnimateScale) {
            const scaleVariation = this._startingScale
                + this._scaleFactor
                * Math.sin((this._elapsed + this._randomOffset) / 70);
            this._sprite.scale.set(scaleVariation);
        }
    }
}

export default Nuvola;