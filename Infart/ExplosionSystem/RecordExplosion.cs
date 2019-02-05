#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace fge
{
    // Da rifare tutto come infart explosion
    public class RecordExplosion
    {
        #region Dichiarazioni

        private Particle scritta_ = null;
        private Rectangle texture_rectangle_;
        private Vector2 origin_;
        private Texture2D texture_;

        private static Random random_;

        #endregion

        #region Costruttore / Distruttore

        public RecordExplosion(
            Texture2D TextureReference,
            Rectangle TextureRectangle)
        {
            random_ = fbonizziHelper.random;
            texture_rectangle_ = TextureRectangle;//Loader.textures_rectangles_["Record"];
            origin_ = new Vector2(texture_rectangle_.Width / 2, texture_rectangle_.Height / 2);
            texture_ = TextureReference;
            scritta_ = new Particle();
        }

        public void Dispose()
        {
            scritta_ = null;
        }

        public void Explode(Vector2 Where, int direction_angle_degrees)
        {
            float radians = MathHelper.ToRadians(direction_angle_degrees);
            Vector2 direction = new Vector2(
                direction.X = (float)Math.Cos(radians), direction.Y = -(float)Math.Sin(radians));

            float velocity =
                fbonizziHelper.RandomBetween(120.0f, 180.0f);
            float acceleration =
                fbonizziHelper.RandomBetween(100.0f, 130.0f);
            float lifetime = 4.0f;
            float scale = 2.0f;
            float rotationSpeed = (float)random_.NextDouble() * 10;

            scritta_.Initialize(
                Where,
                velocity * direction,
                acceleration * direction,
                rotationSpeed,
                Color.White,
                scale,
                lifetime);

            //  SoundManager.PlayRecord();
        }

        #endregion

        #region Update / Draw

        public void Update(double gameTime)
        {
            if (scritta_.Active)
            {
                float dt = (float)gameTime / 1000.0f;

                scritta_.Update(dt);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            if (!scritta_.Active)
                return;

            // Normalized lifetime is a value from 0 to 1 and represents how far
            // a particle is through its life. 0 means it just started, .5 is half
            // way through, and 1.0 means it's just about to be finished.
            // this value will be used to calculate alpha and scale, to avoid 
            // having particles suddenly appear or disappear.
            float normalizedLifetime = scritta_.TimeSinceStart / scritta_.LifeTime;

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
            float scale = scritta_.Scale * (.75f + .25f * normalizedLifetime);

            spriteBatch.Draw(texture_, scritta_.Position, texture_rectangle_, scritta_.Color * alpha,
                scritta_.Rotation, origin_, scale, SpriteEffects.None, 0.0f);
        }

        #endregion
    }
}
