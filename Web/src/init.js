(async () => {
  // Create a PixiJS application.
  const app = new PIXI.Application();

  // Intialize the application.
  await app.init({ background: "#1099bb", resizeTo: window });

  // Then adding the application's canvas to the DOM body.
  document.body.appendChild(app.canvas);
})();
