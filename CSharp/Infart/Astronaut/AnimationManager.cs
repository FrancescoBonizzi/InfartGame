using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Infart.Astronaut
{
    public class AnimationManager
    {
        private readonly List<Rectangle> _frames;
        private float _frameTimer = 0f;
        private int _currentFrame;

        public Texture2D Texture { get; }
        public string Name { get; set; }
        public string NextAnimation { get; set; }
        public bool LoopAnimation { get; set; } = true;
        public bool FinishedPlaying { get; private set; } = false;

        public AnimationManager(
            List<Rectangle> frames,
            string name,
            Texture2D textureReference)
        {
            Name = name;

            _frames = frames;
            Texture = textureReference;

            FrameCount = frames.Count - 1;
        }
        
        public int CurrentFrame
        {
            get { return _currentFrame; }
            set { _currentFrame = value % FrameCount; }
        }

        public int FrameWidth
        {
            get { return _frames[_currentFrame].Width; }
        }

        public int FrameHeight
        {
            get { return _frames[_currentFrame].Height; }
        }

        public int FrameCount { get; }

        public float FrameLength { get; set; } = 0.05f;

        public Rectangle FrameRectangle
        {
            get { return _frames[_currentFrame]; }
        }

        public void Play()
        {
            CurrentFrame = 0;
            FinishedPlaying = false;
        }

        public void Update(double gameTime)
        {
            float elapsed = (float)gameTime / 1000.0f;
            _frameTimer += elapsed;

            if (_frameTimer >= FrameLength)
            {
                ++CurrentFrame;

                if (_currentFrame >= FrameCount)
                {
                    if (LoopAnimation)
                    {
                        CurrentFrame = 0;
                    }
                    else
                    {
                        CurrentFrame = FrameCount;
                        FinishedPlaying = true;
                    }
                }

                _frameTimer = 0f;
            }
        }
    }
}