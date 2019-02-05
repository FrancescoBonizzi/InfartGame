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
    public class MerdeVolantiParticleSystem : ParticleSystem
    {
        public MerdeVolantiParticleSystem(int Density, Texture2D texture)
            : base(Density, texture)
        {
        }

        protected override void InitializeConstants()
        {
            min_initial_speed_ = 20f;
            max_initial_speed_ = 25f;

            min_acceleration_ = 5f;
            max_acceleration_ = 10f;

            min_lifetime_ = 5.50f;
            max_lifetime_ = 8.0f;

            min_scale_ = .2f;
            max_scale_ = 1.0f;

            min_spawn_angle_ = 0.0f;
            max_spawn_angle_ = 360.0f;

            min_num_particles_ = 10;
            max_num_particles_ = 15;

            min_rotation_speed_ = -MathHelper.PiOver2;
            max_rotation_speed_ = MathHelper.PiOver2;
        }

        protected override void InitializeParticle(Particle p, Vector2 where)
        {
            base.InitializeParticle(p, where);
        }
    }
}
