#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;
using System.Runtime.Serialization;

#endregion

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
