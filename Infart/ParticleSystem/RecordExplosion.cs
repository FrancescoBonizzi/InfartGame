using Infart.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Infart.ParticleSystem
{
    public class RecordExplosion
    {
        private Particle _scritta = null;

        private readonly Rectangle _textureRectangle;

        private readonly Vector2 _origin;

        private readonly Texture2D _texture;

        private static Random _random;

        public RecordExplosion(
            Texture2D textureReference,
            Rectangle textureRectangle)
        {
            _random = FbonizziHelper.Random;
            _textureRectangle = textureRectangle;
            _origin = new Vector2(_textureRectangle.Width / 2, _textureRectangle.Height / 2);
            _texture = textureReference;
            _scritta = new Particle();
        }

        public void Dispose()
        {
            _scritta = null;
        }

        public void Explode(Vector2 @where, int directionAngleDegrees)
        {
            float radians = MathHelper.ToRadians(directionAngleDegrees);
            Vector2 direction = new Vector2(
                direction.X = (float)Math.Cos(radians), direction.Y = -(float)Math.Sin(radians));

            float velocity =
                FbonizziHelper.RandomBetween(120.0f, 180.0f);
            float acceleration =
                FbonizziHelper.RandomBetween(100.0f, 130.0f);
            float lifetime = 4.0f;
            float scale = 2.0f;
            float rotationSpeed = (float)_random.NextDouble() * 10;

            _scritta.Initialize(
                @where,
                velocity * direction,
                acceleration * direction,
                rotationSpeed,
                Color.White,
                scale,
                lifetime);
        }

        public void Update(double gameTime)
        {
            if (_scritta.Active)
            {
                float dt = (float)gameTime / 1000.0f;

                _scritta.Update(dt);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_scritta.Active)
                return;

            float normalizedLifetime = _scritta.TimeSinceStart / _scritta.LifeTime;

            float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);

            float scale = _scritta.Scale * (.75f + .25f * normalizedLifetime);

            spriteBatch.Draw(_texture, _scritta.Position, _textureRectangle, _scritta.Color * alpha,
                _scritta.Rotation, _origin, scale, SpriteEffects.None, 0.0f);
        }
    }
}