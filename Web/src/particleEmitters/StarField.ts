import ParticleSystem from "./ParticleSystem.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";
import {Point, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";

class StarField extends ParticleSystem {

    private readonly _camera: Camera;

    private readonly _spawnYRange = {
        min: -1200,
        max: -400
    };

    constructor(
        infartAssets: InfartAssets,
        camera: Camera) {

        super(
            infartAssets.textures.particles.starParticle,
            camera,
            8,
            {min: 4, max: 8},
            {min: 10, max: 15},
            {min: 30, max: 40},
            {min: -Math.PI / 2, max: Math.PI / 2},
            {min: 1.5, max: 3.0},
            {min: 0.2, max: 1.0},
            {min: 0, max: 360},
            20
        );

        this._camera = camera;
    }

    private isCameraInStarfieldSpawnRange() {
        return this._camera.y >= this._spawnYRange.min
            && this._camera.y <= this._spawnYRange.max;
    }

    private evaluateStarsGeneration() {

        if (!this.isCameraInStarfieldSpawnRange()) {
            return;
        }

        const rightEdge = this._camera.x + this._camera.width;
        const x = rightEdge + Numbers.randomBetween(0, this._camera.width);
        const y = Numbers.randomBetween(this._spawnYRange.min, this._spawnYRange.max);

        this.addParticles(new Point(x, y));
    }

    override update(time: Ticker) {
        super.update(time);
        this.evaluateStarsGeneration();
    }
}

export default StarField;