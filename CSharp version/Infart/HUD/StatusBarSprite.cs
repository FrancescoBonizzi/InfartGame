using Infart.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.HUD
{
    public class StatusBarSprite : GameObject
    {
        private double _elapsed = 0.0;
        private bool _animateIn = false;
        private bool _animateOut = false;
        private readonly Texture2D _textureReference;
        private readonly Rectangle _textureRectangle;
        private readonly Vector2 _deactivatedScale;
        private readonly Vector2 _activatedScale;
        private Vector2 _scaleTo;
        private readonly Vector2 _scaleChangeAmount = new Vector2(0.005f);

        public StatusBarSprite(
            Texture2D textureReference,
            Rectangle textureRectangle,
            Vector2 startingPosition)
        {
            _textureReference = textureReference;
            _textureRectangle = textureRectangle;
            base.Position = startingPosition;
        }

        public StatusBarSprite(
            Texture2D textureReference,
            Rectangle textureRectangle,
            Vector2 startingPosition,
            Color overlayColor,
            Vector2 scale)
            : this(textureReference, textureRectangle, startingPosition)
        {
            _overlayColor = overlayColor;
            _activatedScale = scale;
            _deactivatedScale = new Vector2(0.7f);
            _scale = _deactivatedScale;
            _origin = new Vector2(textureRectangle.Width / 2, textureRectangle.Height / 2);
        }

        public void Reset()
        {
            _scale = _deactivatedScale;
            _animateIn = false;
            _animateOut = false;
        }

        public override Vector2 Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }

        public override Rectangle CollisionRectangle
        {
            get { return Rectangle.Empty; }
        }

        public int Width
        {
            get { return _textureRectangle.Width; }
        }

        public int Height
        {
            get { return _textureRectangle.Height; }
        }

        public void Taken()
        {
            _scaleTo = _activatedScale;
            _animateIn = true;
            _animateOut = false;
        }

        public void Lost()
        {
            _scaleTo = _deactivatedScale;
            _animateIn = false;
            _animateOut = true;
        }

        public override void Update(double gameTime)
        {
            if (_animateIn || _animateOut)
            {
                if (_elapsed >= 0.0005)
                {
                    if (_animateIn)
                    {
                        if (_scale.X <= _scaleTo.X)
                            _scale += _scaleChangeAmount;
                        else
                            _animateIn = false;
                    }
                    else if (_animateOut)
                    {
                        if (_scale.X >= _scaleTo.X)
                            _scale -= _scaleChangeAmount;
                        else
                            _animateOut = false;
                    }

                    _elapsed = 0.0;
                }

                _elapsed += gameTime / 1000.0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _textureReference,
                base.Position,
                _textureRectangle,
                _overlayColor,
                _rotation,
                _origin,
                _scale,
                _flip,
                _depth);
        }
    }
}