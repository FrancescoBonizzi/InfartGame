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

#endregion

namespace fge
{
    public class SoundManager_menu : SoundManager
    {
        public SoundManager_menu(bool SoundFlag, Loader_menu Loader_menu)
            : base(SoundFlag, Loader_menu, Loader_menu)
        {
        }

        // Non implemento Dispose perch� � gestito automaticamente
        // dal content manager, in questo caso. Non mi interessa distruggere
        // tutto prima che il gioco venga chiuso

        protected override void AddAllSounds(Loader loader)
        {
            // Niente, � gi� fatto nel costruttore
        }

        protected override void AddNewGameSounds()
        {
            // Nessun suono da far partire automaticamente all'avvio
        }
    }
}
