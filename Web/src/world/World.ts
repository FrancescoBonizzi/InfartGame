import {Application, Container, Renderer} from "pixi.js";

class World {

    private readonly _world: Container;

    constructor(app: Application<Renderer>) {
        this._world = new Container();
        this._world.height = 1500;
        this._world.width = 4000;

        this._world.x = 0;
        this._world.y = app.screen.height;
        this._world.pivot.x = 0;
        this._world.pivot.y = this._world.height;

        app.stage.addChild(this._world);
    }

    get x() {
        return this._world.x;
    }

    get y() {
        return this._world.y;
    }

    set x(x: number) {
        this._world.x = x;
    }

    set y(y: number) {
        this._world.y = y;
    }

    addChild(child: Container) {
        this._world.addChild(child);
    }

}

export default World;