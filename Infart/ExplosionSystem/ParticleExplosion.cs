
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace fge
{
    public class ParticleExplosion
    {
        
        private Texture2D texture_;
        private Rectangle texture_rectangle_;

        public Vector2 position_;
        private Vector2 velocity_;

        private float angle_;
        private float angular_velocity_;

        private Color color_;
        private float scale_;
        private int TTL_;

        private Vector2 origin_;

        private bool fading_ = false;
        private bool active_ = false;

        

        
        public ParticleExplosion(
            Texture2D texture,
            Rectangle texture_rectangle,
            Vector2 position,
            Vector2 velocity,
            float angle,
            float angularVelocity,
            Color color,
            float size,
            int ttl)
        {
            texture_ = texture;
            texture_rectangle_ = texture_rectangle;
            position_ = position;
            velocity_ = velocity;
            angle_ = angle;
            angular_velocity_ = angularVelocity;
            color_ = color;
            scale_ = size;
            TTL_ = ttl;

            fading_ = false;
            active_ = true;

            origin_ = new Vector2(texture_rectangle_.Width / 2, texture_rectangle_.Height / 2);
        }

        public void Initialize(
             Vector2 position,
             Vector2 velocity,
             float angle,
             float angularVelocity,
             Color color,
             float size,
             int ttl)
        {
            position_ = position;
            velocity_ = velocity;
            angle_ = angle;
            angular_velocity_ = angularVelocity;
            color_ = color;
            scale_ = size;
            TTL_ = ttl;

            active_ = true;
            fading_ = false;
        }

        public void Refactor(
            Vector2 velocity,
            float angle,
            float angularVelocity,
            Color color,
            float size,
            int ttl)
        {
            velocity_ = velocity;
            angle_ = angle;
            angular_velocity_ = angularVelocity;
            color_ = color;
            scale_ = size;
            TTL_ = ttl;

            fading_ = false;
            active_ = true;
        }

        

        
        public Vector2 Position
        {
            get { return position_; }
            set { position_ = value; }
        }

        public float Scale
        {
            get { return scale_; }
            set { scale_ = value; }
        }

        public bool IsDead
        {
            get { return !active_; }
        }

        

        
        public void Fade()
        {
            fading_ = true;
        }

        

        
        public void Update(double gameTime)
        {
            if (active_)
            {
                float elapsed = (float)gameTime / 1000.0f;

                --TTL_;
                position_ += velocity_ * elapsed;
                angle_ += angular_velocity_ * elapsed;

                if (!fading_ && TTL_ <= 0)
                {
                    fading_ = true;
                }

                if (fading_)
                {
                    color_ *= 0.95f;
                    if (color_.A <= 50)
                    {
                        active_ = false;
                    }
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active_)
            {
                spriteBatch.Draw(
                    texture_,
                    position_,
                    texture_rectangle_,
                    color_,
                    angle_,
                    origin_,
                    scale_,
                    SpriteEffects.None,
                    0f);
            }
        }


        
    }
}
