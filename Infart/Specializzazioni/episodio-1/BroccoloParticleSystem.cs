
using Microsoft.Xna.Framework;



namespace fge
{
    public class BroccoloParticleSystem : ParticleSystem
    {
        public BroccoloParticleSystem(int Density,
            Loader_episodio1 Loader)
            : base(Density, Loader.textures_, Loader.textures_rectangles_["BroccoloParticle"])
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

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);

            
            
            
            

            
            
            
            
            
            p.Acceleration = -p.Velocity / p.LifeTime;
        }
    }
}
