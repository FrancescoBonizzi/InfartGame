using Infart.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Infart.ParticleSystem
{
    public class InfartExplosion
    {
        private double _elapsed = 0.0;
        private const double TimeToFinish = 5000.0;
        private const float ScrittaScaleIncreaseAmount = 0.004f;
        private const int ParticelleNumber = 100;
        private readonly List<ParticleExplosion> _particelle;
        private ParticleExplosion _scritta = null;

        private readonly List<Color> _fartColors = new List<Color> {
            new Color(111, 86, 41),
            new Color(65, 44, 32),
            Color.LawnGreen
        };

        private readonly Vector2 _emitterLocation = Vector2.Zero;

        private bool _active = false;
        private bool _withText;

        public bool Finished { get; private set; } = false;
        public bool Started { get; private set; } = false;

        public InfartExplosion(AssetsLoader assetsLoader)
        {
            _particelle = new List<ParticleExplosion>();

            AddNewScritta(assetsLoader.Textures, assetsLoader.TexturesRectangles["Bang"]);
            AddRandomParticles(assetsLoader.Textures, assetsLoader.TexturesRectangles["Burger"], assetsLoader.TexturesRectangles["Merda"]);
        }

        public void Explode(Vector2 centerPosition, bool withText, SoundManager soundManager)
        {
            Started = true;
            _withText = withText;

            _active = true;
            soundManager.PlayExplosion();
            SetEmitterLocation(centerPosition);
        }

        public void Reset()
        {
            Finished = false;
            Started = false;
            _elapsed = 0.0;
            _active = false;

            ResetParticles();
            ResetScritta();
        }

        private void SetEmitterLocation(Vector2 emitterLocation)
        {
            _scritta.Position = emitterLocation;

            for (int i = 0; i < ParticelleNumber; ++i)
                _particelle[i].Position = emitterLocation;
        }

        private void ResetParticles()
        {
            for (int i = 0; i < ParticelleNumber; ++i)
            {
                float angle = FbonizziMonoGame.Numbers.RandomBetween(0, 360);
                angle = MathHelper.ToRadians(-angle);
                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle))
                    * FbonizziMonoGame.Numbers.RandomBetween(80, 140);

                _particelle[i].Refactor(
                        velocity,
                        angle,
                        (float)(FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) * 5),
                        _fartColors[FbonizziMonoGame.Numbers.RandomBetween(0, _fartColors.Count)],
                        (float)FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) - 0.3f,
                        500);
            }
        }

        private void ResetScritta()
        {
            _scritta.Refactor(
               Vector2.Zero,
               (float)FbonizziMonoGame.Numbers.RandomBetween(0D, 1D),
               (float)FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) * 10,
               Color.White,
               1.0f,
               500);
        }

        private void AddRandomParticles(
            Texture2D texture,
            Rectangle particleRectangle1,
            Rectangle particleRectangle2)
        {
            for (int i = 0; i < ParticelleNumber; ++i)
            {
                float angle = FbonizziMonoGame.Numbers.RandomBetween(0, 360);
                angle = MathHelper.ToRadians(-angle);
                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle))
                    * FbonizziMonoGame.Numbers.RandomBetween(80, 140);

                _particelle.Add(
                    new ParticleExplosion(
                       texture,
                        FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) < 0.5 ? particleRectangle1 : particleRectangle2,
                        _emitterLocation,
                        velocity,
                        angle,
                        (float)(FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) * 5),
                        _fartColors[FbonizziMonoGame.Numbers.RandomBetween(0, _fartColors.Count)],
                        (float)FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) - 0.3f,
                        500));
            }
        }

        public void AddNewScritta(
            Texture2D textureScritta,
            Rectangle textureRectangle)
        {
            _scritta = new ParticleExplosion(
                textureScritta,
                textureRectangle,
                _emitterLocation,
                Vector2.Zero,
                (float)FbonizziMonoGame.Numbers.RandomBetween(0D, 1D),
                (float)FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) * 10,
                Color.White,
                1.0f,
                500);
        }

        public void Update(double gameTime)
        {
            if (_active)
            {
                _elapsed += gameTime;

                if (_elapsed >= TimeToFinish)
                    Finished = true;

                for (int i = 0; i < _particelle.Count; ++i)
                    _particelle[i].Update(gameTime);

                if (_withText)
                {
                    _scritta.Scale += ScrittaScaleIncreaseAmount * ((float)_elapsed / 1000.0f);
                    _scritta.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_active)
            {
                for (int i = 0; i < _particelle.Count; ++i)
                    _particelle[i].Draw(spriteBatch);

                if (_withText)
                    _scritta.Draw(spriteBatch);
            }
        }
    }
}