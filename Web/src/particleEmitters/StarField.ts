import ParticleSystem from "./ParticleSystem.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";
import {Point, Ticker} from "pixi.js";
import Numbers from "../services/Numbers.ts";

class StarField extends ParticleSystem {

    private readonly _camera: Camera;
    private readonly _timeBetweenNewStarGenerationMs = 20.0;
    private _timeTillNewStarMs;

    private readonly _spawnYRange = {
        min: -1200,
        max: -300
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
            'add',
            true
        );

        this._timeTillNewStarMs = this._timeBetweenNewStarGenerationMs;
        this._camera = camera;
    }

    private isCameraInStarfieldSpawnRange() {
        return this._camera.y >= this._spawnYRange.min
            && this._camera.y <= this._spawnYRange.max;
    }

    private evaluateStarsGeneration(ticker: Ticker) {

        if (!this.isCameraInStarfieldSpawnRange()) {
            return;
        }

        this._timeTillNewStarMs -= ticker.elapsedMS;
        if (this._timeTillNewStarMs < 0) {

            const x = Numbers.randomBetweenInterval({
                min: this._camera.x,
                max: this._camera.x + this._camera.width
            });
            const y = Numbers.randomBetweenInterval(this._spawnYRange);

            this.addParticles(new Point(x, y));
            this._timeTillNewStarMs = this._timeBetweenNewStarGenerationMs;
        }

    }

    override update(time: Ticker) {
        super.update(time);
        this.evaluateStarsGeneration(time);
    }
}

export default StarField;