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
            _minInitialSpeed = 10f;
            _maxInitialSpeed = 15f;

            _minAcceleration = 30f;
            _maxAcceleration = 40f;

            _minLifetime = 1.50f;
            _maxLifetime = 3.0f;

            _minScale = .2f;
            _maxScale = 1.0f;

            _minSpawnAngle = 0.0f;
            _maxSpawnAngle = 360.0f;

            _minNumParticles = 4;
            _maxNumParticles = 8;

            _minRotationSpeed = -MathHelper.PiOver2;
            _maxRotationSpeed = MathHelper.PiOver2;
        }

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);
        }
    }
}