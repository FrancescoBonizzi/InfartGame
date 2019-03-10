using Infart.Extensions;
using Microsoft.Xna.Framework;

namespace Infart.ParticleSystem
{
    public class Particle
    {
        public Vector2 Position;

        public Color Color;

        public float Scale;

        public Vector2 Velocity;

        public Vector2 Acceleration;

        public float Rotation;

        private float _rotationSpeed;

        public float TimeSinceStart;

        public float LifeTime;

        public Particle()
        {
        }

        public void Initialize(
             Vector2 position,
             Vector2 velocity,
             Vector2 acceleration,
             float rotationSpeed,
             Color color,
             float scale,
             float lifetime)
        {
            this.Position = position;
            this.Color = color;
            this.Scale = scale;

            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this._rotationSpeed = rotationSpeed;
            this.LifeTime = lifetime;
            this.TimeSinceStart = 0.0f;
            this.Rotation = FbonizziHelper.RandomBetween(0, MathHelper.TwoPi);
        }

        public bool Active
        {
            get { return TimeSinceStart < LifeTime || Color.R < 0; }
        }

        public void Update(float dt)
        {
            Velocity += Acceleration * dt;
            Position += Velocity * dt;
            Rotation += _rotationSpeed * dt;

            TimeSinceStart += dt;
        }
    }
}