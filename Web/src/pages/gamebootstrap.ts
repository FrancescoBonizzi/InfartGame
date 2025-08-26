import {Application} from "pixi.js";
import {loadAssets} from "../assets/AssetsLoader.ts";
import LoadingThing from "../uiKit/LoadingThing.ts";
import Game from "../Game.ts";
import {SoundManagerInstance} from "../services/SoundInstance.ts";

let app: Application | null = null;

export async function initGame(container: HTMLElement) {

    container.innerHTML = `
    <div id="game-container"></div>
  `;


    const gameContainer = document.getElementById('game-container');

    if (!gameContainer) {
        console.error('Game container non trovato!');
        return;
    }

    app = new Application();

    await app.init({
        background: '#1CB3DE',
        width: 800,
        height: 480,
        premultipliedAlpha: false,
        antialias: true,
        autoDensity: true,
    });

    document.body.appendChild(app.canvas);

    try {
        const loadingThing = new LoadingThing(app);
        loadingThing.show();
        const infartAssets = await loadAssets();
        loadingThing.hide();

        const game = new Game(
            infartAssets,
            app,
            SoundManagerInstance);
        SoundManagerInstance.playGameSoundTrack();

        app.ticker.add((time) => {
            game.update(time);
        });
    }
    catch (e) {
        alert("Ooops! Errore!");
        console.error(e);
    }
};

export function destroyGame() {
    if (app) {
        app.destroy(true, { children: true });
        app = null;
    }
}


