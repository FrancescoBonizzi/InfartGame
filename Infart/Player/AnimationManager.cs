using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace fge
{
    public class AnimationManager : IDisposable
    {
        private Texture2D atlas_reference_;

        private List<Rectangle> frames_;

        private float frame_timer_ = 0f;

        private float frame_delay_ = 0.05f;

        private int current_frame_;

        private int last_frame_;

        private bool loop_animation_ = true;

        private bool finished_playing_ = false;

        private string name_;

        private string next_animation_;

        public AnimationManager(
            List<Rectangle> frames,
            string name,
            Texture2D TextureReference)
        {
            name_ = name;

            frames_ = frames;
            atlas_reference_ = TextureReference;

            last_frame_ = frames.Count - 1;
        }

        public void Dispose()
        {
        }

        public Texture2D Texture
        {
            get { return atlas_reference_; }
        }

        public string Name
        {
            get { return name_; }
            set { name_ = value; }
        }

        public string NextAnimation
        {
            get { return next_animation_; }
            set { next_animation_ = value; }
        }

        public bool LoopAnimation
        {
            get { return loop_animation_; }
            set { loop_animation_ = value; }
        }

        public bool FinishedPlaying
        {
            get { return finished_playing_; }
        }

        public int CurrentFrame
        {
            get { return current_frame_; }
            set { current_frame_ = value % last_frame_; }
        }

        public int FrameWidth
        {
            get { return frames_[current_frame_].Width; }
        }

        public int FrameHeight
        {
            get { return frames_[current_frame_].Height; }
        }

        public int FrameCount
        {
            get { return last_frame_; }
        }

        public float FrameLength
        {
            get { return frame_delay_; }
            set { frame_delay_ = value; }
        }

        public Rectangle FrameRectangle
        {
            get { return frames_[current_frame_]; }
        }

        public void Play()
        {
            CurrentFrame = 0;
            finished_playing_ = false;
        }

        public void Update(double gameTime)
        {
            float elapsed = (float)gameTime / 1000.0f;
            frame_timer_ += elapsed;

            if (frame_timer_ >= frame_delay_)
            {
                ++CurrentFrame;


                if (current_frame_ >= FrameCount)
                {
                    if (loop_animation_)
                    {
                        CurrentFrame = 0;
                    }
                    else
                    {
                        CurrentFrame = last_frame_;
                        finished_playing_ = true;
                    }
                }

                frame_timer_ = 0f;
            }
        }
    }
}
