import InfartAssets from "../assets/InfartAssets.ts";
import {Sprite} from "pixi.js";
import World from "../world/World.ts";

class BackgroundSky {

    private readonly _sprite: Sprite;
    private _world: World;

    constructor(
        assets: InfartAssets,
        world: World) {

        this._world = world;
        this._sprite = assets.sprites.background;
        this._sprite.anchor.set(0, 1);
        this._sprite.y = 0;
        this._sprite.scale.set(1.8);

        world.addChild(this._sprite);
    }

    update() {
        this._sprite.x = -this._world.x;
    }
}

export default BackgroundSky;