#region Using

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

// TODO
// Texture reference e texture rectangle DEVONO finire nel gameobject

namespace fge
{
    public class StatusBarSprite : GameObject
    {
        #region Dichiarazioni

        private double elapsed_ = 0.0;
        private bool animate_in_ = false;
        private bool animate_out_ = false;

        private Texture2D texture_reference_;
        private Rectangle texture_rectangle_;

        private Vector2 deactivated_scale_;
        private Vector2 activated_scale_;
        private Vector2 scaleTo_;
        private Vector2 scale_change_amount = new Vector2(0.005f);

        #endregion

        #region Costruttore / Distruttore

        public StatusBarSprite(
            Texture2D TextureReference,
            Rectangle TextureRectangle,
            Vector2 StartingPosition)
        {
            texture_reference_ = TextureReference;
            texture_rectangle_ = TextureRectangle;
            position_ = StartingPosition;
        }

        public StatusBarSprite(
            Texture2D TextureReference,
            Rectangle TextureRectangle,
            Vector2 StartingPosition,
            Color overlay_color,
            Vector2 scale)
            : this(TextureReference, TextureRectangle, StartingPosition)
        {
            overlay_color_ = overlay_color;
            activated_scale_ = scale;
            deactivated_scale_ = new Vector2(0.7f);
            scale_ = deactivated_scale_;
            origin_ = new Vector2(TextureRectangle.Width / 2, TextureRectangle.Height / 2);
        }

        public void Reset()
        {
            scale_ = deactivated_scale_;
            animate_in_ = false;
            animate_out_ = false;
        }

        #endregion

        #region Proprietà

        public override Vector2 Position
        {
            get { return position_; }
            set { position_ = value; }
        }

        public override Rectangle CollisionRectangle
        {
            get { return Rectangle.Empty; }
        }

        public int Width
        {
            get { return texture_rectangle_.Width; }
        }

        public int Height
        {
            get { return texture_rectangle_.Height; }
        }

        public void Taken()
        {
            scaleTo_ = activated_scale_;
            animate_in_ = true;
            animate_out_ = false;
        }

        public void Lost()
        {
            scaleTo_ = deactivated_scale_;
            animate_in_ = false;
            animate_out_ = true;
        }

        #endregion

        #region Update / Draw

        public override void Update(double gameTime)
        {
            if (animate_in_ || animate_out_)
            {
                if (elapsed_ >= 0.0005)
                {
                    if (animate_in_)
                    {
                        if (scale_.X <= scaleTo_.X)
                            scale_ += scale_change_amount;
                        else
                            animate_in_ = false;
                    }
                    else if (animate_out_)
                    {
                        if (scale_.X >= scaleTo_.X)
                            scale_ -= scale_change_amount;
                        else
                            animate_out_ = false;
                    }

                    elapsed_ = 0.0;
                }

                elapsed_ += gameTime / 1000.0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
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

        #endregion
    }
}
