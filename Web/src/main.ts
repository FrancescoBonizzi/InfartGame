import {Application} from "pixi.js";
import {loadAssets} from "./assets/AssetsLoader";
import LoadingThing from "./uiKit/LoadingThing.ts";
import GrattacieliGroup from "./background/GrattacieliGroup.ts";
import World from "./world/World.ts";
import Controller from "./interaction/Controller.ts";

(async () => {
    // Create a PixiJS application.
    const app = new Application();
    const controller = new Controller();

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

        const world = new World(app);

        const grattacieliGroup = new GrattacieliGroup(
            world,
            infartAssets);

        app.ticker.add((time) => {
            grattacieliGroup.update(time);

            // TODO TMP -> per i salti del player
            if (controller.Keys.up.pressed) {
                world.container.y -= 1;
            }
            if (controller.Keys.down.pressed) {
                world.container.y += 1;
            }

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
