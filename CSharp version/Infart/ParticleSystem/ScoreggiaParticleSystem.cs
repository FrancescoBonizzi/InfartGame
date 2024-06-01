using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.ParticleSystem
{
    public class ScoreggiaParticleSystem : ParticleSystem
    {
        public ScoreggiaParticleSystem(
            int density,
            Texture2D particleTexture,
            Rectangle textureRectangle)
            : base(density, particleTexture, textureRectangle)
        {
        }

        protected override void InitializeConstants()
        {
            _minInitialSpeed = 50.0f;
            _maxInitialSpeed = 80.0f;

            _minAcceleration = 30.0f;
            _maxAcceleration = 50.0f;

            _minLifetime = 0.5f;
            _maxLifetime = 0.7f;

            _minScale = .5f;
            _maxScale = 1.0f;

            _minSpawnAngle = 195.0f;
            _maxSpawnAngle = 280.0f;

            _minNumParticles = 4;
            _maxNumParticles = 8;

            _minRotationSpeed = -MathHelper.PiOver4 / 2.0f;
            _maxRotationSpeed = MathHelper.PiOver4 / 2.0f;
        }
    }
}