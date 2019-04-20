using Infart.Assets;
using Infart.Drawing;
using Infart.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Infart.Background
{
    public class BackgroundManager
    {
        private readonly GrattacieliAutogeneranti _grattacieliFondo = null;
        private readonly GrattacieliAutogeneranti _grattacieliMid = null;
        private float _parallaxSpeedFondo;
        private float _parallaxSpeedMid;
        private const float DefaultParallaxSpeedFondo = -10.0f;
        private const float DefaultParallaxSpeedMid = -18.0f;
        private readonly Nuvolificio _nuvolificioVicino;
        private readonly Nuvolificio _nuvolificioMedio;
        private readonly Nuvolificio _nuvolificioLontano;
        private readonly Vector2 _nuvoleDefaultSpawnYRange = new Vector2(300.0f, -300.0f);
        private readonly StarFieldParticleSystem _starfield;
        private readonly Vector2 _cieloStellatoSpawnYRange = new Vector2(-960.0f, -300.0f);
        private const double TimeBetweenNewStar = 20.0f;
        private double _timeTillNewStar = 0.0f;
        private Camera CurrentCamera;
        private float OldCameraXPos;
        private float ParallaxDir = +1;
        private Texture2D TextureReference;
        private Rectangle SfondoRectangle;
        private Vector2 SfondoOrigin;
        private Vector2 SfondoScale;

        public BackgroundManager(
            Camera cameraInstance,
            AssetsLoader assetsLoader,
            InfartGame gameManagerReference)
        {
            CurrentCamera = cameraInstance;
            OldCameraXPos = CurrentCamera.Position.X;

            SfondoRectangle = assetsLoader.TexturesRectangles["background"];
            TextureReference = assetsLoader.Textures;
            SfondoOrigin = new Vector2(0.0f, SfondoRectangle.Height);
            SfondoScale = Vector2.One;

            _grattacieliFondo = new GrattacieliAutogeneranti(
                assetsLoader.TexturesBuildingsBack, assetsLoader.TexturesRectangles, "back", 69, cameraInstance, gameManagerReference);

            SfondoScale = new Vector2(1.8f);

            _grattacieliMid = new GrattacieliAutogeneranti(
                assetsLoader.TexturesBuildingsMid, assetsLoader.TexturesRectangles, "mid", 69, cameraInstance, gameManagerReference);

            _parallaxSpeedFondo = DefaultParallaxSpeedFondo;
            _parallaxSpeedMid = DefaultParallaxSpeedMid;

            List<Rectangle> tmp = new List<Rectangle>
            {
                assetsLoader.TexturesRectangles["nuvola1"],
                assetsLoader.TexturesRectangles["nuvola2"],
                assetsLoader.TexturesRectangles["nuvola3"]
            };

            _nuvolificioVicino = new Nuvolificio(
                Color.White, 0.6f, new Vector2(45, 60f),
                _nuvoleDefaultSpawnYRange, cameraInstance, tmp, assetsLoader.Textures);

            _nuvolificioMedio = new Nuvolificio(
               new Color(9, 50, 67), 0.4f, new Vector2(30f, 40f),
                _nuvoleDefaultSpawnYRange, cameraInstance, tmp, assetsLoader.Textures);

            _nuvolificioLontano = new Nuvolificio(
                 new Color(5, 23, 40), 0.2f, new Vector2(10f, 20f),
                _nuvoleDefaultSpawnYRange, cameraInstance, tmp, assetsLoader.Textures);

            _starfield = new StarFieldParticleSystem(8, assetsLoader);

            CurrentCamera = cameraInstance;
            OldCameraXPos = CurrentCamera.Position.X;
        }

        public void IncreaseParallaxSpeed()
        {
            _parallaxSpeedFondo -= 4.0f;
            _parallaxSpeedMid -= 4.0f;
        }

        public void DecreaseParallaxSpeed()
        {
            _parallaxSpeedFondo += 4.0f;
            _parallaxSpeedMid += 4.0f;
        }

        public void Reset(Camera camera)
        {
            CurrentCamera = camera;
            OldCameraXPos = CurrentCamera.Position.X;
            _grattacieliFondo.Reset(camera);
            _grattacieliMid.Reset(camera);

            _nuvolificioLontano.Reset(camera);
            _nuvolificioMedio.Reset(camera);
            _nuvolificioVicino.Reset(camera);

            _parallaxSpeedFondo = DefaultParallaxSpeedFondo;
            _parallaxSpeedMid = DefaultParallaxSpeedMid;
        }

        public void Update(double gametime)
        {
            if (OldCameraXPos - CurrentCamera.Position.X < 0)
            {
                ParallaxDir = +1;
            }
            else if (OldCameraXPos - CurrentCamera.Position.X > 0)
            {
                ParallaxDir = -1;
            }
            else
            {
                ParallaxDir = 0;
            }

            OldCameraXPos = CurrentCamera.Position.X;

            float dt = (float)gametime / 1000.0f;

            _grattacieliFondo.MoveX(_parallaxSpeedFondo * dt * ParallaxDir);
            _grattacieliMid.MoveX(_parallaxSpeedMid * dt * ParallaxDir);
            _nuvolificioLontano.MoveX((float)((_parallaxSpeedFondo) * dt * ParallaxDir));
            _nuvolificioMedio.MoveX((float)((_parallaxSpeedMid) * dt * ParallaxDir));

            _grattacieliFondo.Update(gametime);
            _grattacieliMid.Update(gametime);

            _starfield.Update(gametime);

            if (CurrentCamera.Position.Y >= _cieloStellatoSpawnYRange.X
                && CurrentCamera.Position.Y <= _cieloStellatoSpawnYRange.Y)
                GenerateStars(gametime);

            _nuvolificioLontano.Update(gametime);
            _nuvolificioMedio.Update(gametime);
            _nuvolificioVicino.Update(gametime);
        }

        private void GenerateStars(double dt)
        {
            _timeTillNewStar -= dt;
            if (_timeTillNewStar < 0)
            {
                var where = new Vector2(
                    FbonizziMonoGame.Numbers.RandomBetween((int)CurrentCamera.Position.X, (int)CurrentCamera.Position.X + CurrentCamera.ViewPortWidth),
                    FbonizziMonoGame.Numbers.RandomBetween((int)_cieloStellatoSpawnYRange.X, (int)_cieloStellatoSpawnYRange.Y));

                _starfield.AddParticles(where);
                _timeTillNewStar = TimeBetweenNewStar;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(
                    TextureReference,
                    new Vector2(CurrentCamera.Position.X, CurrentCamera.ViewPortHeight),
                    SfondoRectangle,
                    Color.White,
                    0.0f,
                    SfondoOrigin,
                    SfondoScale,
                    SpriteEffects.None,
                    1.0f);

            _starfield.Draw(spritebatch);

            _nuvolificioLontano.Draw(spritebatch);
            _grattacieliFondo.Draw(spritebatch);
            _nuvolificioMedio.Draw(spritebatch);
            _grattacieliMid.Draw(spritebatch);
        }

        public void DrawSpecial(SpriteBatch spritebatch)
        {
            _nuvolificioVicino.Draw(spritebatch);
        }
    }
}