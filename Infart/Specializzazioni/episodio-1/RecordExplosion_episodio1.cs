#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Input.Touch;

#endregion
namespace fge
{
    public class RecordExplosion_episodio1 : RecordExplosion
    {
        public RecordExplosion_episodio1(Loader_episodio1 Loader)
            : base(
            Loader.textures_,
            Loader.textures_rectangles_["Record"])
        {
        }

    }
}
