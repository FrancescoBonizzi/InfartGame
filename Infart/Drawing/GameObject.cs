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

        protected float _rotation = 0.0f;
        protected Vector2 _scale = Vector2.One;
        protected Vector2 _origin = Vector2.Zero;
        protected float _depth = 0.0f;
        protected Color _overlayColor = Color.White;
        protected SpriteEffects _flip = SpriteEffects.None;

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
                _scale.X = value;
                _scale.Y = value;
            }
            get { return _scale.X; }
        }
        

        public Matrix Transformation
        {
            get
            {
                return
                         Matrix.CreateTranslation(new Vector3(-_origin, 0.0f)) *
                         Matrix.CreateScale(_scale.X, _scale.Y, 1.0f) *
                         Matrix.CreateRotationZ(_rotation) *
                         Matrix.CreateTranslation(new Vector3(Position, 0.0f));
            }
        }

        public SpriteEffects FlipEffect
        {
            set { _flip = value; }
            get { return _flip; }
        }

        public virtual Color FillColor
        {
            set { _overlayColor = value; }
            get { return _overlayColor; }
        }

        public virtual void Dispose()
        {
        }

        public abstract void Update(double gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}