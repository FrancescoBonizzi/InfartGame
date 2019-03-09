
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace fge
{
    public class AbstractGameObject : GameObject
    {
        private Rectangle collision_rectangle_;

        public AbstractGameObject(Rectangle CollisionRectangle)
        {
            collision_rectangle_ = CollisionRectangle;
        }

        public override Rectangle CollisionRectangle
        {
            get { return collision_rectangle_; }
        }

        public void SetCollisionRectangleX(int x)
        {
            collision_rectangle_.X = x;
        }

        public override void Update(double gameTime)
        { }

        public override void Draw(SpriteBatch spriteBatch)
        { }
    }
}
