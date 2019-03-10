using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.Drawing
{
    public class SingleTextureGameObject : GameObject
    {
        private readonly Texture2D _texture;

        private readonly int _width;

        private readonly int _height;

        private readonly Rectangle _textureBounds;

        private Rectangle _collisionRectangle;


        public override Rectangle CollisionRectangle
        {
            get => _collisionRectangle;
        }

        public SingleTextureGameObject(Texture2D texture)
        {
            _texture = texture;
            _width = texture.Width;
            _height = texture.Height;
            _collisionRectangle = new Rectangle(0, 0, _width, _height);
            _textureBounds = _texture.Bounds;
        }

        public SingleTextureGameObject(
            Texture2D texture,
            Vector2 startingPosition)
            : this(texture)
        {
            base.Position = startingPosition;
            _collisionRectangle.X = (int)startingPosition.X;
            _collisionRectangle.Y = (int)startingPosition.Y;
        }

        public SingleTextureGameObject(
            Texture2D texture,
            Vector2 startingPosition,
            Color overlayColor)
            : this(texture, startingPosition)
        {
            OverlayColor = overlayColor;
        }

        public SingleTextureGameObject(
            Texture2D texture,
            Vector2 startingPosition,
            Color fillColor,
            Vector2 origin,
            Vector2 scale
            )
            : this(texture, startingPosition)
        {
            OverlayColor = fillColor;
            Origin = origin;
            Scale = scale;
        }

        public override void Dispose()
        {
        }

        public override Vector2 Position
        {
            get { return base.Position; }
            set
            {
                _collisionRectangle.X = (int)value.X;
                _collisionRectangle.Y = (int)value.Y;
                base.Position = value;
            }
        }


        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public Color[] PixelArray
        {
            get
            {
                Color[] pixels = new Color[_width * _height];
                _texture.GetData(
                    0,
                    new Rectangle(
                    0,
                    0,
                    _width,
                    _height),
                pixels,
                0,
                _width * _height);

                return pixels;
            }
        }

        public override void Update(double gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                base.Position,
                _textureBounds,
                OverlayColor,
                Rotation,
                Origin,
                Scale,
                Flip,
                Depth);
        }
    }
}