import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {AnimatedSprite, Point} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import BroccoloParticleSystem from "../particleEmitters/BroccoloParticleSystem.ts";
import PowerUpTypes from "./PowerUpTypes.ts";
import SoundManager from "../services/SoundManager.ts";

class PowerUpBroccolo extends PowerUp {

    private readonly _playerAnimation: AnimatedSprite;

    constructor(
        world: Camera,
        assets: InfartAssets,
        soundManager: SoundManager,
        position: Point) {

        super(
            world,
            assets.textures.verdura,
            assets,
            position,
            soundManager,
            () => new BroccoloParticleSystem(assets, world));

        this._playerAnimation = assets.player.merda;
    }

    override getPopupText(): string {
        return "Propulsione clorofilliana!";
    }

    override getPowerUpType(): PowerUpTypes {
        return PowerUpTypes.Broccolo;
    }

    override getPlayerAnimation(): AnimatedSprite | null {
        return this._playerAnimation;
    }

    override getJumpForce(): number {
        return 500;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 400;
    }

    override getFillColor(): string {
        return '#a7ef17';
    }

    override getDurationMilliseconds(): number {
        return 4500;
    }

    override getMaxConsecutiveJumps(): number {
        return 1000;
    }

    override getParticleGenerationIntervalMilliseconds(): number {
        return 80;
    }
}

export default PowerUpBroccolo;