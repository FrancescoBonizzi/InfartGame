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
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace fge
{
    public abstract class GameManager
    {
        #region Dichiarazioni

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

        // Devo cercare di mantenere questi punti fissi perché ci sia 
        // un'esperienza di gioco complessiva
        protected InfartExplosion dead_explosion_;
        protected RecordExplosion record_explosion_;

        protected Random random_;

        // rimetti tutto highscore
        protected int high_score_;
        protected int score_scoregge_;
        protected bool new_high_score_ = false;

        protected bool force_to_finish_;

        protected Rectangle high_score_position_;
        protected Color high_score_color_ = Color.Blue;
        protected SpriteFont font_;

        protected Texture2D px_texture_;

        #endregion

        #region Costruttore

        public GameManager(
            int ResolutionWidth,
            int ResolutionHeight,
            int GameHeight,
            SoundManager SoundManager,
            StatusBar StatusBar,
            int HighScore,
            Camera Camera,
            SpriteFont Font,
            Texture2D PxTexture,
            Loader Loader,
            InfartExplosion InfartExplosion,
            RecordExplosion RecordExplosion)
        {
            resolution_w_ = ResolutionWidth;
            resolution_h_ = ResolutionHeight;
            game_h_ = GameHeight;

            this.Loader = Loader;

            sound_manager_ = SoundManager;

            status_bar_ = StatusBar;

            high_score_ = HighScore;
            SetRecordRectangle();
            SetHighScore(HighScore);

            font_ = Font;
            px_texture_ = PxTexture;

            player_camera_ = Camera; //new Camera(Vector2.Zero, new Vector2(resolution_w_, resolution_h_));

            dead_explosion_ = InfartExplosion;
            record_explosion_ = RecordExplosion;

            paused_ = false;
            force_to_finish_ = false;

            InitializeBackgroundManager();
            InitializeGroundManager();
            InitializeGemmaManager();
            InitializePlayer();
        }

        protected abstract void InitializeBackgroundManager();
        protected abstract void InitializeGroundManager();
        protected abstract void InitializeGemmaManager();
        protected abstract void InitializePlayer();
        public abstract List<GameObject> GroundObjects();

        public virtual void NewGame()
        {
            dead_explosion_.Reset();

            paused_ = false;
            force_to_finish_ = false;
            new_high_score_ = false;
            sound_manager_.NewGame();
        }



        protected abstract void SetRecordRectangle();

        #endregion

        #region Proprietà

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

        #endregion

        #region Metodi

        public void StopScoreggia()
        {
            sound_manager_.StopScoreggia();
        }

        public void DecreaseParallaxSpeed()
        {
            background_.DecreaseParallaxSpeed();
            //background_.DecreaseParallaxSpeed();
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

        public abstract void SetHighScore(int value);

        public void AddGemma(Vector2 position)
        {
            if (position.X > player_.Position.X + resolution_w_ / 2)
                gemme_.AddGemma(position);
        }

        public abstract void PlayerCollidedWithNormalGemma();

        public virtual void CheckPlayerGemmaCollision()
        {
            if (gemme_.CheckCollisionWithPlayer(player_))
            {
                PlayerCollidedWithNormalGemma();
            }
            // Da fare base all'inizio nella funzione specializzata
        }

        //sound play quando aggiungi scoreggia

        #endregion

        protected virtual void MakePlayerDead()
        {
            player_.Dead = true;
        }

        protected abstract void CheckDead();

        public abstract bool IsTimeToGameOver();

        public Player PlayerReference
        {
            get { return player_; }
        }

        protected void LerpCameraPosition(float NewCameraX, float NewCameraY)
        {
            player_camera_.Position = new Vector2(
              MathHelper.Lerp(player_camera_.Position.X, NewCameraX, 0.08f),
              MathHelper.Lerp(player_camera_.Position.Y, NewCameraY, 0.08f));
        }

        protected abstract void RepositionCamera();

        #region Update/Draw

        protected virtual void GameUpdate(double gametime, TouchCollection touch)
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

        public virtual void Update(double gametime, TouchCollection touch)
        {
            if (!paused_)
            {
                GameUpdate(gametime, touch);
            }

        }

        protected abstract void DrawUI(SpriteBatch spriteBatch);

        public virtual void Draw(SpriteBatch spritebatch)
        {
            Matrix Camera_transformation = player_camera_.GetTransformation();

            #region Scene

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera_transformation);

            background_.Draw(spritebatch);
            ground_.Draw(spritebatch);
            gemme_.Draw(spritebatch);

            spritebatch.End();

            #endregion

            #region Player + Particles + Explosions

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
                player_.Draw(spritebatch); // Perchè la scoreggia stia dietro di lui
            }

            background_.DrawSpecial(spritebatch);

            #endregion

            #region Record

            if (high_score_ != 0
               && player_.Position.X >= high_score_position_.X - resolution_w_
               && !new_high_score_)
            {
                if (player_.Position.X < high_score_position_.X)
                {
                    // E' la sbarra verticale
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

            record_explosion_.Draw(spritebatch); // MMMMMMM

            spritebatch.End();

            #endregion

            #region UI

            DrawUI(spritebatch);

            // Leva il pulsante della pausa, metti in alto la barra...
            // In alto a sx la pubblicità, in alto a destra i metri


            #endregion
        }

        #endregion
    }
}

