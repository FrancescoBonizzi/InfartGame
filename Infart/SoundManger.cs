using Infart.Assets;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infart
{
    public class SoundManager
    {
        private IDictionary<string, SoundEffectInstance> _allSounds;
        private List<SoundEffectInstance> _farts;

        public SoundManager(AssetsLoader assetsLoader)
        {
            _allSounds = new Dictionary<string, SoundEffectInstance>();
            _farts = new List<SoundEffectInstance>();

            foreach (var environmentalSound in assetsLoader.EnvironmentalSounds)
            {
                var soundEffect = environmentalSound.Value.CreateInstance();
                _allSounds.Add(environmentalSound.Key, soundEffect);
            }

            foreach (var fartSound in assetsLoader.FartsSounds)
            {
                var soundEffect = fartSound.Value.CreateInstance();
                _allSounds.Add(fartSound.Key, soundEffect);
                _farts.Add(soundEffect);
            }
        }

        public void PlayFart()
        {
            bool alreadyAFartIsPlaying = _farts.Any(f => f.State == SoundState.Playing);

            if (alreadyAFartIsPlaying)
                return;

            _farts[FbonizziMonoGame.Numbers.RandomBetween(0, _farts.Count)].Play();
        }

        public void StopFart()
        {
            foreach (var f in _farts)
            {
                if (f.State == SoundState.Playing)
                {
                    f.Stop();
                    break;
                }
            }
        }

        public void PlayShit()
        {
            _allSounds["truck"].Play();
        }

        public void PlayExplosion()
        {
            _allSounds["heartbeat"].Stop();
            _allSounds["truck"].Stop();
            StopFart();
            _allSounds["jalapeno"].Stop();
            _allSounds["explosion"].Play();
        }

        public void PlayFall()
        {
            if (_allSounds["fall"].State != SoundState.Playing)
            {
                _allSounds["heartbeat"].Stop();
                _allSounds["fall"].Play();
            }
        }

        public bool HasFallFinished()
        {
            return _allSounds["fall"].State == SoundState.Stopped;
        }

        public bool IsFallPlaying()
        {
            return _allSounds["fall"].State == SoundState.Playing;
        }

        public void PlayHeartBeat()
        {
            if (_allSounds["heartbeat"].State != SoundState.Playing)
                _allSounds["heartbeat"].Play();
        }

        public void StopHeartBeat()
        {
            _allSounds["heartbeat"].Stop();
        }

        public void PlayBite()
        {
            _allSounds["bite"].Play();
        }

        public void PlayJalapeno()
        {
            _allSounds["jalapeno"].Play();
        }

        public void PlayMenuBackground()
        {
            StopSounds();
#warning TODO PlayMenuBackground
        }

        public void PlayGameMusicBackground()
        {
            StopSounds();
#warning TODO PlayMenuBackground
        }

        public void StopSounds()
        {
            foreach (var s in _allSounds.Values)
            {
                s.Stop();
            }
        }

        public void PauseAll()
        {
            foreach (var s in _allSounds.Values)
            {
                if (s.State == SoundState.Playing)
                    s.Pause();
            }
        }

        public void ResumeAll()
        {
            foreach (var s in _allSounds.Values)
            {
                if (s.State == SoundState.Paused)
                    s.Play();
            }
        }

    }
}