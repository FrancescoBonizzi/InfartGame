import {AnimatedSprite, ColorSource, Point, Rectangle, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import PlayerAnimations from "./PlayerAnimations.ts";
import CollisionSolver from "../services/CollisionSolver.ts";
import Numbers from "../services/Numbers.ts";
import Camera from "../world/Camera.ts";
import Foreground from "../background/Foreground.ts";
import ScoreggiaParticleSystem from "../particleEmitters/ScoreggiaParticleSystem.ts";
import SoundManager from "../services/SoundManager.ts";
import DynamicGameParameters from "../services/DynamicGameParameters.ts";
import IHasCollisionRectangle from "../IHasCollisionRectangle.ts";
import InfartExplosion from "../particleEmitters/InfartExplosion.ts";
import FixedGameParamters from "../services/FixedGameParameters.ts";
import HamburgerStatusBar from "../hud/HamburgerStatusBar.ts";

class Player implements IHasCollisionRectangle {

    private _position: Point;
    private _speed: Point;
    private readonly _fallSpeed = 20;
    private readonly _overlayColor: ColorSource;

    private _currentAnimation: AnimatedSprite;
    private _animations: PlayerAnimations;
    private _isOnGround: boolean = false;
    private _walkArea: Foreground;
    private _scoreggeParticleSystems: ScoreggiaParticleSystem[];
    private readonly _camera: Camera;
    private readonly _soundManager: SoundManager;
    private readonly _dynamicGameParameters: DynamicGameParameters;

    private _currentJumpCount: number = 0;
    private _currentEatenHambugers: number = 0;
    private _isDead: boolean = false;
    private _infartExplosion: InfartExplosion;
    private _hamburgerStatusBar: HamburgerStatusBar;
    private _assets: InfartAssets;

    constructor(
        staringPosition: Point,
        assets: InfartAssets,
        camera: Camera,
        walkArea: Foreground,
        soundManager: SoundManager,
        dynamicGameParameters: DynamicGameParameters,
        infartExplosion: InfartExplosion,
        hamburgerStatusBar: HamburgerStatusBar) {

        this._dynamicGameParameters = dynamicGameParameters;
        this._camera = camera;
        this._animations = assets.player;
        this._assets = assets;
        this._position = staringPosition;
        this._speed = new Point(
            this._dynamicGameParameters.playerHorizontalSpeed,
            this._fallSpeed);
        this._overlayColor = '#ffffff';
        this._soundManager = soundManager;

        this._currentAnimation = this._animations.fall;
        this.setNewAnimation(this._animations.fall);
        this._walkArea = walkArea;
        this._infartExplosion = infartExplosion;
        this._hamburgerStatusBar = hamburgerStatusBar;
        this._scoreggeParticleSystems = [];
    }

    jump() {

        if (this._isDead)
            return;

        if (this._currentEatenHambugers > 0) {
            this._currentEatenHambugers--;
            this._hamburgerStatusBar.farted();
        }

        // TODO: ci sarà lo switch sulla base del powerup corrente
        if (this._currentJumpCount >= 1) {
            return;
        }

        const amount = 650;
        this._currentJumpCount++;

        this._speed.y = -amount;
        this._soundManager.playFart();
        this._scoreggeParticleSystems.push(new ScoreggiaParticleSystem(
            this._assets,
            this._camera
        ))
    }

    get isOnGround() {
        return this._isOnGround;
    }

    set isOnGround(value: boolean) {
        this._isOnGround = value;
    }

    get isDead() {
        return this._isDead;
    }

    private evaluatePotentialCollisions(moveAmount: Point) {

        const possibleCollidingObjects = this._walkArea.drawnGrattacieli
            .map(grattacielo => grattacielo.collisionRectangle);

        let afterMove = this.collisionRectangle;
        let corner1: Point;
        let corner2: Point;

        if (moveAmount.x !== 0) {
            // ⚠️ C'era il metodo .OFFSET in XNA
            afterMove.x += moveAmount.x;
            corner1 = new Point(
                afterMove.x + afterMove.width,
                afterMove.y);
            corner2 = new Point(
                afterMove.x + afterMove.width,
                afterMove.y + afterMove.height - 80);

            if (CollisionSolver.checkCollisions(
                new Rectangle(
                    corner1.x,
                    corner1.y,
                    1,
                    Math.abs(corner2.y - corner1.y)),
                possibleCollidingObjects
            )
            ) {
                moveAmount.x = 0;
                this._speed.x = 0;
            }
        }

        if (moveAmount.y === 0) {
            return moveAmount;
        }

        afterMove = this.collisionRectangle;
        afterMove.y += moveAmount.y;

        if (this._speed.y > 0) {
            corner1 = new Point(
                afterMove.x + 20,
                afterMove.y + afterMove.height - 2);
            corner2 = new Point(
                afterMove.x + afterMove.width - 20,
                afterMove.y + afterMove.height - 2);

            const collidingObject = CollisionSolver.checkCollisionsReturnCollidingRectangle(
                new Rectangle(
                    corner1.x,
                    corner1.y + 10,
                    Math.abs(corner2.x - corner1.x),
                    1),
                possibleCollidingObjects
            );

            if (collidingObject) {

                this.isOnGround = true;

                moveAmount.y = 0;
                this._speed.y = 0;

                this._position = new Point(
                    this._position.x,
                    Numbers.lerp(
                        this._position.y,
                        collidingObject.y - this._currentAnimation.height + 2,
                        0.1)
                );
            }
        }

        return moveAmount;
    }

    get collisionRectangle() {
        return new Rectangle(
            this._position.x,
            this._position.y,
            this._currentAnimation.width,
            this._currentAnimation.height
        );
    }

    update(time: Ticker) {
        this._speed.y += this._fallSpeed;

        if (this.isDead) {
            return;
        }

        let newAnimation = this._animations.run;

        if (!this.isOnGround) {
            if (this._speed.y < 0) {
                newAnimation = this._animations.fart;
            } else if (this._currentAnimation === this._animations.fart
                || this._currentAnimation === this._animations.fall) {
                newAnimation = this._animations.fall;
            }
        } else {
            this._currentJumpCount = 0;
        }

        if (newAnimation !== this._currentAnimation) {
            this.setNewAnimation(newAnimation);
        }

        const elaspedSeconds = time.elapsedMS / 1000;
        let moveAmount = new Point(
            this._speed.x * elaspedSeconds,
            this._speed.y * elaspedSeconds);

        moveAmount = this.evaluatePotentialCollisions(moveAmount);

        if (this._speed.y < 0) {
            this.isOnGround = false;
        }

        this._position.x += moveAmount.x;
        this._position.y += moveAmount.y;

        this._currentAnimation.x = this._position.x;
        this._currentAnimation.y = this._position.y;

        this._scoreggeParticleSystems.forEach((p) => p.update(time));
        this.evaluateScoreggiaGeneration();
        this._scoreggeParticleSystems = this._scoreggeParticleSystems.filter(p => p.isActive());
    }

    private evaluateScoreggiaGeneration() {
        if (this._speed.y >= 0) {
            this._soundManager.stopFart();
        } else if (this._speed.y < 0) {
            if (this._scoreggeParticleSystems.length > 0) {
                const currentJumpParticleSystem = this._scoreggeParticleSystems[this._scoreggeParticleSystems.length - 1];
                const where = new Point(
                    this._position.x + this._currentAnimation.width / 3,
                    this._position.y + this._currentAnimation.height / 2 + 30
                );
                currentJumpParticleSystem.addParticles(where);
            }
        }
    }

    private setNewAnimation(newAnimation: AnimatedSprite) {
        this._camera.removeFromWorld(this._currentAnimation);

        this._currentAnimation.stop();
        this._currentAnimation = newAnimation;
        this._currentAnimation.play();
        this._currentAnimation.tint = this._overlayColor;
        this._camera.addToWorld(this._currentAnimation);
        this._currentAnimation.x = this._position.x;
        this._currentAnimation.y = this._position.y;
    }

    get position() {
        return this._position;
    }

    die(isWithText: boolean) {
        this._isDead = true;
        this._infartExplosion.explode(this._position, isWithText);
        this._camera.removeFromWorld(this._currentAnimation);
    }

    hamburgerEaten() {
        if (this._currentEatenHambugers > FixedGameParamters.MaxEatenHamburgers) {
            this.die(true);
            return;
        }

        this._soundManager.playBite();
        ++this._currentEatenHambugers;
        this._hamburgerStatusBar.hamburgerEaten();
    }
}

export default Player;