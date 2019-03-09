
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;



namespace fge
{
    public class Player_episodio1 : Player
    {
        
        ScoreggiaParticleSystem scoreggia_system_;
        JalapenoParticleSystem jalapeno_system_;
        BroccoloParticleSystem broccolo_system_;

        InfartGame game_manager_reference_;

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

        

        
        public Player_episodio1(
           Vector2 starting_pos,
            Loader_episodio1 Loader,
            InfartGame GameManagerReference)
            : base(starting_pos, GameManagerReference)
        {
            fill_color_ = overlay_color_;
            Position = starting_pos;
            HorizontalMoveSpeed = 300f;
            Origin = Vector2.Zero;

            game_manager_reference_ = GameManagerReference;

            scoreggia_system_ = new ScoreggiaParticleSystem(10, Loader.textures_, Loader.textures_rectangles_["ScoreggiaParticle"]);
            jalapeno_system_ = new JalapenoParticleSystem(10, Loader);
            broccolo_system_ = new BroccoloParticleSystem(8, Loader);

            rand_ = fbonizziHelper.random;

            LoadAnimation("idle", Loader.player_idle_rects_,
                true, 0.1f, Loader.textures_);

            LoadAnimation("run", Loader.player_run_rects_,
                true, 0.05f, Loader.textures_);

            LoadAnimation("fall", Loader.player_fall_rects_,
                true, 0.01f, Loader.textures_);

            LoadAnimation("fart_sustain_up", Loader.player_fart_rects_,
                true, 0.05f, Loader.textures_);

            LoadAnimation("merdone", Loader.player_merda_rects_,
                true, 0.01f, Loader.textures_);

            PlayAnimation("fall");
        }

        public override void Reset(Vector2 position)
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

        


        
        public override void Jump(float amount)
        {
            base.Jump(amount);

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
            //   scoreggia_system_.ActivateBroccolo();
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

        

        
        public override void Update(double dt, TouchCollection touch)
        {
            if (!dead_)
            {
                string new_animation_ = "run";

                
                if (allow_input_)
                {
                    //  current_keyboard_state_ = Keyboard.GetState();
                    //current_mouse_state_ = Mouse.GetState();

                    // Corsa automatica
                    FlipEffect = SpriteEffects.None;
                    velocity_.X = +HorizontalMoveSpeed;

                    if (touch.Count > 0)
                    {
                        if (touch[0].State == TouchLocationState.Pressed)
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

                    }
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

        public override void DrawParticles(SpriteBatch spriteBatch)
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