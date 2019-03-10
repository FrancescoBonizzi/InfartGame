using Infart.Drawing;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Infart.Extensions
{
    public static class CollisionSolver
    {
        public enum Actions
        {
            BlockAll,
            BlockEnemies,
            BlockPlayer,
            EndLevel
        };

        public static bool CheckCollisions(
             Rectangle objRect,
             List<GameObject> listWith)
        {
            for (int i = 0; i < listWith.Count; ++i)
            {
                if (objRect.Intersects(listWith[i].CollisionRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        public static int CheckCollisionsReturnCollidedObject(
            Rectangle objRect,
             List<GameObject> listWith)
        {
            for (int i = 0; i < listWith.Count; ++i)
            {
                if (objRect.Intersects(listWith[i].CollisionRectangle))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}