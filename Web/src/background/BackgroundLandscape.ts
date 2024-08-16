import {Point, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import Camera from "../world/Camera.ts";
import BackgroundSky from "./BackgroundSky.ts";
import NuvoleAutogeneranti from "./NuvoleAutogeneranti.ts";
import StarField from "../particleEmitters/StarField.ts";
import Numbers from "../services/Numbers.ts";

class BackgroundLandscape {

    private readonly _backgroundSky: BackgroundSky;
    private readonly _starfield: StarField;

    private readonly _grattacieliBack: GrattacieliAutogeneranti;
    private readonly _nuvolificioBack: NuvoleAutogeneranti;

    private readonly _grattacieliMid: GrattacieliAutogeneranti;
    private readonly _nuvolificioMid: NuvoleAutogeneranti;

    private readonly _starfieldSpawnRange  = {
        min: -1200,
        max: -400
    };
    private readonly _camera: Camera;
    private _timeTillNewStar = 0;
    private readonly _timeBetweenNewStar = 20;

    constructor(
        camera: Camera,
        infartAssets: InfartAssets) {

        this._camera = camera;

        this._backgroundSky = new BackgroundSky(
            infartAssets,
            camera);

        this._starfield = new StarField(
            infartAssets,
            camera);

        this._nuvolificioBack = new NuvoleAutogeneranti(
            camera,
            infartAssets,
            0.2,
            "#051728",
            {
                min: 2 / 100,
                max: 5 / 100
            },
            false,
            8);
        this._grattacieliBack = new GrattacieliAutogeneranti(
            camera,
            infartAssets.textures.buildings.back,
            0.01);

        this._nuvolificioMid = new NuvoleAutogeneranti(
            camera,
            infartAssets,
            0.4,
            "#093243",
            {
                min: 12 / 100,
                max: 15 / 100
            },
            false,
            8);
        this._grattacieliMid = new GrattacieliAutogeneranti(
            camera,
            infartAssets.textures.buildings.mid,
            0.18);
    }

    update(time: Ticker) {

        this.evaluateStarsGeneration(time);

        this._starfield.update(time);
        this._backgroundSky.update();
        this._grattacieliBack.update(time);
        this._grattacieliMid.update(time);
        this._nuvolificioBack.update(time);
        this._nuvolificioMid.update(time);
    }

    private isCameraInStarfieldSpawnRange() {
        return this._camera.y >= this._starfieldSpawnRange.min
            && this._camera.y <= this._starfieldSpawnRange.max;
    }

    private evaluateStarsGeneration(time: Ticker) {

        if (!this.isCameraInStarfieldSpawnRange()) {
            return;
        }

        this._timeTillNewStar -= time.deltaTime;
        if (this._timeTillNewStar < 0) {
            const where = new Point(
                Numbers.randomBetween(this._camera.x, this._camera.x + this._camera.width),
                Numbers.randomBetween(
                    this._starfieldSpawnRange.min,
                    this._starfieldSpawnRange.max),
            );
            this._starfield.addParticles(where);
            this._timeTillNewStar = this._timeBetweenNewStar;
        }

    }
}

export default BackgroundLandscape;