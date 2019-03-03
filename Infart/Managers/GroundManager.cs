#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace fge
{
    public abstract class GroundManager
    {
        protected Random random_;
        protected Camera current_camera_;

        public GroundManager(
            Camera CurrentCamera)
        {
            current_camera_ = CurrentCamera;
            random_ = fbonizziHelper.random;
        }

        public abstract List<GameObject> WalkableObjects();

        public abstract void Reset(Camera CameraReference);

        public abstract void Update(double gametime);

        public abstract void Draw(SpriteBatch spritebatch);
    }
}
