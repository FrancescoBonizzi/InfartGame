#region Using

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace fge
{
    public class SingleTextureGameObject : GameObject
    {
        #region Dichiarazioni

        private Texture2D texture_;

        private int width_;
        private int height_;

        private Rectangle texture_bounds_;
        protected Rectangle collision_rectangle_;

        #endregion

        #region Costruttore / Distruttore

        public SingleTextureGameObject(Texture2D texture)
        {
            texture_ = texture;
            width_ = texture.Width;
            height_ = texture.Height;
            collision_rectangle_ = new Rectangle(0, 0, width_, height_);
            texture_bounds_ = texture_.Bounds;
        }

        public SingleTextureGameObject(
            Texture2D texture,
            Vector2 starting_position)
            : this(texture)
        {
            position_ = starting_position;
            collision_rectangle_.X = (int)starting_position.X;
            collision_rectangle_.Y = (int)starting_position.Y;
        }

        public SingleTextureGameObject(
            Texture2D texture,
            Vector2 starting_position,
            Color overlay_color)
            : this(texture, starting_position)
        {
            overlay_color_ = overlay_color;
        }

        public SingleTextureGameObject(
            Texture2D texture,
            Vector2 starting_position,
            Color fill_color,
            Vector2 origin,
            Vector2 scale
            )
            : this(texture, starting_position)
        {
            overlay_color_ = fill_color;
            origin_ = origin;
            scale_ = scale;
        }

        public override void Dispose()
        {
        }

        #endregion

        #region Proprietà

        public override Vector2 Position
        {
            get { return position_; }
            set
            {
                collision_rectangle_.X = (int)value.X;
                collision_rectangle_.Y = (int)value.Y;
                position_ = value;
            }
        }

        public override Rectangle CollisionRectangle
        {
            get { return collision_rectangle_; }
        }

        public int Width
        {
            get { return width_; }
        }

        public int Height
        {
            get { return height_; }
        }

        public Texture2D Texture
        {
            get { return texture_; }
        }

        public Color[] PixelArray
        {
            get
            {
                Color[] pixels = new Color[width_ * height_];
                texture_.GetData(
                    0,
                    new Rectangle(
                    0,
                    0,
                    width_,
                    height_),
                pixels,
                0,
                width_ * height_);

                return pixels;
            }
        }

        #endregion

        #region Update/Draw

        public override void Update(double gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture_,
                position_,
                texture_bounds_,
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
