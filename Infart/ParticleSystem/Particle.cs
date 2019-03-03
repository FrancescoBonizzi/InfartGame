
using Microsoft.Xna.Framework;

namespace fge
{
    public class Particle
    {
        #region Dichiarazioni

        public Vector2 Position;
        public Color Color;
        public float Scale;

        public Vector2 Velocity;
        public Vector2 Acceleration;

        public float Rotation;
        private float rotation_speed_;

        public float TimeSinceStart;
        public float LifeTime;

        #endregion

        public Particle() { /* Nothing to do */ }

        public void Initialize(
             Vector2 Position,
             Vector2 Velocity,
             Vector2 Acceleration,
             float RotationSpeed,
             Color Color,
             float Scale,
             float Lifetime)
        {
            this.Position = Position;
            this.Color = Color;
            this.Scale = Scale;

            this.Velocity = Velocity;
            this.Acceleration = Acceleration;
            this.rotation_speed_ = RotationSpeed;
            this.LifeTime = Lifetime;
            this.TimeSinceStart = 0.0f;
            this.Rotation = fbonizziHelper.RandomBetween(0, MathHelper.TwoPi);
        }

        public bool Active
        {
            get { return TimeSinceStart < LifeTime || Color.R < 0; }
        }

        public void Update(float dt)
        {
            Velocity += Acceleration * dt;
            Position += Velocity * dt;
            Rotation += rotation_speed_ * dt;

            TimeSinceStart += dt;
        }
    }
}
