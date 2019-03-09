
using Microsoft.Xna.Framework.Content;



namespace fge
{
    // Serve, perché così poi posso instanziarla per ogni livello e non in generale
    // Le animazioni dell'omino e il suono delle scoregge le posso caricare con il loading normale...
    // dovrò poi trovare un modo di collegare insieme tutto
    public abstract class Loader
    {
        protected ContentManager content_;
 
        private bool music_loaded_ = false;
        private bool texture_loaded_ = false;

        public Loader(
            ContentManager content)
        {
            content_ = content;

            TextureLoader();
            MusicLoader();
        }

       
        private void TextureLoader()
        {
            if (!texture_loaded_)
            {
                LoadTexture();
                texture_loaded_ = true;
            }
        }

        private void MusicLoader()
        {
            if (!music_loaded_)
            {
                LoadMusic();
                music_loaded_ = true;
            }
        }

        public abstract void LoadTexture();
        public abstract void LoadMusic();
    }
}
