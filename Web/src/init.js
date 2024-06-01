(async () => {
  // Create a PixiJS application.
  const app = new PIXI.Application();

  // Intialize the application.
  await app.init({
    background: "#1099bb",
    width: 800,
    height: 480,
  });

  // Then adding the application's canvas to the DOM body.
  document.body.appendChild(app.canvas);

  // Menu
  const texture = await PIXI.Assets.load(
    "src/assets/images/menuBackground.png"
  );
  const sprite1 = new PIXI.Sprite(texture);

  sprite1.x = 0;
  sprite1.y = 0;
  app.stage.addChild(sprite1);

  // Spritesheet

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
      image: "src/assets/images/textures.png",
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

  const spritesheet = new PIXI.Spritesheet(
    await PIXI.Assets.load(spritesheetConfiguration.meta.image),
    spritesheetConfiguration
  );

  // Generate all the Textures asynchronously
  await spritesheet.parse();

  // spritesheet is ready to use!
  const anim = new PIXI.AnimatedSprite(spritesheet.animations.playerRun);

  anim.anchor.set(0.5);
  anim.x = app.screen.width / 2;
  anim.y = app.screen.height / 2;

  // set the animation speed
  anim.animationSpeed = 0.4;
  // play the animation on a loop
  anim.play();
  // add it to the stage to render
  app.stage.addChild(anim);
})();
