using Infart.Assets;
using Microsoft.Xna.Framework;

namespace Infart.ParticleSystem
{
    public class JalapenoParticleSystem : ParticleSystem
    {
        public JalapenoParticleSystem(
            int density,
            AssetsLoader assetsLoader)
            : base(density, assetsLoader.Textures, assetsLoader.TexturesRectangles["JalapenoParticle"])
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