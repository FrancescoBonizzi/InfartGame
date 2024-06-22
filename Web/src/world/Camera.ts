import {Application, Container, Renderer} from "pixi.js";

class Camera {

    private readonly _worldHeight = 1500;

    private readonly _world: Container;
    private readonly _width: number;
    private readonly _height: number;

    constructor(app: Application<Renderer>) {
        this._world = new Container();
        this._world.height = this._worldHeight;

        this._width = app.screen.width;
        this._height = app.screen.height;

        this._world.x = 0;
        this._world.y = app.screen.height;
        this._world.pivot.x = 0;
        this._world.pivot.y = this._world.height;

        app.stage.addChild(this._world);
    }

    get width() {
        return this._width;
    }

    get height() {
        return this._height;
    }

    get x() {
        return -this._world.x;
    }

    get y() {
        return -this._world.y;
    }

    set x(x: number) {
        this._world.x = -x;
    }

    set y(y: number) {
        this._world.y = -y;
    }

    addToWorld(child: Container) {
        this._world.addChild(child);
    }

    /*
      Quando hai un Sprite (come il grattacielo) all'interno di un Container (come World),
      le coordinate del Sprite sono relative al suo Container.
      Quindi, se grattacielo.x è 100, significa che il grattacielo è posizionato a 100 pixel
      dall'origine del Container (World).

      D'altra parte, il Container stesso (World) ha delle coordinate
      rispetto alla scena principale (l'intera applicazione).
      Quindi, this._world.x rappresenta la posizione orizzontale
      di World, worldX è la coordinata x nel mondo di gioco.
   */
    private worldToScreenX(worldX: number) {
        return worldX - this.x;
    }

    /* Si basa sul fatto che l'Anchor sia a sinistra */
    isOutOfCameraLeft(sprite: { x: number, width: number }) {
        const globalX = this.worldToScreenX(sprite.x);
        return globalX + sprite.width <= 0;
    }
}

export default Camera;