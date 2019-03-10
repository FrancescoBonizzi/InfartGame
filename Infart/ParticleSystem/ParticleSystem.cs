using Infart.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Infart.ParticleSystem
{
    public abstract class ParticleSystem
    {
        private readonly Texture2D _texture;

        private readonly Rectangle _textureRectangle;

        private readonly Vector2 _origin;

        private readonly int _density;

        private Particle[] _activeParticles;

        protected Queue<Particle> FreeParticles;

        protected int MinNumParticles;

        protected int MaxNumParticles;

        protected float MinInitialSpeed;

        protected float MaxInitialSpeed;

        protected float MinAcceleration;

        protected float MaxAcceleration;

        protected float MinRotationSpeed;

        protected float MaxRotationSpeed;

        protected float MinLifetime;

        protected float MaxLifetime;

        protected float MinScale;

        protected float MaxScale;

        protected float MinSpawnAngle;

        protected float MaxSpawnAngle;

        private Vector2 _emitterLocation = Vector2.Zero;

        private static Random _random;

        public ParticleSystem(
            int density,
            Texture2D texture,
            Rectangle textureRectangle)
        {
            _texture = texture;
            _textureRectangle = textureRectangle;
            _origin = new Vector2(_textureRectangle.Width / 2, _textureRectangle.Height / 2);

            _density = density;

            Initialize();
        }

        public ParticleSystem(int density, Texture2D textureName)
        {
            _texture = textureName;
            _textureRectangle = _texture.Bounds;
            _origin = new Vector2(_textureRectangle.Width / 2, _textureRectangle.Height / 2);

            _density = density;

            Initialize();
        }

        public void Initialize()
        {
            InitializeConstants();

            _random = FbonizziHelper.Random;

            _activeParticles = new Particle[_density * MaxNumParticles];
            FreeParticles = new Queue<Particle>(_density * MaxNumParticles);
            for (int i = 0; i < _activeParticles.Length; ++i)
            {
                _activeParticles[i] = new Particle();
                FreeParticles.Enqueue(_activeParticles[i]);
            }
        }

        protected abstract void InitializeConstants();

        public virtual void AddParticles(Vector2 where)
        {
            int numParticles = _random.Next(MinNumParticles, MaxNumParticles);

            for (int i = 0; i < numParticles && FreeParticles.Count > 0; ++i)
            {
                Particle p = FreeParticles.Dequeue();
                InitializeParticle(p, where);
            }
        }

        protected virtual void InitializeParticle(Particle p, Vector2 where)
        {
            Vector2 direction = PickRandomDirection();

            float velocity =
                FbonizziHelper.RandomBetween(MinInitialSpeed, MaxInitialSpeed);
            float acceleration =
                FbonizziHelper.RandomBetween(MinAcceleration, MaxAcceleration);
            float lifetime =
                FbonizziHelper.RandomBetween(MinLifetime, MaxLifetime);
            float scale =
                FbonizziHelper.RandomBetween(MinScale, MaxScale);
            float rotationSpeed =
                FbonizziHelper.RandomBetween(MinRotationSpeed, MaxRotationSpeed);

            p.Initialize(
                where,
                velocity * direction,
                acceleration * direction,
                rotationSpeed,
                Color.White,
                scale,
                lifetime);
        }

        private Vector2 PickRandomDirection()
        {
            float radians = FbonizziHelper.RandomBetween(
                MathHelper.ToRadians(MinSpawnAngle), MathHelper.ToRadians(MaxSpawnAngle));

            Vector2 direction = new Vector2(
                direction.X = (float)Math.Cos(radians), direction.Y = -(float)Math.Sin(radians));

            return direction;
        }

        public virtual void Update(double gameTime)
        {
            float dt = (float)gameTime / 1000.0f;

            for (int i = 0; i < _activeParticles.Length; ++i)
            {
                Particle p = _activeParticles[i];

                if (p.Active)
                {
                    p.Update(dt);

                    if (!p.Active)
                    {
                        FreeParticles.Enqueue(p);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _activeParticles.Length; ++i)
            {
                Particle p = _activeParticles[i];

                if (!p.Active)
                    continue;

                float normalizedLifetime = p.TimeSinceStart / p.LifeTime;

                float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);

                float scale = p.Scale * (.75f + .25f * normalizedLifetime);

                spriteBatch.Draw(_texture, p.Position, _textureRectangle, p.Color * alpha,
                    p.Rotation, _origin, scale, SpriteEffects.None, 0.0f);
            }
        }
    }
}