using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Infart.Astronaut
{
    public class AnimationManager : IDisposable
    {
        private readonly Texture2D _atlasReference;

        private readonly List<Rectangle> _frames;

        private float _frameTimer = 0f;

        private float _frameDelay = 0.05f;

        private int _currentFrame;

        private readonly int _lastFrame;

        private bool _loopAnimation = true;

        private bool _finishedPlaying = false;

        private string _name;

        private string _nextAnimation;

        public AnimationManager(
            List<Rectangle> frames,
            string name,
            Texture2D textureReference)
        {
            _name = name;

            _frames = frames;
            _atlasReference = textureReference;

            _lastFrame = frames.Count - 1;
        }

        public void Dispose()
        {
        }

        public Texture2D Texture
        {
            get { return _atlasReference; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string NextAnimation
        {
            get { return _nextAnimation; }
            set { _nextAnimation = value; }
        }

        public bool LoopAnimation
        {
            get { return _loopAnimation; }
            set { _loopAnimation = value; }
        }

        public bool FinishedPlaying
        {
            get { return _finishedPlaying; }
        }

        public int CurrentFrame
        {
            get { return _currentFrame; }
            set { _currentFrame = value % _lastFrame; }
        }

        public int FrameWidth
        {
            get { return _frames[_currentFrame].Width; }
        }

        public int FrameHeight
        {
            get { return _frames[_currentFrame].Height; }
        }

        public int FrameCount
        {
            get { return _lastFrame; }
        }

        public float FrameLength
        {
            get { return _frameDelay; }
            set { _frameDelay = value; }
        }

        public Rectangle FrameRectangle
        {
            get { return _frames[_currentFrame]; }
        }

        public void Play()
        {
            CurrentFrame = 0;
            _finishedPlaying = false;
        }

        public void Update(double gameTime)
        {
            float elapsed = (float)gameTime / 1000.0f;
            _frameTimer += elapsed;

            if (_frameTimer >= _frameDelay)
            {
                ++CurrentFrame;

                if (_currentFrame >= FrameCount)
                {
                    if (_loopAnimation)
                    {
                        CurrentFrame = 0;
                    }
                    else
                    {
                        CurrentFrame = _lastFrame;
                        _finishedPlaying = true;
                    }
                }

                _frameTimer = 0f;
            }
        }
    }
}