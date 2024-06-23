import {AnimatedSprite, Point, Rectangle, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import PlayerAnimations from "./PlayerAnimations.ts";
import CollisionSolver from "../services/CollisionSolver.ts";
import Numbers from "../services/Numbers.ts";
import Camera from "../world/Camera.ts";
import Foreground from "../background/Foreground.ts";

class Player {

    private _position: Point;
    private _speed: Point;
    private readonly _fallSpeed = 20;
    // private _overlayColor: ColorSource;
    private _horizontalSpeed = 300;

    private _currentAnimation: AnimatedSprite;
    private _animations: PlayerAnimations;
    private _isOnGround: boolean = false;
    private _walkArea: Foreground;

    constructor(
        staringPosition: Point,
        assets: InfartAssets,
        camera: Camera,
        walkArea: Foreground) {

        this._animations = assets.player;
        this._position = staringPosition;
        this._speed = new Point(
            this._horizontalSpeed,
            this._fallSpeed);
        //    this._overlayColor = '#ffffff';

        this._currentAnimation = this._animations.fall;
        this._currentAnimation.play();
        this._walkArea = walkArea;

        camera.addToWorld(this._currentAnimation);
    }

    jump(amount: number) {
        this._speed.y = -amount;
    }

    get isOnGround() {
        return this._isOnGround;
    }

    set isOnGround(value: boolean) {
        this._isOnGround = value;
    }

    get isDead() {
        return false;
    }

    isFalling() {
        return this._currentAnimation === this._animations.fall;
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

            const collidingObject = CollisionSolver.checkCollisionsReturnCollidingObject(
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

    private get collisionRectangle() {
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
    }


}

export default Player;