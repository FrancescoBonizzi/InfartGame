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
import Hud from "../hud/Hud.ts";
import PowerUp from "../gemme/PowerUp.ts";
import {deallocateEmptyParticleSystems} from "../particleEmitters/ParticleSystem.ts";

class Player implements IHasCollisionRectangle {

    private _position: Point;
    private _speed: Point;
    private _overlayColor: ColorSource;

    private _currentAnimation: AnimatedSprite;
    private _animations: PlayerAnimations;
    private _isOnGround: boolean = false;
    private _walkArea: Foreground;
    private _scoreggeParticleSystems: ScoreggiaParticleSystem[];

    private readonly _defaultOverlayColor = '#ffffff';
    private readonly _fallSpeed = 20;
    private readonly _camera: Camera;
    private readonly _soundManager: SoundManager;
    private readonly _dynamicGameParameters: DynamicGameParameters;
    private readonly _assets: InfartAssets;
    private readonly _hud: Hud;
    private readonly _defaultJumpAmount = 550;
    private readonly _defaultMaxConsecutiveJumps = 2;

    private _currentJumpCount: number = 0;
    private _isDead: boolean = false;
    private _infartExplosion: InfartExplosion;

    private _activePowerUp: PowerUp | null = null;
    private _totalFarts : number = 0;

    constructor(
        staringPosition: Point,
        assets: InfartAssets,
        camera: Camera,
        walkArea: Foreground,
        soundManager: SoundManager,
        dynamicGameParameters: DynamicGameParameters,
        infartExplosion: InfartExplosion,
        hud: Hud) {

        this._dynamicGameParameters = dynamicGameParameters;
        this._camera = camera;
        this._animations = assets.player;
        this._assets = assets;
        this._position = staringPosition;
        this._speed = new Point(
            this._dynamicGameParameters.playerHorizontalSpeed,
            this._fallSpeed);
        this._overlayColor = this._defaultOverlayColor;
        this._soundManager = soundManager;

        this._currentAnimation = this._animations.fall;
        this.setNewAnimation(this._animations.fall);
        this._walkArea = walkArea;
        this._infartExplosion = infartExplosion;
        this._scoreggeParticleSystems = [];
        this._hud = hud;
    }

    public get totalFarts() {
        return this._totalFarts;
    }

    jump(force: number = this._defaultJumpAmount) {

        if (this._isDead)
            return;

        const maxConsecutiveJump = this._activePowerUp
            ? this._activePowerUp.getMaxConsecutiveJumps()
            : this._defaultMaxConsecutiveJumps;
        if (this._currentJumpCount >= maxConsecutiveJump - 1) {
            return;
        }

        this._hud.getHamburgerStatusBar().farted();

        const jumpForce = this._activePowerUp
            ? this._activePowerUp.getJumpForce()
            : force;

        this._currentJumpCount++;
        this._totalFarts++;

        this._speed.y = -jumpForce;
        this._soundManager.playFart();
        this._scoreggeParticleSystems.push(new ScoreggiaParticleSystem(
            this._assets,
            this._camera
        ));
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

    activatePowerUp(powerUp: PowerUp) {
        this._activePowerUp = powerUp;

        this._currentJumpCount = 0;
        this._hud.getHamburgerStatusBar().activatePowerUp(powerUp);

        this.jump(powerUp.getJumpForce());

        this._dynamicGameParameters.playerHorizontalSpeed += powerUp.getHorizontalMoveSpeedIncrease();
        this._overlayColor = powerUp.getFillColor();
    }

    deactivatePowerUp() {
        this._dynamicGameParameters.playerHorizontalSpeed -= this._activePowerUp!.getHorizontalMoveSpeedIncrease();
        this._overlayColor = this._defaultOverlayColor;
        this._activePowerUp = null;
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

    get activePowerUp() {
        return this._activePowerUp;
    }

    update(time: Ticker) {
        this._speed.x = this._dynamicGameParameters.playerHorizontalSpeed;
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

        if (this._activePowerUp) {
            if (this._activePowerUp.getPlayerAnimation()) {
                newAnimation = this._activePowerUp.getPlayerAnimation()!;
            }
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

        if (this._position.y > 0) {
            // TODO: Suono fall
            this.die(false);
        }

        this._position.x += moveAmount.x;
        this._position.y += moveAmount.y;

        this._currentAnimation.x = this._position.x;
        this._currentAnimation.y = this._position.y;

        this._scoreggeParticleSystems.forEach((p) => p.update(time));
        this.evaluateScoreggiaGeneration();
        this._scoreggeParticleSystems = deallocateEmptyParticleSystems(this._scoreggeParticleSystems);
    }

    private evaluateScoreggiaGeneration() {
        if (this._speed.y >= 0) {
            this._soundManager.stopFart();
        } else if (this._speed.y < 0) {

            if (this._scoreggeParticleSystems.length > 0 || this._activePowerUp) {
                const where = new Point(
                    this._position.x + this._currentAnimation.width / 3,
                    this._position.y + this._currentAnimation.height / 2 + 30
                );

                if (!this._activePowerUp) {
                    const currentJumpParticleSystem = this._scoreggeParticleSystems[this._scoreggeParticleSystems.length - 1];
                    currentJumpParticleSystem?.addParticles(where);
                } else {
                    this._activePowerUp.addParticles(where);
                }
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
        this._hud.getHamburgerStatusBar().playerDead();
        this._hud.playerDead();
    }

    hamburgerEaten() {
        if (this._hud.getHamburgerStatusBar().getCurrentEatenHamburgers() > FixedGameParamters.MaxEatenHamburgers) {
            this.die(true);
            return;
        }

        this._soundManager.playBite();
        this._hud.getHamburgerStatusBar().hamburgerEaten();
    }
}

export default Player;