
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;



namespace fge
{
    public class Loader_menu : Loader
    {
        public List<SoundEffect> sound_scoregge_;
        public Texture2D merda_;

        public Loader_menu(
            ContentManager Content)
            : base(Content)
        {
        }

        public override void LoadTexture()
        {
          
        }

        public override void LoadMusic()
        {
            string folder = Path.Combine("Music", "Scoregge");
            sound_scoregge_ = new List<SoundEffect>();
            for (int i = 1; i <= 7; ++i)
                sound_scoregge_.Add(content_.Load<SoundEffect>(Path.Combine(folder, "fart" + i)));

            folder = Path.Combine("Music", "Effects");
        }
    }
}
