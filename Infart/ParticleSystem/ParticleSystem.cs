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
    public abstract class ParticleSystem
    {
        #region Dichiarazioni

        private Texture2D texture_;
        private Rectangle texture_rectangle_;
        private Vector2 origin_;

        private int density_;

        private Particle[] active_particles_;
        protected Queue<Particle> free_particles_;

        protected int min_num_particles_;
        protected int max_num_particles_;

        protected float min_initial_speed_;
        protected float max_initial_speed_;

        protected float min_acceleration_;
        protected float max_acceleration_;

        protected float min_rotation_speed_;
        protected float max_rotation_speed_;

        protected float min_lifetime_;
        protected float max_lifetime_;

        protected float min_scale_;
        protected float max_scale_;

        protected float min_spawn_angle_;
        protected float max_spawn_angle_;

        private Vector2 emitter_location_ = Vector2.Zero;

        private static Random random_;

        #endregion

        #region Costruttore / Inizializzazione

        public ParticleSystem(
            int Density,
            Texture2D Texture,
            Rectangle TextureRectangle)
        {
            texture_ = Texture;
            texture_rectangle_ = TextureRectangle;//Loader.textures_rectangles_[TextureName];
            origin_ = new Vector2(texture_rectangle_.Width / 2, texture_rectangle_.Height / 2);

            density_ = Density;

            Initialize();
        }

        public ParticleSystem(int Density, Texture2D TextureName)
        {
            texture_ = TextureName;
            texture_rectangle_ = texture_.Bounds;
            origin_ = new Vector2(texture_rectangle_.Width / 2, texture_rectangle_.Height / 2);

            density_ = Density;

            Initialize();
        }

        public void Initialize()
        {
            InitializeConstants();

            random_ = fbonizziHelper.random;

            active_particles_ = new Particle[density_ * max_num_particles_];
            free_particles_ = new Queue<Particle>(density_ * max_num_particles_);
            for (int i = 0; i < active_particles_.Length; ++i)
            {
                active_particles_[i] = new Particle();
                free_particles_.Enqueue(active_particles_[i]);
            }
        }

        protected abstract void InitializeConstants();

        #endregion

        #region Metodi

        public virtual void AddParticles(Vector2 where)
        {
            int numParticles = random_.Next(min_num_particles_, max_num_particles_);

            for (int i = 0; i < numParticles && free_particles_.Count > 0; ++i)
            {
                Particle p = free_particles_.Dequeue();
                InitializeParticle(p, where);
            }
        }

        protected virtual void InitializeParticle(Particle p, Vector2 where)
        {
            Vector2 direction = PickRandomDirection();

            // pick some random values for our particle
            float velocity =
                fbonizziHelper.RandomBetween(min_initial_speed_, max_initial_speed_);
            float acceleration =
                fbonizziHelper.RandomBetween(min_acceleration_, max_acceleration_);
            float lifetime =
                fbonizziHelper.RandomBetween(min_lifetime_, max_lifetime_);
            float scale =
                fbonizziHelper.RandomBetween(min_scale_, max_scale_);
            float rotationSpeed =
                fbonizziHelper.RandomBetween(min_rotation_speed_, max_rotation_speed_);

            p.Initialize(
                where,
                velocity * direction,
                acceleration * direction,
                rotationSpeed,
                Color.White,
                scale,
                lifetime);
        }

        private Vector2 PickRandomDirection()
        {
            float radians = fbonizziHelper.RandomBetween(
                MathHelper.ToRadians(min_spawn_angle_), MathHelper.ToRadians(max_spawn_angle_));

            Vector2 direction = new Vector2(
                direction.X = (float)Math.Cos(radians), direction.Y = -(float)Math.Sin(radians));

            return direction;
        }

        #endregion

        #region Update/Draw

        public virtual void Update(double gameTime)
        {
            float dt = (float)gameTime / 1000.0f;

            for (int i = 0; i < active_particles_.Length; ++i)
            {
                Particle p = active_particles_[i];

                if (p.Active)
                {
                    p.Update(dt);

                    if (!p.Active)
                    {
                        free_particles_.Enqueue(p);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < active_particles_.Length; ++i)
            {
                Particle p = active_particles_[i];

                if (!p.Active)
                    continue;

                // Normalized lifetime is a value from 0 to 1 and represents how far
                // a particle is through its life. 0 means it just started, .5 is half
                // way through, and 1.0 means it's just about to be finished.
                // this value will be used to calculate alpha and scale, to avoid 
                // having particles suddenly appear or disappear.
                float normalizedLifetime = p.TimeSinceStart / p.LifeTime;

                // We want particles to fade in and fade out, so we'll calculate alpha
                // to be (normalizedLifetime) * (1-normalizedLifetime). this way, when
                // normalizedLifetime is 0 or 1, alpha is 0. the maximum value is at
                // normalizedLifetime = .5, and is
                // (normalizedLifetime) * (1-normalizedLifetime)
                // (.5)                 * (1-.5)
                // .25
                // since we want the maximum alpha to be 1, not .25, we'll scale the 
                // entire equation by 4.
                float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);

                // Make particles grow as they age. they'll start at 75% of their size,
                // and increase to 100% once they're finished.
                float scale = p.Scale * (.75f + .25f * normalizedLifetime);

                spriteBatch.Draw(texture_, p.Position, texture_rectangle_, p.Color * alpha,
                    p.Rotation, origin_, scale, SpriteEffects.None, 0.0f);
            }
        }

        #endregion
    }
}
