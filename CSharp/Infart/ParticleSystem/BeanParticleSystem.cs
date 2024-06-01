using Infart.Assets;
using Microsoft.Xna.Framework;

namespace Infart.ParticleSystem
{
    public class BeanParticleSystem : ParticleSystem
    {
        public BeanParticleSystem(int density,
            AssetsLoader assetsLoader)
            : base(density, assetsLoader.Textures, assetsLoader.TexturesRectangles["Merda"])
        {

        }

        protected override void InitializeConstants()
        {
            _minInitialSpeed = 300.0f;
            _maxInitialSpeed = 400.0f;

            _minAcceleration = 200.0f;
            _maxAcceleration = 300.0f;

            _minLifetime = 0.5f;
            _maxLifetime = 0.8f;

            _minScale = .5f;
            _maxScale = 1.0f;

            _minSpawnAngle = 270.0f;
            _maxSpawnAngle = 320.0f;

            _minNumParticles = 8;
            _maxNumParticles = 16;

            _minRotationSpeed = -MathHelper.Pi;
            _maxRotationSpeed = MathHelper.Pi;
        }

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);

            p.Acceleration = -p.Velocity / p.LifeTime;
        }
    }
}
