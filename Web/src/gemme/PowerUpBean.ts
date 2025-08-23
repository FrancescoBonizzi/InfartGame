import PowerUp from "./PowerUp.ts";
import Camera from "../world/Camera.ts";
import {Point} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import BeanParticleSystem from "../particleEmitters/BeanParticleSystem.ts";

class PowerUpBean extends PowerUp {

    constructor(
        world: Camera,
        assets: InfartAssets,
        position: Point) {

        super(
            world,
            assets.textures.bean,
            position,
            new BeanParticleSystem(assets, world));
    }

    override getJumpForce(): number {
        return 300;
    }

    override getHorizontalMoveSpeedIncrease(): number {
        return 100;
    }

    override getFillColor(): string {
        return '#D2691E';
    }

    override getDurationMilliseconds(): number {
        return 3500;
    }

    override getMaxConsecutiveJumps(): number {
        return 1000;
    }
}

export default PowerUpBean;