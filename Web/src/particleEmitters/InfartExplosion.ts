import {Point, Ticker} from "pixi.js";
import ExplosionParticleSystem from "./ExplosionParticleSystem.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";
import SoundManager from "../services/SoundManager.ts";

class InfartExplosion {

    private readonly _scrittaExplosion: ExplosionParticleSystem;
    private readonly _hamburgerExplosion: ExplosionParticleSystem;
    private readonly _merdaExplosion: ExplosionParticleSystem;
    private readonly _soundManager: SoundManager;

    constructor(
        assets: InfartAssets,
        camera: Camera,
        soundManager: SoundManager) {

        this._soundManager = soundManager;

        this._scrittaExplosion = new ExplosionParticleSystem(
            assets.textures.bang,
            camera,
            {min: 0, max: 0},
            {min: 1, max: 1},
            1,
            {min: 1.5, max: 2.0},
            { min: 0, max: 0 },
            false
        );

        this._hamburgerExplosion = new ExplosionParticleSystem(
            assets.textures.burger,
            camera,
            {min: 1, max: 5},
            {min: 12, max: 24},
            1,
            {min: 0.5, max: 1.0},
            { min: 20, max: 60 },
            true
        );

        this._merdaExplosion = new ExplosionParticleSystem(
            assets.textures.merda,
            camera,
            {min: 1, max: 5},
            {min: 12, max: 24},
            1,
            {min: 0.5, max: 1.0},
            { min: 20, max: 60 },
            true
        );
    }

    explode(where: Point, isWithText: boolean) {

        this._soundManager.playExplosion();

        if (isWithText) {
            this._scrittaExplosion.addParticles(where);
        }

        this._hamburgerExplosion.addParticles(where);
        this._merdaExplosion.addParticles(where);
    }

    update(time: Ticker) {
        this._scrittaExplosion.update(time);
        this._hamburgerExplosion.update(time);
        this._merdaExplosion.update(time);
    }

}

export default InfartExplosion;