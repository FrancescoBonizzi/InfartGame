using Infart.Assets;
using Infart.Extensions;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Infart
{
    public class SoundManager
    {
        protected List<SoundEffectInstance> newgame_sounds_;

        protected List<SoundEffectInstance> all_sounds_;

        private List<SoundEffectInstance> scoreggia_sound_;

        protected bool sound_on_ = true;

        private SoundEffectInstance background_music_;

        private SoundEffectInstance explosion_sound_;

        private SoundEffectInstance fall_sound_;

        private SoundEffectInstance heart_beat_;

        private SoundEffectInstance jalapeno_sound_;

        private SoundEffectInstance merdone_sound_;

        private SoundEffectInstance morso_ = null;

        public SoundManager(
            bool SoundFlag,
            // Loader_menu Loader_menu,
            Loader Loader_episodio1)
        {
            sound_on_ = SoundFlag;

            //       LoadScoregge(Loader_menu);
            AddAllSounds(Loader_episodio1);
            AddNewGameSounds();
        }

        //private void LoadScoregge(Loader_menu scoregge_loader)
        //{
        //    scoreggia_sound_ = new List<SoundEffectInstance>();
        //    all_sounds_ = new List<SoundEffectInstance>();
        //    foreach (SoundEffect s in scoregge_loader.sound_scoregge_)
        //    {
        //        SoundEffectInstance tmp = s.CreateInstance();
        //        scoreggia_sound_.Add(tmp);
        //        all_sounds_.Add(tmp);
        //    }
        //}
        public void NewGame()
        {
            if (sound_on_)
            {
                foreach (SoundEffectInstance s in newgame_sounds_)
                    s.Play();
            }
        }

        public void StopSounds()
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

        public void SoundOff()
        {
            StopSounds();
            sound_on_ = false;
        }

        public void SoundOn()
        {
            sound_on_ = true;
        }

        protected void AddAllSounds(Loader MyLoader)
        {
            background_music_ = ((MyLoader) as Loader).sound_effects_["night"].CreateInstance();
            background_music_.IsLooped = true;
            all_sounds_.Add(background_music_);

            explosion_sound_ = ((MyLoader) as Loader).sound_effects_["esplosione"].CreateInstance();
            all_sounds_.Add(explosion_sound_);

            fall_sound_ = ((MyLoader) as Loader).sound_effects_["fall"].CreateInstance();
            all_sounds_.Add(fall_sound_);

            heart_beat_ = ((MyLoader) as Loader).sound_effects_["cuore"].CreateInstance();
            all_sounds_.Add(heart_beat_);

            morso_ = ((MyLoader) as Loader).sound_effects_["morso"].CreateInstance();
            all_sounds_.Add(morso_);

            jalapeno_sound_ = ((MyLoader) as Loader).sound_effects_["jalapeno"].CreateInstance();
            all_sounds_.Add(jalapeno_sound_);

            merdone_sound_ = ((MyLoader) as Loader).sound_effects_["merdone"].CreateInstance();
            all_sounds_.Add(merdone_sound_);
        }

        protected void AddNewGameSounds()
        {
            newgame_sounds_ = new List<SoundEffectInstance>();
            newgame_sounds_.Add(background_music_);
        }

        public void PlayShit()
        {
            if (sound_on_)
            {
                merdone_sound_.Play();
            }
        }

        public void PlayExplosion()
        {
            if (sound_on_)
            {
                heart_beat_.Stop();
                merdone_sound_.Stop();
                StopScoreggia();
                jalapeno_sound_.Stop();
                explosion_sound_.Play();
            }
        }

        public void PlayFall()
        {
            if (sound_on_)
            {
                if (fall_sound_.State != SoundState.Playing)
                {
                    heart_beat_.Stop();
                    fall_sound_.Play();
                }
            }
        }

        public bool HasFallFinished()
        {
            return fall_sound_.State == SoundState.Stopped;
        }

        public bool IsFallPlaying()
        {
            return fall_sound_.State == SoundState.Playing;
        }

        public void PlayHeartBeat()
        {
            if (sound_on_)
            {
                if (heart_beat_.State != SoundState.Playing)
                    heart_beat_.Play();
            }
        }

        public void StopHeartBeat()
        {
            heart_beat_.Stop();
        }

        public void PlayMorso()
        {
            if (sound_on_)
            {
                morso_.Play();
            }
        }

        public void PlayJalapeno()
        {
            if (sound_on_)
            {
                jalapeno_sound_.Play();
            }
        }
    }
}