import {Point, Ticker} from "pixi.js";
import ExplosionParticleSystem from "./ExplosionParticleSystem.ts";
import InfartAssets from "../assets/InfartAssets.ts";
import Camera from "../world/Camera.ts";
import SoundManager from "../services/SoundManager.ts";

class InfartExplosion {

    private readonly _scrittaExplosion : ExplosionParticleSystem;
    private readonly _hamburgerExplosion : ExplosionParticleSystem;
    private readonly _merdaExplosion : ExplosionParticleSystem;
    private readonly _soundManager: SoundManager;

    constructor(
        assets: InfartAssets,
        camera: Camera,
        soundManager: SoundManager) {

        this._soundManager = soundManager;

        this._scrittaExplosion = new ExplosionParticleSystem(
            assets.textures.bang,
            camera
        );

        this._hamburgerExplosion = new ExplosionParticleSystem(
            assets.textures.burger,
            camera
        );

        this._merdaExplosion = new ExplosionParticleSystem(
            assets.textures.merda,
            camera
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