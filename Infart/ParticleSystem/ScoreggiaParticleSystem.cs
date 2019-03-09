using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.ParticleSystem
{
    public class ScoreggiaParticleSystem : ParticleSystem
    {
        public ScoreggiaParticleSystem(
            int Density,
            Texture2D ParticleTexture,
            Rectangle TextureRectangle)
            : base(Density, ParticleTexture, TextureRectangle)
        {
        }

        protected override void InitializeConstants()
        {
            min_initial_speed_ = 50.0f;
            max_initial_speed_ = 80.0f;

            min_acceleration_ = 30.0f;
            max_acceleration_ = 50.0f;

            min_lifetime_ = 0.5f;
            max_lifetime_ = 0.7f;

            min_scale_ = .5f;
            max_scale_ = 1.0f;

            min_spawn_angle_ = 195.0f;
            max_spawn_angle_ = 280.0f;

            min_num_particles_ = 4;
            max_num_particles_ = 8;

            min_rotation_speed_ = -MathHelper.PiOver4 / 2.0f;
            max_rotation_speed_ = MathHelper.PiOver4 / 2.0f;
        }
    }
}