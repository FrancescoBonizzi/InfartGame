using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.ParticleSystem
{
    public class ParticleExplosion
    {
        private readonly Texture2D _texture;
        private readonly Rectangle _textureRectangle;
        private readonly Vector2 _origin;

        public Vector2 Position { get; set; }

        private Vector2 _velocity;
        private float _angle;
        private float _angularVelocity;
        private Color _color;
        private int _ttl;
        private bool _fading = false;
        private bool _active = false;

        public ParticleExplosion(
            Texture2D texture,
            Rectangle textureRectangle,
            Vector2 position,
            Vector2 velocity,
            float angle,
            float angularVelocity,
            Color color,
            float size,
            int ttl)
        {
            _texture = texture;
            _textureRectangle = textureRectangle;
            Position = position;
            _velocity = velocity;
            _angle = angle;
            _angularVelocity = angularVelocity;
            _color = color;
            Scale = size;
            _ttl = ttl;

            _fading = false;
            _active = true;

            _origin = new Vector2(_textureRectangle.Width / 2, _textureRectangle.Height / 2);
        }

        public void Initialize(
             Vector2 position,
             Vector2 velocity,
             float angle,
             float angularVelocity,
             Color color,
             float size,
             int ttl)
        {
            Position = position;
            _velocity = velocity;
            _angle = angle;
            _angularVelocity = angularVelocity;
            _color = color;
            Scale = size;
            _ttl = ttl;

            _active = true;
            _fading = false;
        }

        public void Refactor(
            Vector2 velocity,
            float angle,
            float angularVelocity,
            Color color,
            float size,
            int ttl)
        {
            _velocity = velocity;
            _angle = angle;
            _angularVelocity = angularVelocity;
            _color = color;
            Scale = size;
            _ttl = ttl;

            _fading = false;
            _active = true;
        }

        public float Scale { get; set; }

        public bool IsDead
        {
            get { return !_active; }
        }

        public void Fade()
        {
            _fading = true;
        }

        public void Update(double gameTime)
        {
            if (_active)
            {
                float elapsed = (float)gameTime / 1000.0f;

                --_ttl;
                Position += _velocity * elapsed;
                _angle += _angularVelocity * elapsed;

                if (!_fading && _ttl <= 0)
                {
                    _fading = true;
                }

                if (_fading)
                {
                    _color *= 0.95f;
                    if (_color.A <= 50)
                    {
                        _active = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_active)
            {
                spriteBatch.Draw(
                    _texture,
                    Position,
                    _textureRectangle,
                    _color,
                    _angle,
                    _origin,
                    Scale,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}