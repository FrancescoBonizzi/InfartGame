import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";
import PowerUp from "./PowerUp.ts";
import {Point, Texture, Ticker} from "pixi.js";
import Hamburger from "./Hamburger.ts";
import Player from "../player/Player.ts";
import CollisionSolver from "../services/CollisionSolver.ts";
import PowerUpTypes from "./PowerUpTypes.ts";
import Grattacielo from "../background/Grattacielo.ts";
import Numbers from "../services/Numbers.ts";
import Foreground from "../background/Foreground.ts";
import PowerUpJalapeno from "./PowerUpJalapeno.ts";
import PowerUpBean from "./PowerUpBean.ts";
import PowerUpBroccolo from "./PowerUpBroccolo.ts";

class GemmeManager {

    private readonly _camera: Camera;
    private readonly _maxActiveHamburgers = 30;
    private readonly _hamburgerTexture: Texture;
    private readonly _hamburgerProbability = 0.4;

    private readonly _powerUpProbability = 0.8;
    private readonly _assets: InfartAssets;
    private readonly _player: Player;

    private _hambugers: Hamburger[];
    private _activePowerup: PowerUp | null;

    constructor(
        assets: InfartAssets,
        camera: Camera,
        player: Player,
        foreground: Foreground) {

        this._camera = camera;
        this._hambugers = [];
        this._assets = assets;
        this._hamburgerTexture = assets.textures.burger;
        this._activePowerup = null;
        this._player = player;

        foreground.onGrattacieloGeneratoHandler = this.onGrattacieloGeneratoHandler.bind(this);
        foreground.drawnGrattacieli
            .slice(10)
            .forEach(grattacielo => {
                this.onGrattacieloGeneratoHandler(grattacielo);
            });
    }

    private spawnPowerUp(
        where: Point) {

        // Only one active powerup at a time
        if (this._activePowerup) {
            return;
        }

        const randomPowerUpType = this.getRandomPowerUp();
        this._activePowerup = this.powerUpFactory(randomPowerUpType, where);
    }

    private powerUpFactory(
        powerUpType: PowerUpTypes,
        where: Point): PowerUp {

        switch (powerUpType) {
            case PowerUpTypes.Jalapeno:
                return new PowerUpJalapeno(
                    this._camera,
                    this._assets,
                    where
                );
            case PowerUpTypes.Bean:
                return new PowerUpBean(
                    this._camera,
                    this._assets,
                    where
                );
            case PowerUpTypes.Broccolo:
                return new PowerUpBroccolo(
                    this._camera,
                    this._assets,
                    where
                );
        }

    }

    private getRandomPowerUp(): PowerUpTypes {
        const values = Object
            .values(PowerUpTypes)
            .filter(value => typeof value === 'number');
        const randomIndex = Math.floor(Math.random() * values.length);
        return values[randomIndex] as PowerUpTypes;
    }

    private spawnHamburger(where: Point) {
        if (this._hambugers.length >= this._maxActiveHamburgers)
            return;

        const newHamburger = new Hamburger(
            this._camera,
            this._hamburgerTexture,
            where
        )

        this._hambugers.push(newHamburger);
    }

    update(ticker: Ticker) {
        this.updateHamburgers(ticker);
        this.updatePowerUp(ticker);
    }

    private updatePowerUp(ticker: Ticker) {

        if (!this._activePowerup)
            return;

        if (!this._activePowerup.hasBeenActivatedByPlayer) {
            if (this._camera.isOutOfCameraLeft(this._activePowerup)) {
                this._camera.removeFromWorld(this._activePowerup.sprite);
                this._activePowerup = null;
            }

            return;
        }

        this._activePowerup.update(ticker);
        if (this._activePowerup.isExpired()) {
            this._player.deactivatePowerUp();
            this._activePowerup = null;
        }
    }

    private updateHamburgers(ticker: Ticker) {
        this._hambugers = this._hambugers.filter(hamburger =>
            !this._camera.isOutOfCameraLeft(hamburger) && !hamburger.isGone);
        this.checkPlayerCollisionWithGemme();
        this._hambugers.forEach(hamburger => hamburger.update(ticker));
    }

    checkPlayerCollisionWithGemme() {

        const collidedHamburger = this.playerCollidedWithHamburger(this._player);
        if (collidedHamburger) {
            if(this._player.activePowerUp === null || this._player.activePowerUp.getPowerUpType() !== PowerUpTypes.Broccolo) {
                this._player.hamburgerEaten();
                this._hambugers = this._hambugers.filter(hamburger => hamburger !== collidedHamburger);
                this._camera.removeFromWorld(collidedHamburger.sprite);
                return;
            }

            if(this._player.activePowerUp.getPowerUpType() === PowerUpTypes.Broccolo) {
                collidedHamburger.throwAway();
            }
        }

        if (this.playerCollidedWithPowerUp(this._player)
            && this._activePowerup
            && !this._activePowerup.hasBeenActivatedByPlayer) {
            this._player.activatePowerUp(this._activePowerup);
            this._activePowerup.activate();
        }
    }

    private playerCollidedWithHamburger(player: Player): Hamburger | null {
        const collidedWith = CollisionSolver.checkCollisionsReturnCollidingObjectSpecific(
            player,
            this._hambugers);

        if (!collidedWith)
            return null;

        return collidedWith;
    }

    private playerCollidedWithPowerUp(player: Player): boolean {

        if (!this._activePowerup)
            return false;

        const collidedWith = CollisionSolver.checkCollisionsReturnCollidingObjectSpecific(
            player,
            [this._activePowerup]);
        if (!collidedWith)
            return false;

        this._camera.removeFromWorld(collidedWith.sprite);
        return true;
    }

    private onGrattacieloGeneratoHandler(grattacielo: Grattacielo) {

        const where = new Point(
            grattacielo.x + (grattacielo.width / 2),
            grattacielo.y - grattacielo.height - 70);

        if (Numbers.randomBetween(0, 1) < this._hamburgerProbability) {
            this.spawnHamburger(where);
        } else if (Numbers.randomBetween(0, 1) < this._powerUpProbability) {
            this.spawnPowerUp(where);
        }

    }

}

export default GemmeManager;