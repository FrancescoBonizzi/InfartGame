
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace fge
{
    public class Gemma : GameObject
    {
        
        
        
        private float move_y_amount_ = 10f;
        private float elapsed_ = 0.0f;

        private Texture2D texture_reference_;
        private Rectangle texture_rectangle_;

        private Rectangle collision_rectangle_;

        private bool active_;

        

        
        public Gemma(
            Texture2D TextureReference,
            Rectangle TextureRectangle)
        {
            texture_reference_ = TextureReference;
            texture_rectangle_ = TextureRectangle;
            
            collision_rectangle_ = new Rectangle(
                   0, 0,
                   TextureRectangle.Width - 40,
                   TextureRectangle.Height - 40);

            active_ = false;
        }

        public Gemma(
           Texture2D TextureReference,
            Rectangle TextureRectangle,
            Vector2 starting_position)
            : this(TextureReference, TextureRectangle)
        {
            Position = starting_position;
        }

        

        
        public bool Active
        {
            get { return active_; }
            set { active_ = value; }
        }

        public override Vector2 Position
        {
            get { return position_; }
            set
            {
                
                collision_rectangle_.X = (int)value.X + 20;
                collision_rectangle_.Y = (int)value.Y + 20;
                position_ = value;
            }
        }

        public override Rectangle CollisionRectangle
        {
            get { return collision_rectangle_; }
        }

        public int Width
        {
            get { return texture_rectangle_.Width; }
        }

        public int Height
        {
            get { return texture_rectangle_.Height; }
        }


        

        
        public override void Update(double gameTime)
        {
            if (active_)
            {
                float elapsed = (float)gameTime / 1000.0f;

                if (elapsed_ >= 0.4f)
                {
                    move_y_amount_ *= -1;
                    elapsed_ = 0.0f;
                }

                position_ += new Vector2(0, move_y_amount_ * elapsed);

                elapsed_ += elapsed;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active_)
            {
                spriteBatch.Draw(
                    texture_reference_,
                    position_,
                    texture_rectangle_,
                    overlay_color_,
                    rotation_,
                    origin_,
                    scale_,
                    flip_,
                    depth_);
            }
        }

        
    }
}
