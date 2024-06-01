using Microsoft.Xna.Framework;

namespace Infart.ParticleSystem
{
    public class Particle
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public float Scale { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Rotation { get; set; }
        public float TimeSinceStart { get; set; }
        public float LifeTime { get; set; }

        private float _rotationSpeed;
        
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
            this.Rotation = FbonizziMonoGame.Numbers.RandomBetween(0, MathHelper.TwoPi);
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