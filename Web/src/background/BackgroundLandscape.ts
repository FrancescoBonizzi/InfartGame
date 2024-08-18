import {Point, Ticker} from "pixi.js";
import InfartAssets from "../assets/InfartAssets.ts";
import GrattacieliAutogeneranti from "./GrattacieliAutogeneranti.ts";
import Camera from "../world/Camera.ts";
import BackgroundSky from "./BackgroundSky.ts";
import NuvoleAutogeneranti from "./NuvoleAutogeneranti.ts";
import StarField from "../particleEmitters/StarField.ts";
import Numbers from "../services/Numbers.ts";
import DynamicGameParameters from "../services/DynamicGameParameters.ts";

class BackgroundLandscape {

    private readonly _backgroundSky: BackgroundSky;
    private readonly _starfield: StarField;

    private readonly _grattacieliBack: GrattacieliAutogeneranti;
    private readonly _nuvolificioBack: NuvoleAutogeneranti;

    private readonly _grattacieliMid: GrattacieliAutogeneranti;
    private readonly _nuvolificioMid: NuvoleAutogeneranti;

    private readonly _starfieldSpawnRange = {
        min: -1200,
        max: -400
    };

    private readonly _camera: Camera;
    private readonly _dynamicGameParameters: DynamicGameParameters;

    constructor(
        camera: Camera,
        infartAssets: InfartAssets,
        dynamicGameParameters: DynamicGameParameters) {

        this._camera = camera;
        this._dynamicGameParameters = dynamicGameParameters;

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
            this._dynamicGameParameters,
            0.01);

        // TODO anche nuvole devono avere una velocità di parallasse?

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
            this._dynamicGameParameters,
            0.9);
    }

    update(time: Ticker) {

        this.evaluateStarsGeneration();

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

    private evaluateStarsGeneration() {

        if (!this.isCameraInStarfieldSpawnRange()) {
            return;
        }

        const where = new Point(
            Numbers.randomBetween(this._camera.x, this._camera.x + this._camera.width),
            Numbers.randomBetween(
                this._starfieldSpawnRange.min,
                this._starfieldSpawnRange.max),
        );
        this._starfield.addParticles(where);
    }
}

export default BackgroundLandscape;