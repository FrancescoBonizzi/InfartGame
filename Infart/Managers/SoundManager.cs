
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;




namespace fge
{
    public abstract class SoundManager
    {
        
        // Riferimenti ai suoni per le funzioni built-in di questa classe astratta
        protected List<SoundEffectInstance> newgame_sounds_;
        protected List<SoundEffectInstance> all_sounds_;

        private List<SoundEffectInstance> scoreggia_sound_;

        protected bool sound_on_ = true;

        

        
        public SoundManager(bool SoundFlag, Loader_menu LoaderScoregge, Loader MyLoader)
        {
            sound_on_ = SoundFlag;

            LoadScoregge(LoaderScoregge);
            AddAllSounds(MyLoader);
            AddNewGameSounds();
        }

        

        
        // Suoni da aggiungere alla lista di quelli da far partire col new game
        // Non serve il loader perché è già tutto caricato, è solo un riferimento
        protected abstract void AddNewGameSounds();
        // Aggiunge i riferimenti ai suoni per gestire pause ecc
        protected abstract void AddAllSounds(Loader loader);

        private void LoadScoregge(Loader_menu scoregge_loader)
        {
            scoreggia_sound_ = new List<SoundEffectInstance>();
            all_sounds_ = new List<SoundEffectInstance>();
            foreach (SoundEffect s in scoregge_loader.sound_scoregge_)
            {
                SoundEffectInstance tmp = s.CreateInstance();
                scoreggia_sound_.Add(tmp);
                all_sounds_.Add(tmp);
            }
        }

        public void NewGame()
        {
            if (sound_on_)
            {
                foreach (SoundEffectInstance s in newgame_sounds_)
                    s.Play();
            }
        }

        public void SoundOff()
        {
            StopSounds();
            sound_on_ = false;
        }

        public void SoundOn()
        {
            sound_on_ = true;
        }

        public virtual void StopSounds()
        {
            foreach (SoundEffectInstance s in all_sounds_)
            {
                s.Stop();
            }
        }

        public void PauseAll()
        {
            foreach (SoundEffectInstance s in all_sounds_)
            {
                if (s.State == SoundState.Playing)
                    s.Pause();
            }
        }

        public void ResumeAll()
        {
            if (sound_on_)
            {
                foreach (SoundEffectInstance s in all_sounds_)
                {
                    if (s.State == SoundState.Paused)
                        s.Play();
                }
                NewGame();
            }
        }

        public void PlayScoreggia()
        {
            if (sound_on_)
            {
                bool one_playing = false;

                foreach (SoundEffectInstance s in scoreggia_sound_)
                {   
                    if (s.State == SoundState.Playing)
                    {
                        one_playing = true;
                    }
                }

                if (!one_playing)
                {
                    scoreggia_sound_[fbonizziHelper.random.Next(scoreggia_sound_.Count)].Play();
                }
            }
        }

        public void StopScoreggia()
        {
            foreach (SoundEffectInstance s in scoreggia_sound_)
            {
                if (s.State == SoundState.Playing)
                {
                    s.Stop();
                    break;
                }
            }
        }

        
    }
}