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
    public class SoundManager_episodio1 : SoundManager
    {
        #region Dichiarazioni

        private SoundEffectInstance background_music_;
        private SoundEffectInstance explosion_sound_;
        private SoundEffectInstance fall_sound_;
        private SoundEffectInstance heart_beat_;
        private SoundEffectInstance jalapeno_sound_;
        private SoundEffectInstance merdone_sound_;
        private SoundEffectInstance morso_ = null;

        #endregion

        #region Costruttore

        public SoundManager_episodio1(
            bool SoundFlag,
            Loader_menu Loader_menu,
            Loader_episodio1 Loader_episodio1)
            : base(SoundFlag, Loader_menu, Loader_episodio1)
        {
        }

        #endregion

        #region Metodi

        protected override void AddAllSounds(Loader MyLoader)
        {
            background_music_ = ((MyLoader) as Loader_episodio1).sound_effects_["night"].CreateInstance();
            background_music_.IsLooped = true;
            all_sounds_.Add(background_music_);

            explosion_sound_ = ((MyLoader) as Loader_episodio1).sound_effects_["esplosione"].CreateInstance();
            all_sounds_.Add(explosion_sound_);
            
            fall_sound_ = ((MyLoader) as Loader_episodio1).sound_effects_["fall"].CreateInstance();
            all_sounds_.Add(fall_sound_);
            
            heart_beat_ = ((MyLoader) as Loader_episodio1).sound_effects_["cuore"].CreateInstance();
            all_sounds_.Add(heart_beat_);

            morso_ = ((MyLoader) as Loader_episodio1).sound_effects_["morso"].CreateInstance();
            all_sounds_.Add(morso_);
            
            jalapeno_sound_ = ((MyLoader) as Loader_episodio1).sound_effects_["jalapeno"].CreateInstance();
            all_sounds_.Add(jalapeno_sound_);

            merdone_sound_ = ((MyLoader) as Loader_episodio1).sound_effects_["merdone"].CreateInstance();
            all_sounds_.Add(merdone_sound_);
        }

        protected override void AddNewGameSounds()
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

        #endregion
    }
}
