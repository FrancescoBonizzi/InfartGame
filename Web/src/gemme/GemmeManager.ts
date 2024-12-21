import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";
import PowerUp from "./PowerUp.ts";
import {Point, Texture, Ticker} from "pixi.js";
import Hamburger from "./Hamburger.ts";
import Player from "../player/Player.ts";
import CollisionSolver from "../services/CollisionSolver.ts";
import PowerUpTypes from "./PowerUpTypes.ts";
import IHasCollisionRectangle from "../IHasCollisionRectangle.ts";
import Grattacielo from "../background/Grattacielo.ts";
import Numbers from "../services/Numbers.ts";
import Foreground from "../background/Foreground.ts";

class GemmeManager {

    private readonly _camera: Camera;
    private readonly _maxActiveHamburgers = 20;
    private readonly _powerUpsTextures: Record<PowerUpTypes, Texture>;
    private readonly _hamburgerTexture: Texture;
    private readonly _hamburgerProbability = 0.4;
    private readonly _powerUpProbability = 0.005;

    private _hambugers: Hamburger[];
    private _activePowerup: PowerUp | null;
    private readonly _player: Player;

    constructor(
        assets: InfartAssets,
        camera: Camera,
        player: Player,
        foreground: Foreground) {

        this._camera = camera;
        this._hambugers = [];
        this._powerUpsTextures = {
            [PowerUpTypes.Bean]: assets.textures.bean,
            [PowerUpTypes.Broccolo]: assets.textures.verdura,
            [PowerUpTypes.Jalapeno]: assets.textures.jalapenos
        };
        this._hamburgerTexture = assets.textures.burger;
        this._activePowerup = null;
        this._player = player;
        foreground.onGrattacieloGeneratoHandler = this.onGrattacieloGeneratoHandler.bind(this);
    }

    private addPowerUp(
        where: Point) {

        if (this._activePowerup) {
            return;
        }

        const randomPowerUpType = this.getRandomPowerUp();
        this._activePowerup = new PowerUp(
            this._camera,
            this._powerUpsTextures[randomPowerUpType],
            randomPowerUpType,
            where
        );
    }

    private getRandomPowerUp() : PowerUpTypes {
        const values = Object.values(PowerUpTypes).filter(value => typeof value === 'number');
        const randomIndex = Math.floor(Math.random() * values.length);
        return values[randomIndex] as PowerUpTypes;
    }

    private addHamburger(where: Point) {
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

        if (this._activePowerup) {
            if (this._camera.isOutOfCameraLeft(this._activePowerup)) {
                this._activePowerup = null;
            } else {
                this._activePowerup.update(ticker);
            }
        }

        const hamburgersToRemove: Hamburger[] = [];
        this._hambugers.forEach(hamburger => {
            if (this._camera.isOutOfCameraLeft(hamburger)) {
                hamburgersToRemove.push(hamburger);
            } else {
                hamburger.update(ticker);
            }
        })

        this._hambugers = this._hambugers.filter(hamburger =>
            !hamburgersToRemove.includes(hamburger));
        this.checkPlayerCollisionWithGemme();
    }
    
    checkPlayerCollisionWithGemme() {
        if (this.playerCollidedWithHamburger(this._player)) {
            // TODO
        }

        const powerUpType = this.playerCollidedWithPowerUp(this._player);
        if (powerUpType) {
            // TODO
        }
    }

    private playerCollidedWithHamburger(player: Player): boolean {
        const collidedWith = CollisionSolver.checkCollisionsReturnCollidingObjectSpecific(
            player,
            this._hambugers);

        if (!collidedWith)
            return false;

        this._hambugers = this._hambugers.filter(hamburger => hamburger !== collidedWith);
        return true;
    }

    private playerCollidedWithPowerUp(player: Player): PowerUpTypes | null {

        if (!this._activePowerup)
            return null;

        const collidedWith = CollisionSolver.checkCollisionsReturnCollidingObjectSpecific(
            player,
            [this._activePowerup as IHasCollisionRectangle]);
        if (!collidedWith)
            return null;

        const activePowerupType = this._activePowerup.powerUpType;
        this._activePowerup = null;
        return activePowerupType;
    }

    private onGrattacieloGeneratoHandler(grattacielo : Grattacielo) {

        if (Numbers.randomBetween(0, 1) < this._hamburgerProbability) {
            this.addHamburger(new Point(
                grattacielo.x + (grattacielo.width / 2),
                grattacielo.y - grattacielo.height - 70
            ));
        }

        if (Numbers.randomBetween(0, 1) < this._powerUpProbability) {
            this.addPowerUp(
                new Point(
                    grattacielo.x,
                    grattacielo.y - (grattacielo.height - 180)
                )
            );
        }

    }

}

export default GemmeManager;