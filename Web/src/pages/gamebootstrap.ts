import {Application} from "pixi.js";
import {loadAssets} from "../assets/AssetsLoader.ts";
import LoadingThing from "../uiKit/LoadingThing.ts";
import Game from "../Game.ts";
import {SoundManagerInstance} from "../services/SoundInstance.ts";

const GAME_W = 800;
const GAME_H = 480;

let app: Application | null = null;
let gameContainer: HTMLDivElement | null = null;

export async function initGame(container: HTMLElement) {

    if (app) {
        destroyGame();
    }

    container.innerHTML = `
    <div id="game-container" style="width: 100vw; height: 100vh; display: flex; align-items: center; justify-content: center"></div>
  `;

    gameContainer = container.querySelector<HTMLDivElement>("#game-container");
    if (!gameContainer) {
        console.error("Game container non trovato");
        return;
    }

    app = new Application();

    await app.init({
        background: '#1CB3DE',
        width: GAME_W,
        height: GAME_H,
        premultipliedAlpha: false,
        antialias: true,
        autoDensity: true,
        resolution: Math.min(window.devicePixelRatio || 1, 2),
    });

    window.addEventListener('resize', resize);
    resize();

    gameContainer.appendChild(app.canvas);

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
}

function resize() {
    if (!app || !gameContainer) return;

    const containerW = gameContainer.clientWidth;
    const containerH = gameContainer.clientHeight;

    if (containerW === 0 || containerH === 0) return;

    // Calcola il fattore di scala mantenendo l'aspect ratio
    const scale = Math.min(containerW / GAME_W, containerH / GAME_H);

    // Dimensioni finali del canvas
    const canvasW = Math.floor(GAME_W * scale);
    const canvasH = Math.floor(GAME_H * scale);

    // Ridimensiona il renderer mantenendo le dimensioni logiche del gioco
    app.renderer.resize(GAME_W, GAME_H);

    // Scala il canvas via CSS
    app.canvas.style.width = canvasW + 'px';
    app.canvas.style.height = canvasH + 'px';

    // Non scalare lo stage - mantieni le coordinate logiche
    app.stage.scale.set(1);
    app.stage.position.set(0, 0);
}

export function destroyGame() {
    if (!app) {
        return;
    }

    window.removeEventListener("resize", resize);
    app.destroy(true, {children: true});
    app = null;
    gameContainer = null;
}


