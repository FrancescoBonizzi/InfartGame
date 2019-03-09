



namespace fge
{
    public class SoundManager_menu : SoundManager
    {
        public SoundManager_menu(bool SoundFlag, Loader_menu Loader_menu)
            : base(SoundFlag, Loader_menu, Loader_menu)
        {
        }

        // Non implemento Dispose perché é gestito automaticamente
        // dal content manager, in questo caso. Non mi interessa distruggere
        // tutto prima che il gioco venga chiuso

        protected override void AddAllSounds(Loader loader)
        {
            // Niente, è già fatto nel costruttore
        }

        protected override void AddNewGameSounds()
        {
            // Nessun suono da far partire automaticamente all'avvio
        }
    }
}
