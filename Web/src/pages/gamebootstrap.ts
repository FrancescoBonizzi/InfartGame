import {Application} from "pixi.js";
import {loadAssets} from "../assets/AssetsLoader.ts";
import LoadingThing from "../uiKit/LoadingThing.ts";
import Game from "../Game.ts";
import {SoundManagerInstance} from "../services/SoundInstance.ts";

const GAME_W = 800;
const GAME_H = 480;

let app: Application | null = null;
let gameContainer: HTMLDivElement | null = null;
let orientationOverlay: HTMLDivElement | null = null;
let resizeObserver: ResizeObserver | null = null;

export async function initGame(container: HTMLElement) {

    if (app) {
        destroyGame();
    }

    container.innerHTML = `
    <div id="game-wrapper">
        <div id="game-container"></div>
        <div id="orientation-overlay">
          <svg class="rotate-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"
               fill="none" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M21 12a9 9 0 1 1-9-9c2.52 0 4.93 1 6.74 2.74L21 8" />
            <path d="M21 3v5h-5" />
          </svg>
          <p>Ruota il dispositivo</p>
        </div>
      </div>
  `;

    gameContainer = container.querySelector<HTMLDivElement>("#game-container");
    if (!gameContainer) {
        console.error("Game container non trovato");
        return;
    }

    orientationOverlay = container.querySelector<HTMLDivElement>("#orientation-overlay");

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

    try { await screen.orientation.lock('landscape'); } catch {}

    resizeObserver = new ResizeObserver(resize);
    resizeObserver.observe(gameContainer);
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

    if (orientationOverlay) {
        const isPortrait = containerH > containerW;
        orientationOverlay.classList.toggle('visible', isPortrait);
        if (isPortrait) {
            app.ticker.stop();
        } else {
            app.ticker.start();
        }
    }

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

    resizeObserver?.disconnect();
    resizeObserver = null;
    orientationOverlay = null;
    try { screen.orientation.unlock(); } catch {}
    app.destroy(true, {children: true});
    app = null;
    gameContainer = null;
}


