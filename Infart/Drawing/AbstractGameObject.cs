using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.Drawing
{
    public class AbstractGameObject : GameObject
    {
        private Rectangle _collisionRectangle;

        public AbstractGameObject(Rectangle collisionRectangle)
        {
            _collisionRectangle = collisionRectangle;
        }

        public override Rectangle CollisionRectangle
        {
            get { return _collisionRectangle; }
        }

        public void SetCollisionRectangleX(int x)
        {
            _collisionRectangle.X = x;
        }

        public override void Update(double gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}