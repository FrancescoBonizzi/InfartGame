using Infart.Assets;
using Microsoft.Xna.Framework;

namespace Infart.ParticleSystem
{
    public class StarFieldParticleSystem : ParticleSystem
    {
        public StarFieldParticleSystem(
            int density,
            AssetsLoader assetsLoader)
            : base(density, assetsLoader.Textures, assetsLoader.TexturesRectangles["Stella"])
        {
        }

        protected override void InitializeConstants()
        {
            MinInitialSpeed = 10f;
            MaxInitialSpeed = 15f;

            MinAcceleration = 30f;
            MaxAcceleration = 40f;

            MinLifetime = 1.50f;
            MaxLifetime = 3.0f;

            MinScale = .2f;
            MaxScale = 1.0f;

            MinSpawnAngle = 0.0f;
            MaxSpawnAngle = 360.0f;

            MinNumParticles = 4;
            MaxNumParticles = 8;

            MinRotationSpeed = -MathHelper.PiOver2;
            MaxRotationSpeed = MathHelper.PiOver2;
        }

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);
        }
    }
}