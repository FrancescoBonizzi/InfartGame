#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

#endregion

namespace fge
{
    public class StarFieldParticleSystem : ParticleSystem
    {
        public StarFieldParticleSystem(
            int Density,
            Loader_episodio1 Loader)
            : base(Density, Loader.textures_, Loader.textures_rectangles_["Stella"])
        {
        }

        protected override void InitializeConstants()
        {
            // Prova a giocarci per vedere cosa succede

            min_initial_speed_ = 10f;
            max_initial_speed_ = 15f;

            min_acceleration_ = 30f;
            max_acceleration_ = 40f;

            min_lifetime_ = 1.50f;
            max_lifetime_ = 3.0f;

            min_scale_ = .2f;
            max_scale_ = 1.0f;

            min_spawn_angle_ = 0.0f;
            max_spawn_angle_ = 360.0f;

            min_num_particles_ = 4;
            max_num_particles_ = 8;

            min_rotation_speed_ = -MathHelper.PiOver2;
            max_rotation_speed_ = MathHelper.PiOver2;
        }

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);
        }
    }
}
