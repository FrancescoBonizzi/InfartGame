import { AnimatedSprite, Assets, Sprite, Spritesheet } from "pixi.js";
import InfartAssets from "./InfartAssets";

export const loadAssets = async () : Promise<InfartAssets> => {

  // Menu
  const texture = await Assets.load(
    "/assets/images/menuBackground.png"
  );
  const menuBackground = new Sprite(texture);


 // forse conviene fare un mega json
 const spritesheetConfiguration = {
    frames: {
      run__000: {
        frame: { x: 1770, y: 616, w: 79, h: 94 },
        sourceSize: { w: 79, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 79, h: 94 },
      },
      run__001: {
        frame: { x: 1831, y: 722, w: 77, h: 94 },
        sourceSize: { w: 77, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 77, h: 94 },
      },
      run__002: {
        frame: { x: 1931, y: 702, w: 77, h: 94 },
        sourceSize: { w: 77, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 77, h: 94 },
      },
      run__003: {
        frame: { x: 1487, y: 922, w: 77, h: 94 },
        sourceSize: { w: 77, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 77, h: 94 },
      },
      run__004: {
        frame: { x: 1326, y: 833, w: 79, h: 94 },
        sourceSize: { w: 79, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 79, h: 94 },
      },
      run__005: {
        frame: { x: 1505, y: 616, w: 81, h: 92 },
        sourceSize: { w: 81, h: 92 },
        spriteSourceSize: { x: 0, y: 0, w: 81, h: 92 },
      },
      run__006: {
        frame: { x: 1687, y: 608, w: 81, h: 96 },
        sourceSize: { w: 81, h: 96 },
        spriteSourceSize: { x: 0, y: 0, w: 81, h: 96 },
      },
      run__007: {
        frame: { x: 1416, y: 812, w: 79, h: 96 },
        sourceSize: { w: 79, h: 96 },
        spriteSourceSize: { x: 0, y: 0, w: 79, h: 96 },
      },
      run__008: {
        frame: { x: 1256, y: 529, w: 77, h: 96 },
        sourceSize: { w: 77, h: 96 },
        spriteSourceSize: { x: 0, y: 0, w: 77, h: 96 },
      },
      run__009: {
        frame: { x: 1910, y: 798, w: 73, h: 94 },
        sourceSize: { w: 73, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 73, h: 94 },
      },
      run__010: {
        frame: { x: 1002, y: 651, w: 71, h: 92 },
        sourceSize: { w: 71, h: 92 },
        spriteSourceSize: { x: 0, y: 0, w: 71, h: 92 },
      },
      run__011: {
        frame: { x: 1971, y: 416, w: 75, h: 92 },
        sourceSize: { w: 75, h: 92 },
        spriteSourceSize: { x: 0, y: 0, w: 75, h: 92 },
      },
      run__012: {
        frame: { x: 1971, y: 510, w: 73, h: 94 },
        sourceSize: { w: 73, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 73, h: 94 },
      },
      run__013: {
        frame: { x: 1577, y: 874, w: 77, h: 94 },
        sourceSize: { w: 77, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 77, h: 94 },
      },
      run__014: {
        frame: { x: 1245, y: 819, w: 79, h: 94 },
        sourceSize: { w: 79, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 79, h: 94 },
      },
      run__015: {
        frame: { x: 1162, y: 731, w: 81, h: 92 },
        sourceSize: { w: 81, h: 92 },
        spriteSourceSize: { x: 0, y: 0, w: 81, h: 92 },
      },
      run__016: {
        frame: { x: 1335, y: 639, w: 81, h: 94 },
        sourceSize: { w: 81, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 81, h: 94 },
      },
      run__017: {
        frame: { x: 1335, y: 735, w: 79, h: 96 },
        sourceSize: { w: 79, h: 96 },
        spriteSourceSize: { x: 0, y: 0, w: 79, h: 96 },
      },
      run__018: {
        frame: { x: 1418, y: 714, w: 79, h: 96 },
        sourceSize: { w: 79, h: 96 },
        spriteSourceSize: { x: 0, y: 0, w: 79, h: 96 },
      },
      run__019: {
        frame: { x: 1256, y: 627, w: 77, h: 94 },
        sourceSize: { w: 77, h: 94 },
        spriteSourceSize: { x: 0, y: 0, w: 77, h: 94 },
      },
    },
    meta: {
      image: "/assets/images/textures.png",
      format: "RGBA8888",
      size: { w: 2048, h: 2048 },
      scale: 1,
    },
    animations: {
      playerRun: [
        "run__000",
        "run__001",
        "run__002",
        "run__003",
        "run__004",
        "run__005",
        "run__006",
        "run__007",
        "run__008",
        "run__009",
        "run__010",
        "run__011",
        "run__012",
        "run__013",
        "run__014",
        "run__015",
        "run__016",
        "run__017",
        "run__018",
        "run__019",
      ],
    },
  };

  const spritesheet = new Spritesheet(
    await Assets.load(spritesheetConfiguration.meta.image),
    spritesheetConfiguration
  );

  // Generate all the Textures asynchronously
  await spritesheet.parse();

  // spritesheet is ready to use!
  const anim = new AnimatedSprite(spritesheet.animations.playerRun
  );
  anim.animationSpeed = 0.4;
  anim.play();

  return {
        menu: {
            background: menuBackground,
        },
        player: {
            run: anim,
        }
  }
};
