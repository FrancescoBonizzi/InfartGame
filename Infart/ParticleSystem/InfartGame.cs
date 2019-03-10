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
        private readonly int _resolutionW;

        private readonly int _resolutionH;

        protected int GameH;

        protected Camera PlayerCamera;

        protected Player Player;

        protected BackgroundManager Background;

        protected GroundManager Ground;

        protected GemmaManager Gemme;

        protected SoundManager SoundManager;

        protected StatusBar StatusBar;

        protected AssetsLoader AssetsLoader;

        protected bool Paused;

        protected InfartExplosion DeadExplosion;

        protected RecordExplosion RecordExplosion;

        protected Random Random;

        protected int HighScore;

        protected int ScoreScoregge;

        protected bool NewHighScore = false;

        protected bool ForceToFinish;

        protected Rectangle HighScorePosition;

        protected Color HighScoreColor = Color.Blue;

        protected SpriteFont Font;

        protected Texture2D PxTexture;

        public double BucoProbability;

        public double PowerUpProbability;

        public double PeperoncinoDuration;

        public double GemmaProbability;

        public double BroccoloDuration;

        public Vector2 LarghezzaBuchi;

        private const double DefaultBucoProbability = 0.005;

        private const double DefaultPoweupProbability = 0.03;

        private const double DefaultPeperoncinoDuration = 6000.0;

        private const double DefaultMerdoneDuration = 3500.0;

        private const double DefaultGemmaProbability = 0.4;

        private const float DefaultBucoMinW = 190.0f;

        private const float DefaultBucoMaxW = 250.0f;

        private int _oldScoreMetri = 0;

        public int ScoreMetri = 0;

        private readonly StringBuilder _scoreString;

        private readonly Vector2 _scoreStringPosition = new Vector2(680, 438);

        private Rectangle _barRectangle = new Rectangle(400, 430, 800, 50);

        private Color _barColor = Color.LightCyan * 0.5f;

        private readonly string _metriString;

        private int _highScoreX;

        public bool FallSoundActive;

        public bool MerdaModeActive;

        public bool JalapenosModeActive;

        private int _gameCameraHLimit;

        public InfartGame(
            AssetsLoader assetsLoader,
            //        Loader_menu LoaderMenu,
            SoundManager soundManager,
            int highScore,
            string metriString,
            string pauseString)
        {
            _resolutionW = 800;
            _resolutionH = 480;
            GameH = 1500;

            this.AssetsLoader = assetsLoader;

            this.SoundManager = soundManager;

            StatusBar = new StatusBar(new Vector2(460, 435), assetsLoader, soundManager);

            this.HighScore = highScore;
            SetRecordRectangle();
            SetHighScore(highScore);

            Font = assetsLoader.Font;
            //   px_texture_ = AssetsLoader.px_texture_;

            PlayerCamera = new Camera(Vector2.Zero, new Vector2(800, 480), 0.8f);

            DeadExplosion = new InfartExplosion(assetsLoader);
            //     record_explosion_ = new RecordExplosion_episodio1(AssetsLoader);
#warning TODO: mettere un popup per il record

            Paused = false;
            ForceToFinish = false;

            InitializeBackgroundManager();
            InitializeGroundManager();
            InitializeGemmaManager();
            InitializePlayer();

            _scoreString = new StringBuilder();

            HighScoreColor = new Color(22, 232, 86) * 0.5f;
            //     px_texture_ = AssetsLoader.px_texture_;
            _metriString = metriString;

            NewGame();
        }

        protected void InitializeBackgroundManager()
        {
            Background = new BackgroundManager(PlayerCamera, (AssetsLoader as AssetsLoader), this);
        }

        protected void InitializeGroundManager()
        {
            Ground = new GroundManager(PlayerCamera, (AssetsLoader as AssetsLoader), this);
        }

        protected void InitializeGemmaManager()
        {
            Gemme = new GemmaManager(PlayerCamera, (AssetsLoader as AssetsLoader));
        }

        protected void InitializePlayer()
        {
            Player = new Player(new Vector2(240, 300), (AssetsLoader as AssetsLoader), this);
        }

        public void NewGame()
        {
            DeadExplosion.Reset();

            Paused = false;
            ForceToFinish = false;
            NewHighScore = false;
            SoundManager.NewGame();

            _gameCameraHLimit = -GameH - PlayerCamera.ViewPortHeight;

            FallSoundActive = false;
            MerdaModeActive = false;
            JalapenosModeActive = false;

            _oldScoreMetri = 0;
            ScoreMetri = 0;
            ScoreScoregge = 0;

            BucoProbability = DefaultBucoProbability;
            PowerUpProbability = DefaultPoweupProbability;
            PeperoncinoDuration = DefaultPeperoncinoDuration;
            BroccoloDuration = DefaultMerdoneDuration;
            GemmaProbability = DefaultGemmaProbability;
            LarghezzaBuchi = new Vector2(DefaultBucoMinW, DefaultBucoMaxW);

            PlayerCamera.Reset(Vector2.Zero);
            Player.Reset(new Vector2(240, 300));

            Background.Reset(PlayerCamera);
            Ground.Reset(PlayerCamera);
            Gemme.Reset(PlayerCamera);
            StatusBar.Reset();
        }

        public int GetScoregge
        {
            get { return ScoreScoregge; }
        }

        public int GetCurrentScore
        {
            get { return ScoreMetri; }
        }

        public void HandleInput()
        {
            if (!Player.Dead)
            {
                Player.HandleInput();
            }

            if (FallSoundActive && !DeadExplosion.Started)
            {
                ForceToFinish = true;

                if ((SoundManager as SoundManager).HasFallFinished())
                {
                    DeadExplosion.Explode(Player.Position, false, (SoundManager as SoundManager));
                    StatusBar.SetInfart();
                }
            }

            if (DeadExplosion.Started)
            {
                ForceToFinish = true;
            }
        }

        public int GetHamburger
        {
            get { return StatusBar.HamburgerMangiatiInTotale; }
        }

        public List<GameObject> GroundObjects()
        {
            return Ground.WalkableObjects();
        }

        protected void SetRecordRectangle()
        {
            HighScorePosition = new Rectangle(0, -1020, 20, 1500);
        }

        public int GetScore
        {
            get { return HighScore; }
        }

        public bool IsPaused
        {
            get { return Paused; }
            set
            {
                if (value)
                    Pause();
                else ResumeFromPause();
            }
        }

        public void Pause()
        {
            Paused = true;
            SoundManager.PauseAll();
            GC.Collect();
        }

        public void ResumeFromPause()
        {
            Paused = false;
            SoundManager.ResumeAll();
        }

        public int ResolutionWidth
        {
            get { return _resolutionW; }
        }

        public int ResolutionHeight
        {
            get { return _resolutionH; }
        }

        public int PlayableGameWindowHeight
        {
            get { return GameH; }
        }

        public void StopScoreggia()
        {
            SoundManager.StopScoreggia();
        }

        public void DecreaseParallaxSpeed()
        {
            Background.DecreaseParallaxSpeed();
        }

        public void IncreaseParallaxSpeed()
        {
            Background.IncreaseParallaxSpeed();
        }

        public void PlayerJumped()
        {
            StatusBar.PlayerJumped();
        }

        public void AddScoreggia()
        {
            ++ScoreScoregge;
            SoundManager.PlayScoreggia();
        }

        public void MoveCamera(Vector2 where)
        {
            PlayerCamera.MoveTo(where);
        }

        public void SetHighScore(int value)
        {
            HighScore = value;
            HighScorePosition.X = value * 100;
            _highScoreX = value * 100;
        }

        public void AddGemma(Vector2 position)
        {
            if (position.X > Player.Position.X + _resolutionW / 2)
                Gemme.AddGemma(position);
        }

        public int HamburgerMangiati()
        {
            return StatusBar.HamburgerMangiatiInTotale;
        }

        public void AddPowerUp(Vector2 position)
        {
            if (position.X > Player.Position.X)
                (Gemme as GemmaManager).AddPowerUp(position);
        }

        public void PlayerCollidedWithNormalGemma()
        {
            StatusBar.HamburgerEaten();
            (SoundManager as SoundManager).PlayMorso();
        }

        public void Update(TimeSpan elapsed)
        {
            var gametime = elapsed.TotalMilliseconds;
            if (!Paused)
            {
                RepositionCamera();
                PlayerCamera.Update(gametime);

                CheckDead();
                Background.Update(gametime);
                Ground.Update(gametime);
                Player.Update(gametime);
                Player.CollidingObjectsReference = Ground.WalkableObjects();
                Gemme.Update(gametime);
                CheckPlayerGemmaCollision();
                //         record_explosion_.Update(gametime);

                if (StatusBar != null)
                    StatusBar.Update(gametime);

                if (DeadExplosion.Started)
                {
                    DeadExplosion.Update(gametime);
                }
            }

            if (ScoreMetri % 50 == 0)
            {
                if (_oldScoreMetri != ScoreMetri)
                {
                    (Player as Player).IncreaseMoveSpeed();
                    Background.IncreaseParallaxSpeed();
                    if (LarghezzaBuchi.Y < 600)
                    {
                        LarghezzaBuchi.X += 80.0f;
                        LarghezzaBuchi.Y += 80.0f;
                    }
                }
            }
            _oldScoreMetri = ScoreMetri;
        }

        public void CheckPlayerGemmaCollision()
        {
            if (Gemme.CheckCollisionWithPlayer(Player))
            {
                PlayerCollidedWithNormalGemma();
            }

            if ((Gemme as GemmaManager).CheckJalapenoCollisionWithPlayer(Player))
            {
                StatusBar.ComputeJalapenos();
                (Player as Player).ActivateJalapenos();
                (SoundManager as SoundManager).PlayJalapeno();
                JalapenosModeActive = true;
            }
            else if ((Gemme as GemmaManager).CheckMerdaCollisionWithPlayer(Player))
            {
                StatusBar.ComputeMerda();
                (Player as Player).ActivateBroccolo();
                (SoundManager as SoundManager).PlayShit();
                MerdaModeActive = true;
            }
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        protected void MakePlayerDead()
        {
            Player.Dead = true;

            if (HighScore < ScoreMetri)
            {
                SetHighScore(ScoreMetri);
                RecordExplosion.Explode(Player.Position, 90);
            }
        }

        protected void CheckDead()
        {
            if (!FallSoundActive)
            {
                if (Player.Position.Y > PlayerCamera.ViewPortHeight)
                {
                    MakePlayerDead();
                    (SoundManager as SoundManager).PlayFall();
                    FallSoundActive = true;
                }
            }

            if (StatusBar.IsInfarting() && !Player.Dead)
            {
                DeadExplosion.Explode(Player.Position, true, (SoundManager as SoundManager));
                MakePlayerDead();
            }
        }

        public bool IsTimeToGameOver()
        {
            if (ForceToFinish)
            {
                SoundManager.StopSounds();
                return true;
            }

            if (DeadExplosion.Started)
            {
                return DeadExplosion.Finished;
            }
            else if (FallSoundActive)
            {
                return false;
            }
            else
            {
                return Player.Dead;
            }
        }

        protected void LerpCameraPosition(float newCameraX, float newCameraY)
        {
            PlayerCamera.Position = new Vector2(
              MathHelper.Lerp(PlayerCamera.Position.X, newCameraX, 0.08f),
              MathHelper.Lerp(PlayerCamera.Position.Y, newCameraY, 0.08f));
        }

        public Player PlayerReference
        {
            get { return Player; }
        }

        protected void RepositionCamera()
        {
            float playerCameraY;
            float playerCameraX;

            playerCameraX = Player.Position.X - 150;
            playerCameraY = Player.Position.Y - 200;

            if (playerCameraY < _gameCameraHLimit)
                playerCameraY = _gameCameraHLimit;
            else if (playerCameraY > 0)
                playerCameraY = 0;

            LerpCameraPosition(playerCameraX, playerCameraY);
        }

        public void UpdateForMenu(double gametime, TouchCollection touch)
        {
            Background.Update(gametime);
            Ground.Update(gametime);
            PlayerCamera.Update(gametime);

            Player.Update(gametime);
        }

        protected void DrawUi(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //spriteBatch.Draw(
            //    px_texture_,
            //    bar_rectangle_,
            //    bar_color_);

            _scoreString.Clear();
            StringBuilderExtensions.AppendNumber(_scoreString, ScoreMetri).Append(_metriString);
            spriteBatch.DrawString(Font, _scoreString, _scoreStringPosition, Color.DarkBlue);
            StatusBar.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Matrix cameraTransformation = PlayerCamera.GetTransformation();

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraTransformation);

            Background.Draw(spritebatch);
            Ground.Draw(spritebatch);
            Gemme.Draw(spritebatch);

            spritebatch.End();

            if (!DeadExplosion.Started)
            {
                spritebatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, cameraTransformation);
                Player.DrawParticles(spritebatch);
                spritebatch.End();
            }

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraTransformation);

            if (DeadExplosion.Started)
                DeadExplosion.Draw(spritebatch);
            else
            {
                Player.Draw(spritebatch);
            }

            Background.DrawSpecial(spritebatch);

            if (HighScore != 0
               && Player.Position.X >= HighScorePosition.X - _resolutionW
               && !NewHighScore)
            {
                if (Player.Position.X < HighScorePosition.X)
                {
                    //spritebatch.Draw(
                    //    px_texture_,
                    //    high_score_position_,
                    //    high_score_color_);
                }
                else
                {
                    RecordExplosion.Explode(Player.Position, 0);
                    NewHighScore = true;
                }
            }

            //    record_explosion_.Draw(spritebatch);

            spritebatch.End();

            DrawUi(spritebatch);
        }

        public void DrawForMenu(SpriteBatch spritebatch)
        {
            Matrix cameraTransformation = PlayerCamera.GetTransformation();

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraTransformation);
            Background.Draw(spritebatch);
            Ground.Draw(spritebatch);
            spritebatch.End();

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, cameraTransformation);
            if (!DeadExplosion.Started)
                Player.DrawParticles(spritebatch);
            spritebatch.End();

            spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraTransformation);
            if (!DeadExplosion.Started)
                Player.Draw(spritebatch);
            Background.DrawSpecial(spritebatch);
            spritebatch.End();
        }
    }
}