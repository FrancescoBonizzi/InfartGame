import {AnimatedSprite, Assets, Spritesheet, Texture} from "pixi.js";
import InfartAssets from "./InfartAssets";
import {Sound, sound} from "@pixi/sound";

const _assetsRoot: string = '/assets';
const _imagesAssetsRoot: string = `${_assetsRoot}/images`;
const _spriteSheetsAssetsRoot: string = `${_imagesAssetsRoot}/spriteSheets`;
const _soundsAssetsRoot: string = `${_assetsRoot}/sounds`;

export const loadAssets = async (): Promise<InfartAssets> => {

    // Single image sprites
    const menuBackground = await loadMenuTexture("menuBackground");
    const gameTitle = await loadMenuTexture("gameTitle");
    const scoreBackground = await loadMenuTexture("scoreBackground");
    const gameOverBackground = await loadMenuTexture("gameOverBackground");

    // Sprite sheets
    const playerSpriteSheet = await loadSpriteSheet("player");
    const buildingsBackSpriteSheet = await loadSpriteSheet("buildingsBack");
    const buildingsMidSpriteSheet = await loadSpriteSheet("buildingsMid");
    const buildingsGroundSpriteSheet = await loadSpriteSheet("buildingsGround");

    return {
        textures: {
            menu: {
                background: menuBackground,
                gameTitle: gameTitle,
                scoreBackground: scoreBackground,
                gameOverBackground: gameOverBackground
            },
            buildings: {
                back: loadGrattacieliFromSpriteSheet(buildingsBackSpriteSheet, "back"),
                mid: loadGrattacieliFromSpriteSheet(buildingsMidSpriteSheet, "mid"),
                ground: loadGrattacieliFromSpriteSheet(buildingsGroundSpriteSheet, "ground")
            },
            particles: {
                broccoloParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "broccoloParticle"),
                jalapenoParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "jalapenoParticle"),
                scoreggiaParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "scoreggiaParticle"),

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
            deathScreen: loadSpriteFromSpriteSheet(playerSpriteSheet, "deathScreen"),
            nuvola1: loadSpriteFromSpriteSheet(playerSpriteSheet, "nuvola1"),
            nuvola2: loadSpriteFromSpriteSheet(playerSpriteSheet, "nuvola2"),
            nuvola3: loadSpriteFromSpriteSheet(playerSpriteSheet, "nuvola3")
        },
        player: {
            run: new AnimatedSprite(playerSpriteSheet.animations.playerRun),
            idle: new AnimatedSprite(playerSpriteSheet.animations.playerIdle),
            fart: new AnimatedSprite(playerSpriteSheet.animations.playerFart),
            fall: new AnimatedSprite(playerSpriteSheet.animations.playerFall),
            merda: new AnimatedSprite(playerSpriteSheet.animations.playerMerda),
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

const loadTextureFromFile = async (name: string): Promise<Texture> => {
    return await Assets.load(`${_imagesAssetsRoot}/${name}`);
}

const loadSpriteFromSpriteSheet = (spriteSheet: Spritesheet, name: string): Texture => {
    return spriteSheet.textures[`${name}`];
}

const loadMenuTexture = async (name: string): Promise<Texture> => {
    return await loadTextureFromFile(`menu/${name}.png`);
}

const loadSpriteSheet = async (name: string): Promise<Spritesheet> => {
    const spriteSheet = await Assets.load(`${_spriteSheetsAssetsRoot}/${name}.json`);
    return spriteSheet as Spritesheet;
}

const loadGrattacieliFromSpriteSheet = (
    spriteSheet: Spritesheet,
    keyName: string): Texture[] => {

    const textures : Texture[] = [];
    for (let i = 0; i < 69; i++) {
        textures.push(spriteSheet.textures[`${keyName}${i}`]);
    }

    return textures;
}

const loadSound = (folder: string, name: string): Sound => {
    return sound.add(name, `${_soundsAssetsRoot}/${folder}/${name}.mp3`);
}