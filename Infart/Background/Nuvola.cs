using Infart.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.Background
{
    public class Nuvola : GameObject
    {
        private float velocity_;

        private Vector2 current_move_amount = Vector2.Zero;

        private Rectangle texture_rectangle_;

        private Texture2D texture_reference_;

        private float scale_float_amount_ = 0.05f;

        private float scale_elapsed_ = 0.0f;

        private bool active_ = false;

        public Nuvola(Texture2D TextureReference, Rectangle TextureRectangle)
        {
            texture_rectangle_ = TextureRectangle;
            texture_reference_ = TextureReference;
            origin_ = new Vector2(TextureRectangle.Width / 2, TextureRectangle.Height / 2);
            scale_ = Vector2.One;
        }

        public void Set(
            Vector2 pos,
            float speed,
            Color overlay_color,
            Vector2 scale)
        {
            this.position_ = pos;
            this.velocity_ = speed;
            this.overlay_color_ = overlay_color;
            this.scale_ = scale;

            active_ = true;
        }

        public bool Active
        {
            get { return active_; }
        }

        public override Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)position_.X, (int)position_.Y, texture_rectangle_.Width, texture_rectangle_.Height); }
        }

        public override void Update(double gameTime)
        {
            if (active_)
            {
                float elapsed = (float)gameTime / 1000.0f;

                position_ += new Vector2(velocity_ * elapsed, 0.0f);

                if (scale_elapsed_ >= 0.4f)
                {
                    scale_float_amount_ *= -1;
                    scale_elapsed_ = 0.0f;
                }
                scale_ += new Vector2(scale_float_amount_ * elapsed, scale_float_amount_ * elapsed);
                scale_elapsed_ += elapsed;
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
                    0.0f,
                    origin_,
                    scale_,
                    SpriteEffects.None,
                    1.0f);
            }
        }
    }
}
