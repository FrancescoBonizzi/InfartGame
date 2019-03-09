
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace fge
{
    
    public class RecordExplosion
    {
        
        private Particle scritta_ = null;
        private Rectangle texture_rectangle_;
        private Vector2 origin_;
        private Texture2D texture_;

        private static Random random_;

        

        
        public RecordExplosion(
            Texture2D TextureReference,
            Rectangle TextureRectangle)
        {
            random_ = fbonizziHelper.random;
            texture_rectangle_ = TextureRectangle;
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

            
        }

        

        
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

            
            
            
            
            
            float normalizedLifetime = scritta_.TimeSinceStart / scritta_.LifeTime;

            
            
            
            
            
            
            
            
            
            float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);

            
            
            float scale = scritta_.Scale * (.75f + .25f * normalizedLifetime);

            spriteBatch.Draw(texture_, scritta_.Position, texture_rectangle_, scritta_.Color * alpha,
                scritta_.Rotation, origin_, scale, SpriteEffects.None, 0.0f);
        }

        
    }
}
