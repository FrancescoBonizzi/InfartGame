using FbonizziMonoGame.Extensions;
using Infart.Assets;
using Infart.Astronaut;
using Infart.Background;
using Infart.Drawing;
using Infart.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infart.ParticleSystem
{
    public class InfartGame
    {
        private int resolution_w_;

        private int resolution_h_;

        protected int game_h_;

        protected Camera player_camera_;

        protected Player player_;

        protected BackgroundManager background_;

        protected GroundManager ground_;

        protected GemmaManager gemme_;

        protected SoundManager sound_manager_;

        protected StatusBar status_bar_;

        protected Loader Loader;

        protected bool paused_;

        protected InfartExplosion dead_explosion_;

        protected RecordExplosion record_explosion_;

        protected Random random_;

        protected int high_score_;

        protected int score_scoregge_;

        protected bool new_high_score_ = false;

        protected bool force_to_finish_;

        protected Rectangle high_score_position_;

        protected Color high_score_color_ = Color.Blue;

        protected SpriteFont font_;

        protected Texture2D px_texture_;

        public double BucoProbability;

        public double PowerUpProbability;

        public double PeperoncinoDuration;

        public double GemmaProbability;

        public double BroccoloDuration;

        public Vector2 LarghezzaBuchi;

        private const double default_BucoProbability = 0.005;

        private const double default_PoweupProbability = 0.03;

        private const double default_PeperoncinoDuration = 6000.0;

        private const double default_MerdoneDuration = 3500.0;

        private const double default_GemmaProbability = 0.4;

        private const float default_buco_min_w_ = 190.0f;

        private const float default_buco_max_w_ = 250.0f;

        private int old_score_metri_ = 0;

        public int score_metri_ = 0;

        private StringBuilder score_string_;

        private Vector2 score_string_position_ = new Vector2(680, 438);

        private Rectangle bar_rectangle_ = new Rectangle(400, 430, 800, 50);

        private Color bar_color_ = Color.LightCyan * 0.5f;

        private string metri_string_;

        private int high_score_x_;

        public bool fall_sound_active_;

        public bool merda_mode_active_;

        public bool jalapenos_mode_active_;

        private int game_cameraH_limit_;

        public InfartGame(
            Loader Loader,
            //        Loader_menu LoaderMenu,
            SoundManager SoundManager,
            int HighScore,
            string MetriString,
            string PauseString)
        {
            resolution_w_ = 800;
            resolution_h_ = 480;
            game_h_ = 1500;

            this.Loader = Loader;

            sound_manager_ = SoundManager;

            status_bar_ = new StatusBar(new Vector2(460, 435), Loader, SoundManager);

            high_score_ = HighScore;
            SetRecordRectangle();
            SetHighScore(HighScore);

            font_ = Loader.font_;
            px_texture_ = Loader.px_texture_;

            player_camera_ = new Camera(Vector2.Zero, new Vector2(800, 480), 0.8f);

            dead_explosion_ = new InfartExplosion(Loader);
            //     record_explosion_ = new RecordExplosion_episodio1(Loader);
#warning TODO: mettere un popup per il record

            paused_ = false;
            force_to_finish_ = false;

            InitializeBackgroundManager();
            InitializeGroundManager();
            InitializeGemmaManager();
            InitializePlayer();

            score_string_ = new StringBuilder();

            high_score_color_ = new Color(22, 232, 86) * 0.5f;
            px_texture_ = Loader.px_texture_;
            metri_string_ = MetriString;

            NewGame();
        }

        protected void InitializeBackgroundManager()
        {
            background_ = new BackgroundManager(player_camera_, (Loader as Loader), this);
        }

        protected void InitializeGroundManager()
        {
            ground_ = new GroundManager(player_camera_, (Loader as Loader), this);
        }

        protected void InitializeGemmaManager()
        {
            gemme_ = new GemmaManager(player_camera_, (Loader as Loader));
        }

        protected void InitializePlayer()
        {
            player_ = new Player(new Vector2(240, 300), (Loader as Loader), this);
        }

        public void NewGame()
        {
            dead_explosion_.Reset();

            paused_ = false;
            force_to_finish_ = false;
            new_high_score_ = false;
            sound_manager_.NewGame();

            game_cameraH_limit_ = -game_h_ - player_camera_.ViewPortHeight;

            fall_sound_active_ = false;
            merda_mode_active_ = false;
            jalapenos_mode_active_ = false;

            old_score_metri_ = 0;
            score_metri_ = 0;
            score_scoregge_ = 0;

            BucoProbability = default_BucoProbability;
            PowerUpProbability = default_PoweupProbability;
            PeperoncinoDuration = default_PeperoncinoDuration;
            BroccoloDuration = default_MerdoneDuration;
            GemmaProbability = default_GemmaProbability;
            LarghezzaBuchi = new Vector2(default_buco_min_w_, default_buco_max_w_);

            player_camera_.Reset(Vector2.Zero);
            player_.Reset(new Vector2(240, 300));

            background_.Reset(player_camera_);
            ground_.Reset(player_camera_);
            gemme_.Reset(player_camera_);
            status_bar_.Reset();
        }

        public int GetScoregge
        {
            get { return score_scoregge_; }
        }

        public int GetCurrentScore
        {
            get { return score_metri_; }
        }

        internal void HandleInput(Vector2 value)
        {
            if (fall_sound_active_ && !dead_explosion_.Started)
            {
                force_to_finish_ = true;

                if ((sound_manager_ as SoundManager).HasFallFinished())
                {
                    dead_explosion_.Explode(player_.Position, false, (sound_manager_ as SoundManager));
                    status_bar_.SetInfart();
                }
            }
        }

        public int GetHamburger
        {
            get { return status_bar_.HamburgerMangiatiInTotale; }
        }

        public List<GameObject> GroundObjects()
        {
            return ground_.WalkableObjects();
        }

        protected void SetRecordRectangle()
        {
            high_score_position_ = new Rectangle(0, -1020, 20, 1500);
        }

        public int GetScore
        {
            get { return high_score_; }
        }

        public bool IsPaused
        {
            get { return paused_; }
            set
            {
                if (value)
                    Pause();
                else ResumeFromPause();
            }
        }

        public void Pause()
        {
            paused_ = true;
            sound_manager_.PauseAll();
            GC.Collect();
        }

        public void ResumeFromPause()
        {
            paused_ = false;
            sound_manager_.ResumeAll();
        }

        public int ResolutionWidth
        {
            get { return resolution_w_; }
        }

        public int ResolutionHeight
        {
            get { return resolution_h_; }
        }

        public int PlayableGameWindowHeight
        {
            get { return game_h_; }
        }

        public void StopScoreggia()
        {
            sound_manager_.StopScoreggia();
        }

        public void DecreaseParallaxSpeed()
        {
            background_.DecreaseParallaxSpeed();
        }

        public void IncreaseParallaxSpeed()
        {
            background_.IncreaseParallaxSpeed();
        }

        public void PlayerJumped()
        {
            status_bar_.PlayerJumped();
        }

        public void AddScoreggia()
        {
            ++score_scoregge_;
            sound_manager_.PlayScoreggia();
        }

        public void MoveCamera(Vector2 where)
        {
            player_camera_.MoveTo(where);
        }

        public void SetHighScore(int value)
        {
            high_score_ = value;
            high_score_position_.X = value * 100;
            high_score_x_ = value * 100;
        }

        public void AddGemma(Vector2 position)
        {
            if (position.X > player_.Position.X + resolution_w_ / 2)
                gemme_.AddGemma(position);
        }

        public int HamburgerMangiati()
        {
            return status_bar_.HamburgerMangiatiInTotale;
        }

        public void AddPowerUp(Vector2 position)
        {
            if (position.X > player_.Position.X)
                (gemme_ as GemmaManager).AddPowerUp(position);
        }

        public void PlayerCollidedWithNormalGemma()
        {
            status_bar_.HamburgerEaten();
            (sound_manager_ as SoundManager).PlayMorso();
        }

        public void Update(TimeSpan elapsed)
        {
            var gametime = elapsed.TotalMilliseconds;
            var touch = TouchPanel.GetState();

            if (!paused_)
            {
                RepositionCamera();
                player_camera_.Update(gametime);

                CheckDead();
                background_.Update(gametime);
                ground_.Update(gametime);
                player_.Update(gametime, touch);
                player_.CollidingObjectsReference = ground_.WalkableObjects();
                gemme_.Update(gametime);
                CheckPlayerGemmaCollision();
                record_explosion_.Update(gametime);

                if (status_bar_ != null)
                    status_bar_.Update(gametime);

                if (dead_explosion_.Started)
                {
                    dead_explosion_.Update(gametime);
                    if (touch.Count > 0)
                        force_to_finish_ = true;
                }
            }

            if (score_metri_ % 50 == 0)
            {
                if (old_score_metri_ != score_metri_)
                {
                    (player_ as Player).IncreaseMoveSpeed();
                    background_.IncreaseParallaxSpeed();
                    if (LarghezzaBuchi.Y < 600)
                    {
                        LarghezzaBuchi.X += 80.0f;
                        LarghezzaBuchi.Y += 80.0f;
                    }
                }
            }
            old_score_metri_ = score_metri_;
        }

        public void CheckPlayerGemmaCollision()
        {
            if (gemme_.CheckCollisionWithPlayer(player_))
            {
                PlayerCollidedWithNormalGemma();
            }

            if ((gemme_ as GemmaManager).CheckJalapenoCollisionWithPlayer(player_))
            {
                status_bar_.ComputeJalapenos();
                (player_ as Player).ActivateJalapenos();
                (sound_manager_ as SoundManager).PlayJalapeno();
                jalapenos_mode_active_ = true;
            }
            else if ((gemme_ as GemmaManager).CheckMerdaCollisionWithPlayer(player_))
            {
                status_bar_.ComputeMerda();
                (player_ as Player).ActivateBroccolo();
                (sound_manager_ as SoundManager).PlayShit();
                merda_mode_active_ = true;
            }
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        protected void MakePlayerDead()
        {
            player_.Dead = true;

            if (high_score_ < score_metri_)
            {
                SetHighScore(score_metri_);
                record_explosion_.Explode(player_.Position, 90);
            }
        }

        protected void CheckDead()
        {
            if (!fall_sound_active_)
            {
                if (player_.Position.Y > player_camera_.ViewPortHeight)
                {
                    MakePlayerDead();
                    (sound_manager_ as SoundManager).PlayFall();
                    fall_sound_active_ = true;
                }
            }

            if (status_bar_.IsInfarting() && !player_.Dead)
            {
                dead_explosion_.Explode(player_.Position, true, (sound_manager_ as SoundManager));
                MakePlayerDead();
            }
        }

        public bool IsTimeToGameOver()
        {
            if (force_to_finish_)
            {
                sound_manager_.StopSounds();
                return true;
            }

            if (dead_explosion_.Started)
            {
                return dead_explosion_.Finished;
            }
            else if (fall_sound_active_)
            {
                return false;
            }
            else
            {
                return player_.Dead;
            }
        }

        protected void LerpCameraPosition(float NewCameraX, float NewCameraY)
        {
            player_camera_.Position = new Vector2(
              MathHelper.Lerp(player_camera_.Position.X, NewCameraX, 0.08f),
              MathHelper.Lerp(player_camera_.Position.Y, NewCameraY, 0.08f));
        }

        public Player PlayerReference
        {
            get { return player_; }
        }

        protected void RepositionCamera()
        {
            float player_camera_y;
            float player_camera_x;

            player_camera_x = player_.Position.X - 150;
            player_camera_y = player_.Position.Y - 200;

            if (player_camera_y < game_cameraH_limit_)
                player_camera_y = game_cameraH_limit_;
            else if (player_camera_y > 0)
                player_camera_y = 0;

            LerpCameraPosition(player_camera_x, player_camera_y);
        }

        public void UpdateForMenu(double gametime, TouchCollection touch)
        {
            background_.Update(gametime);
            ground_.Update(gametime);
            player_camera_.Update(gametime);

            player_.Update(gametime, touch);
        }

        protected void DrawUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(
                px_texture_,
                bar_rectangle_,
                bar_color_);

            score_string_.Clear();
            StringBuilderExtensions.AppendNumber(score_string_, score_metri_).Append(metri_string_);
            spriteBatch.DrawString(font_, score_string_, score_string_position_, Color.DarkBlue);
            status_bar_.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Matrix Camera_transformation = player_camera_.GetTransformation();

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera_transformation);

            background_.Draw(spritebatch);
            ground_.Draw(spritebatch);
            gemme_.Draw(spritebatch);

            spritebatch.End();

            if (!dead_explosion_.Started)
            {
                spritebatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Camera_transformation);
                player_.DrawParticles(spritebatch);
                spritebatch.End();
            }

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera_transformation);

            if (dead_explosion_.Started)
                dead_explosion_.Draw(spritebatch);
            else
            {
                player_.Draw(spritebatch);
            }

            background_.DrawSpecial(spritebatch);

            if (high_score_ != 0
               && player_.Position.X >= high_score_position_.X - resolution_w_
               && !new_high_score_)
            {
                if (player_.Position.X < high_score_position_.X)
                {
                    spritebatch.Draw(
                        px_texture_,
                        high_score_position_,
                        high_score_color_);
                }
                else
                {
                    record_explosion_.Explode(player_.Position, 0);
                    new_high_score_ = true;
                }
            }

            record_explosion_.Draw(spritebatch);

            spritebatch.End();

            DrawUI(spritebatch);
        }

        public void DrawForMenu(SpriteBatch spritebatch)
        {
            Matrix Camera_transformation = player_camera_.GetTransformation();

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera_transformation);
            background_.Draw(spritebatch);
            ground_.Draw(spritebatch);
            spritebatch.End();

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Camera_transformation);
            if (!dead_explosion_.Started)
                player_.DrawParticles(spritebatch);
            spritebatch.End();

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera_transformation);
            if (!dead_explosion_.Started)
                player_.Draw(spritebatch);
            background_.DrawSpecial(spritebatch);
            spritebatch.End();
        }
    }
}