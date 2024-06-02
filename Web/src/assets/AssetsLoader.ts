import {AnimatedSprite, Assets, Sprite, Spritesheet} from "pixi.js";
import InfartAssets from "./InfartAssets";
import {Sound, sound} from "@pixi/sound";

const _assetsRoot: string = '/assets';
const _imagesAssetsRoot: string = `${_assetsRoot}/images`;
const _spriteSheetsAssetsRoot: string = `${_imagesAssetsRoot}/spriteSheets`;
const _soundsAssetsRoot: string = `${_assetsRoot}/sounds`;

export const loadAssets = async (): Promise<InfartAssets> => {

    // Single image sprites
    const menuBackground = await loadMenuSprite("menuBackground");
    const gameTitle = await loadMenuSprite("gameTitle");
    const scoreBackground = await loadMenuSprite("scoreBackground");
    const gameOverBackground = await loadMenuSprite("gameOverBackground");

    // Sprite sheets
    const playerSpriteSheet = await loadSpriteSheet("player");
    const buildingsBackSpriteSheet = await loadSpriteSheet("buildingsBack");
    const buildingsMidSpriteSheet = await loadSpriteSheet("buildingsMid");
    const buildingsGroundSpriteSheet = await loadSpriteSheet("buildingsGround");

    return {
        sprites: {
            menu: {
                background: menuBackground,
                gameTitle: gameTitle,
                scoreBackground: scoreBackground,
                gameOverBackground: gameOverBackground
            },
            buildings: {
                back: loadAllSpritesFromSpriteSheet(buildingsBackSpriteSheet),
                mid: loadAllSpritesFromSpriteSheet(buildingsMidSpriteSheet),
                ground: loadAllSpritesFromSpriteSheet(buildingsGroundSpriteSheet)
            },
            bang: loadSpriteFromSpriteSheet(playerSpriteSheet, "bang"),
            broccoloParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "broccoloParticle"),
            bean: loadSpriteFromSpriteSheet(playerSpriteSheet, "bean"),
            burger: loadSpriteFromSpriteSheet(playerSpriteSheet, "burger"),
            gameOver: loadSpriteFromSpriteSheet(playerSpriteSheet, "gameOver"),
            jalapenoParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "jalapenoParticle"),
            jalapenos: loadSpriteFromSpriteSheet(playerSpriteSheet, "jalapenos"),
            merda: loadSpriteFromSpriteSheet(playerSpriteSheet, "merda"),
            pause: loadSpriteFromSpriteSheet(playerSpriteSheet, "pause"),
            play: loadSpriteFromSpriteSheet(playerSpriteSheet, "play"),
            record: loadSpriteFromSpriteSheet(playerSpriteSheet, "record"),
            scoreggiaParticle: loadSpriteFromSpriteSheet(playerSpriteSheet, "scoreggiaParticle"),
            stella: loadSpriteFromSpriteSheet(playerSpriteSheet, "stella"),
            verdura: loadSpriteFromSpriteSheet(playerSpriteSheet, "verdura"),
            background: loadSpriteFromSpriteSheet(playerSpriteSheet, "background"),
            deathScreen: loadSpriteFromSpriteSheet(playerSpriteSheet, "deathScreen"),
            nuvola1: loadSpriteFromSpriteSheet(playerSpriteSheet, "nuvola1"),
            nuvola2: loadSpriteFromSpriteSheet(playerSpriteSheet, "nuvola2"),
            nuvola3: loadSpriteFromSpriteSheet(playerSpriteSheet, "nuvola3"),
            player: {
                run: new AnimatedSprite(playerSpriteSheet.animations.playerRun),
                idle: new AnimatedSprite(playerSpriteSheet.animations.playerIdle),
                fart: new AnimatedSprite(playerSpriteSheet.animations.playerFart),
                fall: new AnimatedSprite(playerSpriteSheet.animations.playerFall),
                merda: new AnimatedSprite(playerSpriteSheet.animations.playerMerda),
            }
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


const loadSpriteFromFile = async (name: string): Promise<Sprite> => {
    const texture = await Assets.load(`${_imagesAssetsRoot}/${name}`);
    return new Sprite(texture);
}

const loadSpriteFromSpriteSheet = (spriteSheet: Spritesheet, name: string): Sprite => {
    const texture = spriteSheet.textures[`${name}.png`];
    return new Sprite(texture);
}

const loadMenuSprite = async (name: string): Promise<Sprite> => {
    return await loadSpriteFromFile(`menu/${name}.png`);
}

const loadSpriteSheet = async (name: string): Promise<Spritesheet> => {
    const spriteSheet = await Assets.load(`${_spriteSheetsAssetsRoot}/${name}.json`);
    return spriteSheet as Spritesheet;
}

const loadAllSpritesFromSpriteSheet = (spriteSheet: Spritesheet): Sprite[] => {
    return Object.keys(spriteSheet.textures).map((key) => {
        return new Sprite(spriteSheet.textures[key]);
    });
}

const loadSound = (folder: string, name: string): Sound => {
    return sound.add(name, `${_soundsAssetsRoot}/${folder}/${name}.mp3`);
}