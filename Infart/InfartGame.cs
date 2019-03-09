
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Text;



namespace fge
{
    public class InfartGame : GameManager
    {
        public double BucoProbability;
        public double PowerUpProbability;
        public double PeperoncinoDuration;
        public double GemmaProbability;
        public double BroccoloDuration;
        public Vector2 LarghezzaBuchi;
        //-----------------------------------------------
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


        private int game_cameraH_limit_;//la nuova altezza dopo lo zoom

        //  private Loader_episodio1 Loader;


        public InfartGame(
            Loader_episodio1 Loader,
            Loader_menu LoaderMenu,
            SoundManager_episodio1 SoundManager,
            int HighScore,
            string MetriString,
            string PauseString)
            : base(
            800,
            480,
            1500,
            SoundManager,
            new StatusBar(new Vector2(460, 435), Loader, SoundManager),
            HighScore,
            new Camera(Vector2.Zero, new Vector2(800, 480), 0.8f),
            Loader.font_,
            Loader.px_texture_,
            Loader,
            new InfartExplosion_episodio1(Loader),
            new RecordExplosion_episodio1(Loader))
        {
            score_string_ = new StringBuilder();

            high_score_color_ = new Color(22, 232, 86) * 0.5f;
            px_texture_ = Loader.px_texture_;
            metri_string_ = MetriString;

            NewGame();
        }

        protected override void InitializeBackgroundManager()
        {
            background_ = new BackgroundManager_episodio1(player_camera_, (Loader as Loader_episodio1), this);
        }

        protected override void InitializeGroundManager()
        {
            ground_ = new GroundManager_episodio1(player_camera_, (Loader as Loader_episodio1), this);
        }

        protected override void InitializeGemmaManager()
        {
            gemme_ = new GemmaManager_episodio1(player_camera_, (Loader as Loader_episodio1));
        }

        protected override void InitializePlayer()
        {
            player_ = new Player_episodio1(new Vector2(240, 300), (Loader as Loader_episodio1), this);
        }

        public override void NewGame()
        {
            base.NewGame();

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

                if ((sound_manager_ as SoundManager_episodio1).HasFallFinished())
                {
                    dead_explosion_.Explode(player_.Position, false, (sound_manager_ as SoundManager_episodio1));
                    status_bar_.SetInfart();
                }
            }
        }

        public int GetHamburger
        {
            get { return status_bar_.HamburgerMangiatiInTotale; }
        }

        public override List<GameObject> GroundObjects()
        {
            return ground_.WalkableObjects();
        }

        protected override void SetRecordRectangle()
        {
            high_score_position_ = new Rectangle(0, -1020, 20, 1500);
        }

        public override void SetHighScore(int value)
        {
            high_score_ = value;
            high_score_position_.X = value * 100;
            high_score_x_ = value * 100;
        }

        public int HamburgerMangiati()
        {
            return status_bar_.HamburgerMangiatiInTotale;
        }

        public void AddPowerUp(Vector2 position)
        {
            if (position.X > player_.Position.X)
                (gemme_ as GemmaManager_episodio1).AddPowerUp(position);
        }

        public override void PlayerCollidedWithNormalGemma()
        {
            status_bar_.HamburgerEaten();
            (sound_manager_ as SoundManager_episodio1).PlayMorso();
        }

        public void Update(TimeSpan elapsed)
        {
            base.Update(elapsed.TotalMilliseconds, TouchPanel.GetState());

            // Ogni 50m aumento i parametri
            if (score_metri_ % 50 == 0)
            {
                // Se è appena cambiato (Perché rimane sullo stesso metro per un po')
                if (old_score_metri_ != score_metri_)
                {
                    (player_ as Player_episodio1).IncreaseMoveSpeed();
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

        public override void CheckPlayerGemmaCollision()
        {
            base.CheckPlayerGemmaCollision();

            if ((gemme_ as GemmaManager_episodio1).CheckJalapenoCollisionWithPlayer(player_))
            {
                status_bar_.ComputeJalapenos();
                (player_ as Player_episodio1).ActivateJalapenos();
                (sound_manager_ as SoundManager_episodio1).PlayJalapeno();
                jalapenos_mode_active_ = true;
            }
            else if ((gemme_ as GemmaManager_episodio1).CheckMerdaCollisionWithPlayer(player_))
            {
                status_bar_.ComputeMerda();
                (player_ as Player_episodio1).ActivateBroccolo();
                (sound_manager_ as SoundManager_episodio1).PlayShit();
                merda_mode_active_ = true;
            }
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        protected override void MakePlayerDead()
        {
            base.MakePlayerDead();

            if (high_score_ < score_metri_)
            {
                SetHighScore(score_metri_);
                record_explosion_.Explode(player_.Position, 90);
            }
        }

        protected override void CheckDead()
        {
            if (!fall_sound_active_)
            {
                if (player_.Position.Y > player_camera_.ViewPortHeight) //480
                {
                    MakePlayerDead();
                    (sound_manager_ as SoundManager_episodio1).PlayFall();
                    fall_sound_active_ = true;
                }
            }

            if (status_bar_.IsInfarting() && !player_.Dead)
            {
                dead_explosion_.Explode(player_.Position, true, (sound_manager_ as SoundManager_episodio1));
                MakePlayerDead();
            }
        }

        public override bool IsTimeToGameOver()
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
            else if (fall_sound_active_)// Se il suono è partito
            {
                return false;
            }
            else
            {
                return player_.Dead;
            }
        }

        protected override void RepositionCamera()
        {
            float player_camera_y;
            float player_camera_x;

            player_camera_x = player_.Position.X - 150;
            player_camera_y = player_.Position.Y - 200;

            // Confino la telecamera nei limiti del gioco
            if (player_camera_y < game_cameraH_limit_)
                player_camera_y = game_cameraH_limit_;
            else if (player_camera_y > 0)//0)
                player_camera_y = 0;//0;

            LerpCameraPosition(player_camera_x, player_camera_y);
        }

        public void UpdateForMenu(double gametime, TouchCollection touch)
        {
            background_.Update(gametime);
            ground_.Update(gametime);
            player_camera_.Update(gametime);

            player_.Update(gametime, touch);
        }

        protected override void DrawUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Barra per contenere ads e score
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

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            // Niente da aggiungere
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
                player_.Draw(spritebatch); // Perchè la scoreggia stia dietro di lui
            background_.DrawSpecial(spritebatch);
            spritebatch.End();
        }
    }
}

