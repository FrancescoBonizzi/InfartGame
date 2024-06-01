using Infart.Assets;
using Microsoft.Xna.Framework;

namespace Infart.ParticleSystem
{
    public class BroccoloParticleSystem : ParticleSystem
    {
        public BroccoloParticleSystem(int density,
            AssetsLoader assetsLoader)
            : base(density, assetsLoader.Textures, assetsLoader.TexturesRectangles["BroccoloParticle"])
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

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);

            p.Acceleration = -p.Velocity / p.LifeTime;
        }
    }
}