using Infart.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.Background
{
    public class Grattacielo : GameObject
    {
        private readonly int _h;

        private readonly int _oneBlockHeight = 25;

        private readonly int _wGrattacielo;

        private Rectangle _collisionRectangle;

        private readonly Rectangle _textureRectangle;

        private readonly Texture2D _textureReference;

        public Grattacielo(Rectangle textureRectangle, Texture2D textureReference)
        {
            Position = Vector2.Zero;

            _wGrattacielo = textureRectangle.Width;
            _h = textureRectangle.Height / _oneBlockHeight;
            Origin = new Vector2(0, textureRectangle.Height);
            _collisionRectangle.Width = textureRectangle.Width;
            _collisionRectangle.Height = textureRectangle.Height;

            _textureReference = textureReference;
            _textureRectangle = textureRectangle;
        }

        public void Move(Vector2 amount)
        {
            base.Position += amount;
        }

        public override Rectangle CollisionRectangle
        {
            get
            {
                return _collisionRectangle;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                _collisionRectangle.X = (int)value.X;
                _collisionRectangle.Y = (int)(value.Y - Origin.Y);
                base.Position = value;
            }
        }

        public int Width
        {
            get { return _textureRectangle.Width; }
        }

        public int Height
        {
            get { return _textureRectangle.Height; }
        }

        public int HeightInBlocksNumber
        {
            get { return _h; }
        }

        public Vector2 PositionAtTopLeftCorner()
        {
            return new Vector2(
                Position.X,
                Position.Y - Origin.Y);
        }

        public override void Update(double gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _textureReference,
                base.Position,
                _textureRectangle,
                Color.White,
                0.0f,
                Origin,
                Scale,
                Flip,
                Depth);
        }
    }
}