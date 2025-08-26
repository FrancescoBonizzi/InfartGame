import {AnimatedSprite, Assets, Spritesheet, Texture} from "pixi.js";
import InfartAssets from "./InfartAssets";
import {Sound, sound} from "@pixi/sound";

/// TODO: MMM non sta funzionando la get del path relativo
//const VITE_BASE = import.meta.env.BASE_URL;
const _assetsRoot: string = `./assets`;
const _imagesAssetsRoot: string = `${_assetsRoot}/images`;
const _spriteSheetsAssetsRoot: string = `${_imagesAssetsRoot}/spriteSheets`;
const _soundsAssetsRoot: string = `${_assetsRoot}/sounds`;
const _fontsAssetsRoot: string = `${_assetsRoot}/fonts`;

export const loadAssets = async (): Promise<InfartAssets> => {

    // Sprite sheets
    const playerSpriteSheet = await loadSpriteSheet("player");
    const buildingsBackSpriteSheet = await loadSpriteSheet("buildingsBack");
    const buildingsMidSpriteSheet = await loadSpriteSheet("buildingsMid");
    const buildingsGroundSpriteSheet = await loadSpriteSheet("buildingsGround");
    const fontName = await loadFont();

    return {
        fontName: fontName,
        textures: {
            buildings: {
                back: loadGrattacieliFromSpriteSheet(buildingsBackSpriteSheet, "back"),
                mid: loadGrattacieliFromSpriteSheet(buildingsMidSpriteSheet, "mid"),
                ground: loadGrattacieliFromSpriteSheet(buildingsGroundSpriteSheet, "ground")
            },
            particles: {
                broccoloParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "broccoloParticle"),
                jalapenoParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "jalapenoParticle"),
                scoreggiaParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "scoreggiaParticle"),
                caccaParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "merda"),
                starParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "stella"),
            },
            bang: loadSpriteFromSpriteSheet(playerSpriteSheet, "bang"),

            bean: loadSpriteFromSpriteSheet(playerSpriteSheet, "bean"),
            burger: loadSpriteFromSpriteSheet(playerSpriteSheet, "burger"),
            gameOver: loadSpriteFromSpriteSheet(playerSpriteSheet, "gameOver"),
            jalapenos: loadSpriteFromSpriteSheet(playerSpriteSheet, "jalapenos"),
            merda: loadSpriteFromSpriteSheet(playerSpriteSheet, "merda"),
            pause: loadSpriteFromSpriteSheet(playerSpriteSheet, "pause"),
            play: loadSpriteFromSpriteSheet(playerSpriteSheet, "play"),
            record: loadSpriteFromSpriteSheet(playerSpriteSheet, "record"),
            verdura: loadSpriteFromSpriteSheet(playerSpriteSheet, "verdura"),
            background: loadSpriteFromSpriteSheet(playerSpriteSheet, "background"),
            deathScreen: loadSpriteFromSpriteSheet(playerSpriteSheet, "death_screen"),
            nuvola1: loadSpriteFromSpriteSheet(playerSpriteSheet, "nuvola1"),
            nuvola2: loadSpriteFromSpriteSheet(playerSpriteSheet, "nuvola2"),
            nuvola3: loadSpriteFromSpriteSheet(playerSpriteSheet, "nuvola3")
        },
        player: {
            run: new AnimatedSprite(playerSpriteSheet.animations.playerRun!),
            idle: new AnimatedSprite(playerSpriteSheet.animations.playerIdle!),
            fart: new AnimatedSprite(playerSpriteSheet.animations.playerFart!),
            fall: new AnimatedSprite(playerSpriteSheet.animations.playerFall!),
            merda: new AnimatedSprite(playerSpriteSheet.animations.playerMerda!),
        },
        sounds: {
            music: {
                game: loadSound("music", "game"),
                menu: loadSound("music", "menu")
            },
            effects: {
                bite: loadSound("effects", "bite"),
                explosion: loadSound("effects", "explosion"),
                fall: loadSound("effects", "fall"),
                heartBeat: loadSound("effects", "heartBeat"),
                jalapenos: loadSound("effects", "jalapenos"),
                thunder: loadSound("effects", "thunder"),
                truck: loadSound("effects", "truck"),
            },
            farts: [
                loadSound("farts", "fart1"),
                loadSound("farts", "fart2"),
                loadSound("farts", "fart3"),
                loadSound("farts", "fart4"),
                loadSound("farts", "fart5"),
                loadSound("farts", "fart6"),
                loadSound("farts", "fart7")
            ]
        }
    }
};

const loadFont = async () => {

    const bundleName = 'fonts';
    const fontName = 'Patrick Hand SC';

    Assets.addBundle(bundleName, [
        { alias: fontName, src: `${_fontsAssetsRoot}/PatrickHandSC-Regular.ttf` }
    ]);

    await Assets.loadBundle(bundleName);
    return fontName;
}

const loadSpriteFromSpriteSheet = (spriteSheet: Spritesheet, name: string): Texture => {
    return spriteSheet.textures[`${name}`]!;
}


const loadSpriteSheet = async (name: string): Promise<Spritesheet> => {
    const spriteSheet = await Assets.load(`${_spriteSheetsAssetsRoot}/${name}.json`);
    return spriteSheet as Spritesheet;
}

const loadGrattacieliFromSpriteSheet = (
    spriteSheet: Spritesheet,
    keyName: string): Texture[] => {

    const count = Object.keys(spriteSheet.textures)
        .filter(k => k.startsWith(keyName))
        .length;
    const textures : Texture[] = [];
    for (let i = 0; i < count; i++) {
        textures.push(spriteSheet.textures[`${keyName}${i}`]!);
    }

    return textures;
}

const loadSound = (folder: string, name: string): Sound => {
    return sound.add(name, `${_soundsAssetsRoot}/${folder}/${name}.mp3`);
}