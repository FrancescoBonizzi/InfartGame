using Infart.Drawing;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Infart.Extensions
{
    public static class CollisionSolver
    {
        public enum actions
        {
            blockAll,
            blockEnemies,
            blockPlayer,
            endLevel
        };

        public static bool CheckCollisions(
             Rectangle obj_rect,
             List<GameObject> ListWith)
        {
            for (int i = 0; i < ListWith.Count; ++i)
            {
                if (obj_rect.Intersects(ListWith[i].CollisionRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        public static int CheckCollisionsReturnCollidedObject(
            Rectangle obj_rect,
             List<GameObject> ListWith)
        {
            for (int i = 0; i < ListWith.Count; ++i)
            {
                if (obj_rect.Intersects(ListWith[i].CollisionRectangle))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}