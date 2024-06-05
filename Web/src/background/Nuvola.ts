import World from "../world/World.ts";
import {Sprite, Ticker} from "pixi.js";

class Nuvola {

    private _sprite: Sprite;
    private readonly _velocity: number;
    private readonly _scaleFactor = 0.5;
    private _elapsed: number;

    constructor(
        world: World,
        sprite: Sprite,
        velocity: number
    ) {
        world.addChild(sprite);
        sprite.anchor.set(0.5, 0.5);

        this._sprite = sprite;
        this._velocity = velocity;
        this._elapsed = 0;
    }

    update(time: Ticker) {
        this._sprite.x -= time.deltaTime * this._velocity;
        this._elapsed += time.deltaTime;
        const scaleVariation = 1 + this._scaleFactor * Math.sin(this._elapsed);
        this._sprite.scale.set(scaleVariation);
    }


}

export default Nuvola;