import {Application} from "pixi.js";
import {loadAssets} from "./assets/AssetsLoader";
import LoadingThing from "./uiKit/LoadingThing.ts";
import Game from "./game.ts";

(async () => {
    // Create a PixiJS application.
    const app = new Application();

    // Intialize the application.
    await app.init({
        background: "#1099aa",
        width: 800,
        height: 480,
        premultipliedAlpha: false
    });

    // Then adding the application's canvas to the DOM body.
    document.body.appendChild(app.canvas);

    try {
        const loadingThing = new LoadingThing(
            app,
            "Farting...");
        loadingThing.show();
        const infartAssets = await loadAssets();
        loadingThing.hide();

        const game = new Game(infartAssets, app);

        app.ticker.add((time) => {
            game.update(time);
        });
    }
    catch (e) {
        alert("Ooops! Errore!");
        console.error(e);
    }
})();

/*
 // Menu
        const testSprite = infartAssets.sprites.menu.background;
        testSprite.x = 0;
        testSprite.y = 0;
        app.stage.addChild(testSprite);

        // Spritesheet
        const playerRun = infartAssets.sprites.player.run;
        playerRun.anchor.set(0.5);
        playerRun.x = app.screen.width / 2;
        playerRun.y = app.screen.height / 2;
        playerRun.animationSpeed = 0.4;
        playerRun.play();

        // add it to the stage to render
        app.stage.addChild(playerRun);

        infartAssets.sounds.music.menu.loop = true;
        infartAssets.sounds.music.menu.play();

 */
