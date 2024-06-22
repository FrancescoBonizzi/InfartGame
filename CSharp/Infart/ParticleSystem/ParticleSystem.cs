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

        protected Queue<Particle> _freeParticles;
        protected int _minNumParticles;
        protected int _maxNumParticles;
        protected float _minInitialSpeed;
        protected float _maxInitialSpeed;
        protected float _minAcceleration;
        protected float _maxAcceleration;
        protected float _minRotationSpeed;
        protected float _maxRotationSpeed;
        protected float _minLifetime;
        protected float _maxLifetime;
        protected float _minScale;
        protected float _maxScale;
        protected float _minSpawnAngle;
        protected float _maxSpawnAngle;

        private Vector2 _emitterLocation = Vector2.Zero;
        
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

            _activeParticles = new Particle[_density * _maxNumParticles];
            _freeParticles = new Queue<Particle>(_density * _maxNumParticles);
            for (int i = 0; i < _activeParticles.Length; ++i)
            {
                _activeParticles[i] = new Particle();
                _freeParticles.Enqueue(_activeParticles[i]);
            }
        }

        protected abstract void InitializeConstants();

        public virtual void AddParticles(Vector2 where)
        {
            int numParticles = FbonizziMonoGame.Numbers.RandomBetween(_minNumParticles, _maxNumParticles);

            for (int i = 0; i < numParticles && _freeParticles.Count > 0; ++i)
            {
                Particle p = _freeParticles.Dequeue();
                InitializeParticle(p, where);
            }
        }

        protected virtual void InitializeParticle(Particle p, Vector2 where)
        {
            Vector2 direction = PickRandomDirection();

            float velocity =
                FbonizziMonoGame.Numbers.RandomBetween(_minInitialSpeed, _maxInitialSpeed);
            float acceleration =
                FbonizziMonoGame.Numbers.RandomBetween(_minAcceleration, _maxAcceleration);
            float lifetime =
                FbonizziMonoGame.Numbers.RandomBetween(_minLifetime, _maxLifetime);
            float scale =
                FbonizziMonoGame.Numbers.RandomBetween(_minScale, _maxScale);
            float rotationSpeed =
                FbonizziMonoGame.Numbers.RandomBetween(_minRotationSpeed, _maxRotationSpeed);

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
            float radians = FbonizziMonoGame.Numbers.RandomBetween(
                MathHelper.ToRadians(_minSpawnAngle), MathHelper.ToRadians(_maxSpawnAngle));

            Vector2 direction = new Vector2(
                direction.X = (float)Math.Cos(radians), 
                direction.Y = -(float)Math.Sin(radians));

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
                        _freeParticles.Enqueue(p);
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