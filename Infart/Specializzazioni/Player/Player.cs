#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace fge
{
    public abstract class Player : Actor
    {
        #region Dichiarazioni

        ScoreggiaParticleSystem scoreggia_system_;
        JalapenoParticleSystem jalapeno_system_;
        BroccoloParticleSystem broccolo_system_;

        // In ms
        const double timeBetweenNewParticleScoregge_ = 30.0f;
        double timeTillNewParticleScoregge_ = 0.0f;

        const double timeBetweenNewParticleJalapeno_ = 20.0f;
        double timeTillNewParticleJalapeno_ = 0.0f;

        const double timeBetweenNewParticleBroccolo_ = 80.0f;
        double timeTillNewParticleBroccolo_ = 0.0f;

        private static Random rand_;

        bool allow_input_ = true;
        bool jalapenos_ = false;
        int jalapenos_jump_count_ = 0;
        bool broccolo_ = false;
        private double elapsed_jalapenos_ = 0.0;
        private double elapsed_broccolo_ = 0.0;

        private Color fill_color_;

        #endregion

        #region Costruttore / Distruttore

        public Player(
           Vector2 starting_pos,
            GameManager GameManagerReference)
            : base(1.0f, starting_pos, GameManagerReference.GroundObjects())
        {
            fill_color_ = overlay_color_;
            Position = starting_pos;
    
            rand_ = fbonizziHelper.random;
        }

        public abstract void Reset(Vector2 position);

        #endregion

        protected void LoadAnimation(
            string name,
            List<Rectangle> frames,
            bool loop_animation,
            float frame_lenght,
            Texture2D TextureReference)
        {
            animations_.Add(
               name,
               new AnimationManager(
                    frames,
                    name,
                    TextureReference
               ));
            animations_[name].LoopAnimation = loop_animation;
            animations_[name].FrameLength = frame_lenght;
        }

        public virtual void Jump(float amount)
        {
            velocity_.Y = -amount;
        }

        #region Update/Draw

        public virtual void Update(double dt, TouchCollection touch)
        {
            base.Update(dt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (animations_.ContainsKey(current_animation_))
            {
                spriteBatch.Draw(
                    animations_[current_animation_].Texture,
                    position_,
                    animations_[current_animation_].FrameRectangle,
                    fill_color_,
                    rotation_,
                    origin_,
                    scale_,
                    flip_,
                    depth_);
            }
        }

        public abstract void DrawParticles(SpriteBatch spriteBatch);

        #endregion
    }
}