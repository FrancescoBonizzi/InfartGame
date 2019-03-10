using Infart.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.Background
{
    public class Nuvola : GameObject
    {
        private float _velocity;

        private Vector2 _currentMoveAmount = Vector2.Zero;

        private readonly Rectangle _textureRectangle;

        private readonly Texture2D _textureReference;

        private float _scaleFloatAmount = 0.05f;

        private float _scaleElapsed = 0.0f;

        private bool _active = false;

        public Nuvola(Texture2D textureReference, Rectangle textureRectangle)
        {
            _textureRectangle = textureRectangle;
            _textureReference = textureReference;
            Origin = new Vector2(textureRectangle.Width / 2, textureRectangle.Height / 2);
            Scale = Vector2.One;
        }

        public void Set(
            Vector2 pos,
            float speed,
            Color overlayColor,
            Vector2 scale)
        {
            this.Position = pos;
            this._velocity = speed;
            this.OverlayColor = overlayColor;
            this.Scale = scale;

            _active = true;
        }

        public bool Active
        {
            get { return _active; }
        }

        public override Rectangle CollisionRectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, _textureRectangle.Width, _textureRectangle.Height); }
        }

        public override void Update(double gameTime)
        {
            if (_active)
            {
                float elapsed = (float)gameTime / 1000.0f;

                Position += new Vector2(_velocity * elapsed, 0.0f);

                if (_scaleElapsed >= 0.4f)
                {
                    _scaleFloatAmount *= -1;
                    _scaleElapsed = 0.0f;
                }
                Scale += new Vector2(_scaleFloatAmount * elapsed, _scaleFloatAmount * elapsed);
                _scaleElapsed += elapsed;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_active)
            {
                spriteBatch.Draw(
                    _textureReference,
                    Position,
                    _textureRectangle,
                    OverlayColor,
                    0.0f,
                    Origin,
                    Scale,
                    SpriteEffects.None,
                    1.0f);
            }
        }
    }
}