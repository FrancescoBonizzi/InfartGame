using Infart.Assets;
using Infart.Extensions;
using Infart.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Infart.Astronaut
{
    public class Player : Actor
    {
        private ScoreggiaParticleSystem scoreggia_system_;

        private JalapenoParticleSystem jalapeno_system_;

        private BroccoloParticleSystem broccolo_system_;

        private InfartGame game_manager_reference_;

        private const double timeBetweenNewParticleScoregge_ = 30.0f;

        private double timeTillNewParticleScoregge_ = 0.0f;

        private const double timeBetweenNewParticleJalapeno_ = 20.0f;

        private double timeTillNewParticleJalapeno_ = 0.0f;

        private const double timeBetweenNewParticleBroccolo_ = 80.0f;

        private double timeTillNewParticleBroccolo_ = 0.0f;

        private static Random rand_;

        private bool allow_input_ = true;

        private bool jalapenos_ = false;

        private int jalapenos_jump_count_ = 0;

        private bool broccolo_ = false;

        private double elapsed_jalapenos_ = 0.0;

        private double elapsed_broccolo_ = 0.0;

        private Color fill_color_;

        public Player(
            Vector2 starting_pos,
            AssetsLoader AssetsLoader,
            InfartGame GameManagerReference)
            : base(1.0f, starting_pos, GameManagerReference.GroundObjects())
        {
            fill_color_ = overlay_color_;
            Position = starting_pos;
            HorizontalMoveSpeed = 300f;
            Origin = Vector2.Zero;

            game_manager_reference_ = GameManagerReference;

            scoreggia_system_ = new ScoreggiaParticleSystem(10, AssetsLoader.Textures, AssetsLoader.TexturesRectangles["ScoreggiaParticle"]);
            jalapeno_system_ = new JalapenoParticleSystem(10, AssetsLoader);
            broccolo_system_ = new BroccoloParticleSystem(8, AssetsLoader);

            rand_ = fbonizziHelper.random;

            LoadAnimation("idle", AssetsLoader.PlayerIdleRects,
                true, 0.1f, AssetsLoader.Textures);

            LoadAnimation("run", AssetsLoader.PlayerRunRects,
                true, 0.05f, AssetsLoader.Textures);

            LoadAnimation("fall", AssetsLoader.PlayerFallRects,
                true, 0.01f, AssetsLoader.Textures);

            LoadAnimation("fart_sustain_up", AssetsLoader.PlayerFartRects,
                true, 0.05f, AssetsLoader.Textures);

            LoadAnimation("merdone", AssetsLoader.PlayerMerdaRects,
                true, 0.01f, AssetsLoader.Textures);

            PlayAnimation("fall");
        }

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

        public void Reset(Vector2 position)
        {
            position_ = position;
            Dead = false;
            jalapenos_ = false;
            jalapenos_jump_count_ = 0;
            broccolo_ = false;
            elapsed_jalapenos_ = 0.0;
            elapsed_broccolo_ = 0.0;
            fill_color_ = Color.White;
            HorizontalMoveSpeed = 300f;
            animations_["run"].FrameLength = 0.05f;
            velocity_ = Vector2.Zero;
            PlayAnimation("fall");
        }

        public bool JalapenosJump
        {
            get { return jalapenos_; }
        }

        public bool IsCulating
        {
            get { return current_animation_ == "culata"; }
        }

        public void Jump(float amount)
        {
            velocity_.Y = -amount;

            game_manager_reference_.PlayerJumped();
            game_manager_reference_.AddScoreggia();
        }

        public void IncreaseMoveSpeed()
        {
            x_move_speed_ += 40.0f;
            animations_["run"].FrameLength -= 0.005f;
        }

        public bool AllowInput
        {
            set { allow_input_ = value; }
        }

        public void ActivateJalapenos()
        {
            Jump(500);
            jalapenos_ = true;
            elapsed_jalapenos_ = 0.0;
            HorizontalMoveSpeed += 200.0f;
            fill_color_ = Color.DarkRed;
            game_manager_reference_.IncreaseParallaxSpeed();
            jalapenos_jump_count_ = 0;
        }

        public void DeactivateJalapenos()
        {
            elapsed_jalapenos_ = 0.0;
            jalapenos_ = false;
            game_manager_reference_.jalapenos_mode_active_ = false;
            HorizontalMoveSpeed -= 200.0f;
            fill_color_ = overlay_color_;
            game_manager_reference_.DecreaseParallaxSpeed();
        }

        public void ActivateBroccolo()
        {
            elapsed_broccolo_ = 0.0;
            fill_color_ = Color.Brown;
            broccolo_ = true;
            Jump(200);
            HorizontalMoveSpeed += 400.0f;

            game_manager_reference_.IncreaseParallaxSpeed();
            game_manager_reference_.IncreaseParallaxSpeed();
        }

        public void DeactivateBroccolo()
        {
            broccolo_ = false;
            game_manager_reference_.merda_mode_active_ = false;
            elapsed_broccolo_ = 0.0;
            HorizontalMoveSpeed -= 400.0f;
            fill_color_ = overlay_color_;
            game_manager_reference_.DecreaseParallaxSpeed();
            game_manager_reference_.DecreaseParallaxSpeed();
        }

        public void HandleInput()
        {
            if (jalapenos_)
            {
                if (jalapenos_jump_count_ < 1)
                {
                    ++jalapenos_jump_count_;
                    Jump(800);
                }
            }
            else if (broccolo_)
                Jump(500);
            else
            {
                if (OnGround)
                    Jump(650);
            }
        }

        public override void Update(double dt)
        {
            if (!dead_)
            {
                string new_animation_ = "run";

                if (allow_input_)
                {
                    FlipEffect = SpriteEffects.None;
                    velocity_.X = +HorizontalMoveSpeed;
                }
                else
                {
                    new_animation_ = "idle";
                }

                if (!OnGround)
                {
                    if (velocity_.Y < 0)
                    {
                        new_animation_ = "fart_sustain_up";
                    }
                    else if (current_animation_ == "fart_sustain_up" || current_animation_ == "fall")
                    {
                        new_animation_ = "fall";
                    }
                }

                if (broccolo_)
                    new_animation_ = "merdone";

                if (new_animation_ != current_animation_)
                {
                    PlayAnimation(new_animation_);
                    if (new_animation_ == "merdone")
                        collision_rectangle_.Width -= 120;
                }

                scoreggia_system_.Update(dt);
                jalapeno_system_.Update(dt);
                broccolo_system_.Update(dt);

                if (jalapenos_ && !broccolo_)
                    JalapenoGeneration(dt);
                else if (!jalapenos_ && broccolo_)
                    BroccoloGeneration(dt);
                else
                    ScoreggiaGeneration(dt);

                if (!Dead)
                {
                    game_manager_reference_.score_metri_ = ((int)(position_.X / 100));
                }
            }

            base.Update(dt);
        }

        private void JalapenoGeneration(double dt)
        {
            if (OnGround)
                jalapenos_jump_count_ = 0;

            elapsed_jalapenos_ += dt;
            if (elapsed_jalapenos_ >= game_manager_reference_.PeperoncinoDuration)
                DeactivateJalapenos();

            timeTillNewParticleJalapeno_ -= dt;
            if (timeTillNewParticleJalapeno_ < 0)
            {
                Vector2 where = position_ + new Vector2(
                   animations_[current_animation_].FrameWidth / 3,
                   (animations_[current_animation_].FrameHeight / 2) + 30);

                jalapeno_system_.AddParticles(where);

                timeTillNewParticleJalapeno_ = timeBetweenNewParticleJalapeno_;
            }
        }

        private void BroccoloGeneration(double dt)
        {
            elapsed_broccolo_ += dt;
            if (elapsed_broccolo_ >= game_manager_reference_.BroccoloDuration)
                DeactivateBroccolo();

            timeTillNewParticleBroccolo_ -= dt;
            if (timeTillNewParticleBroccolo_ < 0)
            {
                Vector2 where = position_ + new Vector2(
                   animations_[current_animation_].FrameWidth / 3,
                   (animations_[current_animation_].FrameHeight / 2) + 30);

                broccolo_system_.AddParticles(where);

                timeTillNewParticleBroccolo_ = timeBetweenNewParticleBroccolo_;
            }
        }

        private void ScoreggiaGeneration(double dt)
        {
            if (velocity_.Y < 0)
            {
                timeTillNewParticleScoregge_ -= dt;
                if (timeTillNewParticleScoregge_ < 0)
                {
                    Vector2 where = position_ + new Vector2(
                       animations_[current_animation_].FrameWidth / 3,
                       (animations_[current_animation_].FrameHeight / 2) + 30);

                    scoreggia_system_.AddParticles(where);

                    timeTillNewParticleScoregge_ = timeBetweenNewParticleScoregge_;
                }
            }
            else
                game_manager_reference_.StopScoreggia();
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

        public void DrawParticles(SpriteBatch spriteBatch)
        {
            if (!dead_)
            {
                scoreggia_system_.Draw(spriteBatch);
                jalapeno_system_.Draw(spriteBatch);
                broccolo_system_.Draw(spriteBatch);
            }
        }
    }
}