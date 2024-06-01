export const loadAssets = async () => {

    const texture = await PIXI.Assets.load('https://pixijs.com/assets/bunny.png');
    const sprite1 = new PIXI.Sprite(texture);
    sprite1.anchor.set(0.5);
    return sprite1;

};
