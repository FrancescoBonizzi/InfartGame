
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;






namespace fge
{
    public class Grattacielo : GameObject
    {
        
        private int h_; 
        private int one_block_height_ = 25;

        int w_grattacielo_;

        private Rectangle collision_rectangle_;
        private Rectangle texture_rectangle_;

        private Texture2D texture_reference_;

        

        
        
        public Grattacielo(Rectangle TextureRectangle, Texture2D TextureReference)
        {
            Position = Vector2.Zero;

            w_grattacielo_ = TextureRectangle.Width;
            h_ = TextureRectangle.Height / one_block_height_;
            origin_ = new Vector2(0, TextureRectangle.Height);
            collision_rectangle_.Width = TextureRectangle.Width;
            collision_rectangle_.Height = TextureRectangle.Height;

            texture_reference_ = TextureReference;
            texture_rectangle_ = TextureRectangle;
        }

        

        
        public void Move(Vector2 amount)
        {
            position_ += amount;
        }

        

        
        public override Rectangle CollisionRectangle
        {
            get
            {
                return collision_rectangle_;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                collision_rectangle_.X = (int)value.X;
                collision_rectangle_.Y = (int)(value.Y - Origin.Y);
                base.Position = value;
            }
        }

        public int Width
        {
            get { return texture_rectangle_.Width; }
        }

        public int Height
        {
            get { return texture_rectangle_.Height; }
        }

        public int HeightInBlocksNumber
        {
            get { return h_; }
        }

        public Vector2 PositionAtTopLeftCorner()
        {
            return new Vector2(
                Position.X,
                Position.Y - Origin.Y);
        }

        

        
        public override void Update(double gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture_reference_,
                position_,
                texture_rectangle_,
                Color.White,
                0.0f,
                origin_,
                scale_,
                flip_,
                depth_);
        }

        
    }
}
