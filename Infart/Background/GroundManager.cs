using Infart.Assets;
using Infart.Drawing;
using Infart.Extensions;
using Infart.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Infart.Background
{
    public class GroundManager
    {
        protected Random Random;

        protected Camera CurrentCamera;

        private readonly GrattacieliAutogeneranti _grattacieliCamminabili = null;

        private readonly float _minTimeToNextBuco = 2000.0f;

        private float _elapsed = 0.0f;

        private readonly InfartGame _gameManagerReference;

        public GroundManager(
            Camera currentCamera,
            AssetsLoader assetsLoader,
            InfartGame gameManagerReference)
        {
            this.CurrentCamera = currentCamera;
            Random = FbonizziHelper.Random;

            _grattacieliCamminabili = new GrattacieliAutogeneranti(
                assetsLoader.TexturesBuildingsGround,
                assetsLoader.TexturesRectangles,
                "ground",
                69,
                currentCamera,
                gameManagerReference);

            _gameManagerReference = gameManagerReference;
        }

        public List<GameObject> WalkableObjects()
        {
            return _grattacieliCamminabili.DrawnObjectsList;
        }

        public void Reset(Camera camera)
        {
            CurrentCamera = camera;
            _grattacieliCamminabili.Reset(camera);
            _elapsed = 0.0f;
        }

        private void GenerateBuco()
        {
            int firstX = (int)_grattacieliCamminabili.NextGrattacieloPosition.X;
            int space = Random.Next((int)_gameManagerReference.LarghezzaBuchi.X, (int)_gameManagerReference.LarghezzaBuchi.Y);

            _grattacieliCamminabili.NextGrattacieloPosition = new Vector2(
                firstX + space,
                (int)_grattacieliCamminabili.NextGrattacieloPosition.Y);
        }

        public void Update(double gametime)
        {
            _elapsed += (float)gametime;
            if (_elapsed >= _minTimeToNextBuco)
            {
                if (Random.NextDouble() < _gameManagerReference.BucoProbability)
                {
                    GenerateBuco();
                    _elapsed = 0.0f;
                }
            }

            _grattacieliCamminabili.Update(gametime, CurrentCamera);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            _grattacieliCamminabili.Draw(spritebatch);
        }
    }
}