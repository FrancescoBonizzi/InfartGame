using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.Drawing
{
    public abstract class GameObject
    {
        protected Vector2 position_ = Vector2.Zero;

        protected float rotation_ = 0.0f;

        protected Vector2 scale_ = Vector2.One;

        protected Vector2 origin_ = Vector2.Zero;

        protected float depth_ = 0.0f;

        protected Color overlay_color_ = Color.White;

        protected SpriteEffects flip_ = SpriteEffects.None;

        public virtual Vector2 Position
        {
            get { return position_; }
            set { position_ = value; }
        }

        public float PositionX
        {
            get { return position_.X; }
            set { position_.X = value; }
        }

        public abstract Rectangle CollisionRectangle { get; }

        public virtual Vector2 Scale
        {
            set { scale_ = value; }
            get { return scale_; }
        }

        public virtual float ScaleUnique
        {
            set
            {
                scale_.X = value;
                scale_.Y = value;
            }
            get { return scale_.X; }
        }

        public virtual float Depth
        {
            get { return depth_; }
            set { depth_ = value; }
        }

        public virtual float Rotation
        {
            get { return rotation_; }
            set { rotation_ = value; }
        }

        public Vector2 Origin
        {
            get { return origin_; }
            set { origin_ = value; }
        }

        public Matrix Transformation
        {
            get
            {
                return
                         Matrix.CreateTranslation(new Vector3(-origin_, 0.0f)) *
                         Matrix.CreateScale(scale_.X, scale_.Y, 1.0f) *
                         Matrix.CreateRotationZ(rotation_) *
                         Matrix.CreateTranslation(new Vector3(position_, 0.0f));
            }
        }

        public SpriteEffects FlipEffect
        {
            set { flip_ = value; }
            get { return flip_; }
        }

        public virtual Color FillColor
        {
            set { overlay_color_ = value; }
            get { return overlay_color_; }
        }

        public virtual void Dispose()
        {
        }

        public abstract void Update(double gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
