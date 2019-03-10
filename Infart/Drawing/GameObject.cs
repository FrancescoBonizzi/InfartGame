using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.Drawing
{
    public abstract class GameObject
    {
        private Vector2 _position = Vector2.Zero;

        public virtual Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        protected float Rotation = 0.0f;

        protected Vector2 Scale = Vector2.One;

        protected Vector2 Origin = Vector2.Zero;

        protected float Depth = 0.0f;

        protected Color OverlayColor = Color.White;

        protected SpriteEffects Flip = SpriteEffects.None;
        

        public float PositionX
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        protected Rectangle _collisionRectangle;
        public virtual Rectangle CollisionRectangle
        {
            get => _collisionRectangle;
        }
        
        public virtual float ScaleUnique
        {
            set
            {
                Scale.X = value;
                Scale.Y = value;
            }
            get { return Scale.X; }
        }
        

        public Matrix Transformation
        {
            get
            {
                return
                         Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                         Matrix.CreateScale(Scale.X, Scale.Y, 1.0f) *
                         Matrix.CreateRotationZ(Rotation) *
                         Matrix.CreateTranslation(new Vector3(Position, 0.0f));
            }
        }

        public SpriteEffects FlipEffect
        {
            set { Flip = value; }
            get { return Flip; }
        }

        public virtual Color FillColor
        {
            set { OverlayColor = value; }
            get { return OverlayColor; }
        }

        public virtual void Dispose()
        {
        }

        public abstract void Update(double gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}