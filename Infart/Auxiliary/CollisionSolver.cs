#region Descrizione
//-----------------------------------------------------------------------------
// CollisionSolver.cs
//
// fbonizzi_Game_Engine
// Copyright (C) Francesco Bonizzi. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

#endregion

namespace fge
{
    public static class CollisionSolver
    {
        #region Dichiarazioni

        public enum actions
        {
            blockAll,
            blockEnemies,
            blockPlayer,
            endLevel
        };

        #endregion

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