(async () => {
  // Create a PixiJS application.
  const app = new PIXI.Application();

  // Intialize the application.
  await app.init({ 
    background: "#1099bb", 
    width: 800,
    height: 480
  });

  const texture = await PIXI.Assets.load('src/assets/images/menuBackground.png');
  const sprite1 = new PIXI.Sprite(texture);
  sprite1.anchor.set(0.5);

  sprite1.x = app.screen.width / 2;
  sprite1.y = app.screen.height / 2;
  app.stage.addChild(sprite1);

  // Then adding the application's canvas to the DOM body.
  document.body.appendChild(app.canvas);
})();
