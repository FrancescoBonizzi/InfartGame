using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.StringsLocalization.Abstractions;
using FbonizziMonoGame.TransformationObjects;
using Infart.Assets;
using Infart.Astronaut;
using Infart.Background;
using Infart.Drawing;
using Infart.HUD;
using Infart.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infart
{
    public class InfartGame
    {
        private readonly Camera _playerCamera;
        private BackgroundManager _background;
        private GroundManager _ground;
        private GemmaManager _gemme;
        private readonly SoundManager _soundManager;
        private readonly StatusBar _statusBar;
        private readonly GameOrchestrator _gameOrchestrator;
        private readonly AssetsLoader _assetsLoader;
        private bool _isPaused;
        private readonly InfartExplosion _deadExplosion;
        private bool _newHighScore = false;
        private bool _forceToFinish;
        private Rectangle _highScorePosition;
        private readonly Color _highScoreColor = Color.Blue;
        private readonly SpriteFont _font;

        public double BucoProbability { get; private set; }
        public double PeperoncinoDuration { get; private set; }
        public double GemmaProbability { get; private set; }
        public double BroccoloDuration { get; private set; }
        public Vector2 LarghezzaBuchi { get => _larghezzaBuchi; private set => _larghezzaBuchi = value; }

        private const double DefaultBucoProbability = 0.005;
        private const double DefaultPowerupProbability = 0.035;
        private const double DefaultPeperoncinoDuration = 6000.0;
        private const double DefaultMerdoneDuration = 3500.0;
        private const double DefaultGemmaProbability = 0.4;
        private const float DefaultBucoMinW = 190.0f;
        private const float DefaultBucoMaxW = 250.0f;
        private int _oldScoreMetri = 0;

        public int ScoreMetri { get; set; } = 0;

        private readonly StringBuilder _scoreString;
        private readonly Vector2 _scoreStringPosition = new Vector2(680, 438);

        private readonly string _metriString;
        private int _highScoreX;

        public bool FallSoundActive { get; set; }
        public bool MerdaModeActive { get; set; }
        public bool JalapenosModeActive { get; set; }

        private int _gameCameraHLimit;
        private Vector2 _larghezzaBuchi;

        private bool _recordMetersNotified = false;
        private PopupText _recordMetersPopup;
        private string _recordMetersText;

        private PopupText _jalapenoPopup;
        private readonly string _jalapenoPopupText;

        private PopupText _broccoloPopup;
        private readonly string _broccoloPopupText;

        private PopupText _beanPopup;
        private readonly string _beanPopupText;

        public InfartGame(
            AssetsLoader assetsLoader,
            SoundManager soundManager,
            GameOrchestrator gameOrchestrator,
            ISettingsRepository settingsRepository,
            ILocalizedStringsRepository localizedStringsRepository)
        {
            ResolutionWidth = 800;
            ResolutionHeight = 480;
            PlayableGameWorldHeight = 1500;

            _assetsLoader = assetsLoader;
            _soundManager = soundManager;
            _statusBar = new StatusBar(new Vector2(460, 435), assetsLoader, soundManager);
            _gameOrchestrator = gameOrchestrator;

            SetRecordRectangle();
            SetHighScore(settingsRepository.GetOrSetInt(GameScores.BestNumberOfMetersScoreKey, 0));

            _font = assetsLoader.Font;

            _playerCamera = new Camera(Vector2.Zero, new Vector2(800, 480), 0.8f);

            _deadExplosion = new InfartExplosion(assetsLoader);

            _isPaused = false;
            _forceToFinish = false;

            InitializeBackgroundManager();
            InitializeGroundManager();
            InitializeGemmaManager();
            InitializePlayer();

            _scoreString = new StringBuilder();

            _highScoreColor = new Color(22, 232, 86) * 0.5f;
            _metriString = localizedStringsRepository.Get(GameStringsLoader.MetriTimeString);
            _recordMetersText = localizedStringsRepository.Get(GameStringsLoader.MetriRecordPopupString);

            _jalapenoPopupText = "JALAPENO!";
            _broccoloPopupText = "RIDING ON A POO!";
            _beanPopupText = "ASS STORM!";

            NewGame();
        }

        private void NewGame()
        {
            _deadExplosion.Reset();

            _isPaused = false;
            _forceToFinish = false;
            _newHighScore = false;

            _gameCameraHLimit = -PlayableGameWorldHeight - _playerCamera.ViewPortHeight;

            FallSoundActive = false;
            MerdaModeActive = false;
            JalapenosModeActive = false;

            _oldScoreMetri = 0;
            ScoreMetri = 0;
            GetScoregge = 0;

            BucoProbability = DefaultBucoProbability;
            PeperoncinoDuration = DefaultPeperoncinoDuration;
            BroccoloDuration = DefaultMerdoneDuration;
            GemmaProbability = DefaultGemmaProbability;
            LarghezzaBuchi = new Vector2(DefaultBucoMinW, DefaultBucoMaxW);

            _playerCamera.Reset(Vector2.Zero);
            Player.Reset(new Vector2(240, 300));

            _background.Reset(_playerCamera);
            _ground.Reset(_playerCamera);
            _gemme.Reset(_playerCamera);
            _statusBar.Reset();

            _soundManager.PlayGameMusicBackground();
        }

        private void InitializeBackgroundManager()
        {
            _background = new BackgroundManager(_playerCamera, _assetsLoader, this);
        }

        private void InitializeGroundManager()
        {
            _ground = new GroundManager(_playerCamera, _assetsLoader, this);
        }

        private void InitializeGemmaManager()
        {
            _gemme = new GemmaManager(_playerCamera, _assetsLoader);
        }

        private void InitializePlayer()
        {
            Player = new Player(new Vector2(240, 300), _assetsLoader, this);
        }

        public int GetScoregge { get; private set; }

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

            if (_deadExplosion.Started)
            {
                _forceToFinish = true;
            }
        }

        public int GetHamburger
        {
            get { return _statusBar.VerdureMangiateInTotale; }
        }

        public List<GameObject> GroundObjects()
        {
            return _ground.WalkableObjects();
        }

        private void SetRecordRectangle()
        {
            _highScorePosition = new Rectangle(0, -1000, 20, 2500);
        }

        public int HighScore { get; private set; }

        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                if (value)
                {
                    Pause();
                }
                else
                {
                    ResumeFromPause();
                }
            }
        }

        public void Pause()
        {
            _isPaused = true;
            _soundManager.PauseAll();
            GC.Collect();
        }

        public void ResumeFromPause()
        {
            _isPaused = false;
            _soundManager.ResumeAll();
        }

        public int ResolutionWidth { get; }
        public int ResolutionHeight { get; }

        public int PlayableGameWorldHeight { get; }

        public void StopScoreggia()
        {
            _soundManager.StopFart();
        }

        public void DecreaseParallaxSpeed()
        {
            _background.DecreaseParallaxSpeed();
        }

        public void IncreaseParallaxSpeed()
        {
            _background.IncreaseParallaxSpeed();
        }

        public void PlayerJumped()
        {
            _statusBar.PlayerJumped();
        }

        public void AddScoreggia()
        {
            ++GetScoregge;
            _soundManager.PlayFart();
        }

        public void SetHighScore(int value)
        {
            HighScore = value;
            _highScorePosition.X = value * 100;
            _highScoreX = value * 100;
        }

        public void AddGemma(Vector2 position)
        {
            if (position.X > Player.Position.X + (ResolutionWidth / 2))
            {
                _gemme.AddGemma(position);
            }
        }

        public int VerdureMangiate()
        {
            return _statusBar.VerdureMangiateInTotale;
        }

        public void AddPowerUp(Vector2 position)
        {
            if (position.X > Player.Position.X)
            {
                (_gemme).AddPowerUp(position);
            }
        }

        public void PlayerCollidedWithNormalGemma()
        {
            _statusBar.HamburgerEaten();
            (_soundManager).PlayBite();
        }

        public void Update(TimeSpan elapsed)
        {
            var gametime = elapsed.TotalMilliseconds;
            if (!_isPaused)
            {
                RepositionCamera();

                CheckDead();
                _background.Update(gametime);
                _ground.Update(gametime);
                Player.Update(gametime);
                Player.CollidingObjectsReference = _ground.WalkableObjects();
                _gemme.Update(gametime);
                CheckPlayerGemmaCollision();

                if (!_recordMetersNotified && ScoreMetri > HighScore)
                {
                    PopupRecord();
                }

                if (_recordMetersPopup != null)
                {
                    _recordMetersPopup.Update(elapsed);

                    if (_recordMetersPopup.PopupObject.IsCompleted)
                    {
                        _recordMetersPopup = null;
                    }
                }

                if (_jalapenoPopup != null)
                {
                    _jalapenoPopup.Update(elapsed);

                    if (_jalapenoPopup.PopupObject.IsCompleted)
                    {
                        _jalapenoPopup = null;
                    }
                }

                if (_broccoloPopup != null)
                {
                    _broccoloPopup.Update(elapsed);

                    if (_broccoloPopup.PopupObject.IsCompleted)
                    {
                        _broccoloPopup = null;
                    }
                }

                if (_beanPopup != null)
                {
                    _beanPopup.Update(elapsed);

                    if (_beanPopup.PopupObject.IsCompleted)
                    {
                        _beanPopup = null;
                    }
                }

                _statusBar?.Update(gametime);

                if (_deadExplosion.Started)
                {
                    _deadExplosion.Update(gametime);
                }
            }

            if (ScoreMetri % 50 == 0)
            {
                if (_oldScoreMetri != ScoreMetri)
                {
                    (Player)?.IncreaseMoveSpeed();
                    _background.IncreaseParallaxSpeed();
                    if (LarghezzaBuchi.Y < 1000)
                    {
                        _larghezzaBuchi.X += 80.0f;
                        _larghezzaBuchi.Y += 80.0f;
                    }
                }
            }
            _oldScoreMetri = ScoreMetri;

            if (IsTimeToGameOver())
            {
                _gameOrchestrator.SetGameOverState(VerdureMangiate(), ScoreMetri, _statusBar.TotalJumps);
            }
        }

        internal void StopMusic()
        {
            _soundManager.StopSounds();
        }

        public void CheckPlayerGemmaCollision()
        {
            if (_gemme.CheckCollisionWithPlayer(Player))
            {
                PlayerCollidedWithNormalGemma();
            }

            if ((_gemme).CheckJalapenoCollisionWithPlayer(Player))
            {
                _statusBar.ComputeJalapenos();
                (Player).ActivateJalapenos();
                (_soundManager).PlayJalapeno();
                JalapenosModeActive = true;
                _jalapenoPopup = new PopupText()
                {
                    Text = _jalapenoPopupText,
                    DrawingInfos = new DrawingInfos()
                    {
                        Position = Player.Position
                    },
                    PopupObject = new PopupObject(
                        TimeSpan.FromSeconds(3),
                        new Vector2(200, 300),
                        Color.Red,
                        260f)
                };
                _jalapenoPopup.PopupObject.Popup();

            }
            else if ((_gemme).CheckBroccoloCollisionWithPlayer(Player))
            {
                _statusBar.ComputeBroccolo();
                (Player).ActivateBroccolo();
                (_soundManager).PlayShit();
                MerdaModeActive = true;
                _broccoloPopup = new PopupText()
                {
                    Text = _broccoloPopupText,
                    DrawingInfos = new DrawingInfos()
                    {
                        Position = Player.Position
                    },
                    PopupObject = new PopupObject(
                        TimeSpan.FromSeconds(3),
                        new Vector2(200, 300),
                        Color.ForestGreen,
                        260f)
                };
                _broccoloPopup.PopupObject.Popup();
            }
        }

        public void Resume()
            => ResumeFromPause();

        private void MakePlayerDead()
        {
            Player.Dead = true;

            if (HighScore < ScoreMetri)
            {
                SetHighScore(ScoreMetri);
                PopupRecord();
            }
        }

        private void PopupRecord()
        {
            if (Player.Dead)
            {
                _recordMetersText = "RECORD!";
            }

            var recordPosition = Player.Dead
                ? Player.Position
                : Player.Position + new Vector2(600, 0);

            _recordMetersPopup = new PopupText()
            {
                Text = _recordMetersText,
                DrawingInfos = new DrawingInfos()
                {
                    Position = Player.Position
                },
                PopupObject = new PopupObject(
                    TimeSpan.FromSeconds(2.8),
                    new Vector2(200, 300),
                    Color.LimeGreen,
                    260f)
            };

            _recordMetersPopup.PopupObject.Popup();
            _recordMetersNotified = true;
        }

        private void CheckDead()
        {
            if (!FallSoundActive)
            {
                if (Player.Position.Y > _playerCamera.ViewPortHeight)
                {
                    MakePlayerDead();
                    (_soundManager)?.PlayFall();
                    FallSoundActive = true;
                }
            }
            else if (!_deadExplosion.Started)
            {
                if (_soundManager.HasFallFinished())
                {
                    _deadExplosion.Explode(Player.Position, false, _soundManager);
                    _statusBar.SetInfart();
                }
            }
            else if (_deadExplosion.Finished)
            {
                _forceToFinish = true;
            }

            if (_statusBar.IsInfarting() && !Player.Dead)
            {
                _deadExplosion.Explode(Player.Position, true, _soundManager);
                MakePlayerDead();
            }
        }

        public bool IsTimeToGameOver()
        {
            if (_forceToFinish)
            {
                return true;
            }

            if (_deadExplosion.Started)
            {
                return _deadExplosion.Finished;
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

        private void LerpCameraPosition(float newCameraX, float newCameraY)
        {
            _playerCamera.Position = new Vector2(
              MathHelper.Lerp(_playerCamera.Position.X, newCameraX, 0.08f),
              MathHelper.Lerp(_playerCamera.Position.Y, newCameraY, 0.08f));
        }

        public Player Player { get; private set; }
        public bool IsGameOver => IsTimeToGameOver();

        private void RepositionCamera()
        {
            float playerCameraY;
            float playerCameraX;

            playerCameraX = Player.Position.X - 150;
            playerCameraY = Player.Position.Y - 200;

            if (playerCameraY < _gameCameraHLimit)
            {
                playerCameraY = _gameCameraHLimit;
            }
            else if (playerCameraY > 0)
            {
                playerCameraY = 0;
            }

            LerpCameraPosition(playerCameraX, playerCameraY);
        }

        private void DrawUi(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            _scoreString.Clear();
            _scoreString.AppendNumber(ScoreMetri).Append(_metriString);
            spriteBatch.DrawString(_font, _scoreString, _scoreStringPosition, Color.DarkBlue, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 1f);
            _statusBar.Draw(spriteBatch);

            if (_jalapenoPopup != null)
            {
                spriteBatch.DrawString(_font, _jalapenoPopup.Text, _jalapenoPopup.DrawingInfos);
            }

            if (_broccoloPopup != null)
            {
                spriteBatch.DrawString(_font, _broccoloPopup.Text, _broccoloPopup.DrawingInfos);
            }

            if (_recordMetersPopup != null)
            {
                spriteBatch.DrawString(_font, _recordMetersPopup.Text, _recordMetersPopup.DrawingInfos);
            }

            if (_beanPopup != null)
            {
                spriteBatch.DrawString(_font, _beanPopup.Text, _beanPopup.DrawingInfos);
            }

            spriteBatch.End();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Matrix cameraTransformation = _playerCamera.GetTransformation();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraTransformation);

            _background.Draw(spriteBatch);
            _ground.Draw(spriteBatch);
            _gemme.Draw(spriteBatch);

            spriteBatch.End();

            if (!_deadExplosion.Started)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, cameraTransformation);
                Player.DrawParticles(spriteBatch);
                spriteBatch.End();
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraTransformation);

            if (!_deadExplosion.Started)
            {
                Player.Draw(spriteBatch);
            }

            _background.DrawSpecial(spriteBatch);

            if (HighScore != 0
               && Player.Position.X >= _highScorePosition.X - ResolutionWidth
               && !_newHighScore)
            {
                if (Player.Position.X >= _highScorePosition.X)
                {
                    _newHighScore = true;
                }
                else
                {
                    // E' la sbarra verticale
                    spriteBatch.DrawRectangle(_highScorePosition, _highScoreColor);
                }
            }

            if (_deadExplosion.Started)
            {
                _deadExplosion.Draw(spriteBatch);
            }

            spriteBatch.End();

            DrawUi(spriteBatch);
        }
    }
}