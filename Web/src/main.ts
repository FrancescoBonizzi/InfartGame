import {Application} from "pixi.js";
import {loadAssets} from "./assets/AssetsLoader";
import LoadingThing from "./uiKit/LoadingThing.ts";

(async () => {
    // Create a PixiJS application.
    const app = new Application();

    // Intialize the application.
    await app.init({
        background: "#1099aa",
        width: 800,
        height: 480,
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

        // Menu
        const menuBackground = infartAssets.menu.background;
        menuBackground.x = 0;
        menuBackground.y = 0;
        app.stage.addChild(menuBackground);

        // Spritesheet
        const playerRun = infartAssets.player.run;
        playerRun.anchor.set(0.5);
        playerRun.x = app.screen.width / 2;
        playerRun.y = app.screen.height / 2;
        playerRun.animationSpeed = 0.4;
        playerRun.play();

        // add it to the stage to render
        app.stage.addChild(playerRun);
    }
    catch (e) {
        alert("Ooops! Errore!");
        console.error(e);
    }
})();
