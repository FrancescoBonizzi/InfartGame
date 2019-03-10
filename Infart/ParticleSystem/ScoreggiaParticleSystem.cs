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
            MinInitialSpeed = 50.0f;
            MaxInitialSpeed = 80.0f;

            MinAcceleration = 30.0f;
            MaxAcceleration = 50.0f;

            MinLifetime = 0.5f;
            MaxLifetime = 0.7f;

            MinScale = .5f;
            MaxScale = 1.0f;

            MinSpawnAngle = 195.0f;
            MaxSpawnAngle = 280.0f;

            MinNumParticles = 4;
            MaxNumParticles = 8;

            MinRotationSpeed = -MathHelper.PiOver4 / 2.0f;
            MaxRotationSpeed = MathHelper.PiOver4 / 2.0f;
        }
    }
}