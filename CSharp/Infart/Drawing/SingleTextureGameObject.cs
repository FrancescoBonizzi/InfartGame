using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.Drawing
{
    public class SingleTextureGameObject : GameObject
    {
        private readonly Rectangle _textureBounds;

        public override Rectangle CollisionRectangle
        {
            get => _collisionRectangle;
        }

        public SingleTextureGameObject(Texture2D texture)
        {
            Texture = texture;
            Width = texture.Width;
            Height = texture.Height;
            _collisionRectangle = new Rectangle(0, 0, Width, Height);
            _textureBounds = Texture.Bounds;
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
            _overlayColor = overlayColor;
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
            _overlayColor = fillColor;
            _origin = origin;
            _scale = scale;
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


        public int Width { get; }

        public int Height { get; }

        public Texture2D Texture { get; }

        public Color[] PixelArray
        {
            get
            {
                Color[] pixels = new Color[Width * Height];
                Texture.GetData(
                    0,
                    new Rectangle(
                    0,
                    0,
                    Width,
                    Height),
                pixels,
                0,
                Width * Height);

                return pixels;
            }
        }

        public override void Update(double gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                base.Position,
                _textureBounds,
                _overlayColor,
                _rotation,
                _origin,
                _scale,
                _flip,
                _depth);
        }
    }
}