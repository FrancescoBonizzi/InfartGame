using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.StringsLocalization.Abstractions;
using FbonizziMonoGame.TransformationObjects;
using Infart.Assets;
using Infart.Pages;
using Infart.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Infart
{
    public class GameOrchestrator
    {
        public enum GameStates
        {
            Menu,
            Playing,
            GameOver,
            Score
        }

        private GameStates? _currentState;

        private readonly Func<InfartGame> _gameFactory;
        private InfartGame _game;

        private readonly Func<MainMenuPage> _menuFactory;
        private MainMenuPage _menu;

        private readonly Func<ScorePage> _scoreFactory;
        private ScorePage _score;

        private GameOverPage _gameOver;
        public event EventHandler GameOver;

        private readonly AssetsLoader _assets;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IWebPageOpener _webPageOpener;
        private readonly ILocalizedStringsRepository _localizedStringsRepository;

        private readonly GraphicsDevice _graphicsDevice;
        private readonly FadeObject _stateTransition;
        private Action _afterTransitionAction;

        private RenderTarget2D _renderTarget;
        private readonly IScreenTransformationMatrixProvider _matrixScaleProvider;

        public bool ShouldEndApplication { get; set; }
        public bool IsPaused { get; set; }

        private readonly TimeSpan _fadeDuration = TimeSpan.FromMilliseconds(800);
        private readonly Uri _aboutUri = new Uri("https://www.fbonizzi.it");

        public GameOrchestrator(
             Func<InfartGame> gameFactory,
             Func<MainMenuPage> menuFactory,
             Func<ScorePage> scoreFactory,
             GraphicsDevice graphicsDevice,
             AssetsLoader assets,
             ISettingsRepository settingsRepository,
             IScreenTransformationMatrixProvider matrixScaleProvider,
             IWebPageOpener webPageOpener,
             ILocalizedStringsRepository localizedStringsRepository)
        {
            _gameFactory = gameFactory ?? throw new ArgumentNullException(nameof(gameFactory));
            _menuFactory = menuFactory ?? throw new ArgumentNullException(nameof(menuFactory));
            _scoreFactory = scoreFactory ?? throw new ArgumentNullException(nameof(scoreFactory));

            _webPageOpener = webPageOpener ?? throw new ArgumentNullException(nameof(webPageOpener));
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));

            _assets = assets ?? throw new ArgumentNullException(nameof(assets));
            _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
            _localizedStringsRepository = localizedStringsRepository ?? throw new ArgumentNullException(nameof(localizedStringsRepository));

            _matrixScaleProvider = matrixScaleProvider ?? throw new ArgumentNullException(nameof(matrixScaleProvider));
            if (_matrixScaleProvider is DynamicScalingMatrixProvider)
            {
                (_matrixScaleProvider as DynamicScalingMatrixProvider).ScaleMatrixChanged += GameOrchestrator_ScaleMatrixChanged;
            }
            RegenerateRenderTarget();

            ShouldEndApplication = false;

            _stateTransition = new FadeObject(_fadeDuration, Color.White);
            _stateTransition.FadeOutCompleted += _stateTransition_FadeOutCompleted;
        }

        private void GameOrchestrator_ScaleMatrixChanged(object sender, EventArgs e)
            => RegenerateRenderTarget();

        public void RegenerateRenderTarget()
        {
            _renderTarget = new RenderTarget2D(
                _graphicsDevice,
                _matrixScaleProvider.VirtualWidth,
                _matrixScaleProvider.VirtualHeight);
        }

        private void _stateTransition_FadeOutCompleted(object sender, EventArgs e)
            => _afterTransitionAction();

        public void SetScoreState()
        {
            if (_currentState == GameStates.Score)
                return;

            if (_stateTransition.IsFading)
                return;

            ShouldEndApplication = false;
            _stateTransition.FadeOut();
            _afterTransitionAction = new Action(
                () =>
                {
                    _stateTransition.FadeIn();

                    _currentState = GameStates.Score;
                    _game = null;
                    _menu = null;
                    _score = _scoreFactory();
                });
        }

        public void SetMenuState()
        {
            if (_currentState == GameStates.Menu)
                return;

            if (_stateTransition.IsFading)
                return;

            _stateTransition.FadeOut();
            _afterTransitionAction = new Action(
                () =>
                {
                    _stateTransition.FadeIn();

                    if (_currentState == GameStates.Playing && _game.IsGameOver)
                    {
                        // Significa che sono uscito dal gioco
                        // perché ho perso
                        GameOver?.Invoke(this, EventArgs.Empty);
                    }

                    _currentState = GameStates.Menu;
                    _game = null;
                    _score = null;
                    _menu = _menuFactory();
                });
        }

        public void SetAboutState()
            => _webPageOpener.OpenWebpage(_aboutUri);

        public void SetGameState()
        {
            if (_currentState == GameStates.Playing)
                return;

            if (_stateTransition.IsFading)
                return;

            ShouldEndApplication = false;
            _stateTransition.FadeOut();
            _afterTransitionAction = new Action(
                () =>
                {
                    _stateTransition.FadeIn();

                    _currentState = GameStates.Playing;
                    _game = _gameFactory();
                    _menu = null;
                    _score = null;
                });
        }

        public void SetGameOverState(
            int thisGameNumberOfVegetablesEaten,
            int thisGameNumberOfMeters,
            int thisGameNumberOfFarts)
        {
            if (_currentState == GameStates.GameOver)
                return;

            if (_stateTransition.IsFading)
                return;

            ShouldEndApplication = false;
            _stateTransition.FadeOut();
            _afterTransitionAction = new Action(
                () =>
                {
                    _stateTransition.FadeIn();

                    _currentState = GameStates.GameOver;
                    _game = null;
                    _menu = null;
                    _score = null;
                    _gameOver = new GameOverPage(
                        matrixScaleProvider: _matrixScaleProvider,
                        assets: _assets,
                        settingsRepository: _settingsRepository,
                        thisGameNumberOfVegetablesEaten: thisGameNumberOfVegetablesEaten,
                        thisGameNumberOfMeters: thisGameNumberOfMeters,
                        thisGameNumberOfFarts: thisGameNumberOfFarts,
                        localizedStringsRepository: _localizedStringsRepository);
                });
        }

        public void HandleInput(Vector2? touchLocation = null)
        {
            switch (_currentState)
            {
                case GameStates.Menu:
                    if (touchLocation == null)
                        return;

                    _menu.HandleInput(touchLocation.Value, this);
                    break;

                case GameStates.Playing:
                    _game.HandleInput();
                    break;

                case GameStates.GameOver:
                    _gameOver.HandleInput(this);
                    break;

                case GameStates.Score:
                    SetMenuState();
                    break;
            }
        }

        public void Update(TimeSpan elapsed)
        {
            if (IsPaused)
                return;

            if (_stateTransition.IsFading)
            {
                _stateTransition.Update(elapsed);
            }

            switch (_currentState)
            {
                case GameStates.Menu:
                    _menu.Update(elapsed);
                    break;

                case GameStates.Playing:
                    _game.Update(elapsed);
                    break;

                case GameStates.GameOver:
                    _gameOver.Update(elapsed);
                    break;

                case GameStates.Score:
                    _score.Update(elapsed);
                    break;
            }
        }

        public void Resume()
        {
            _game?.Resume();
            IsPaused = false;
        }

        public void Pause()
        {
            _game?.Pause();
            IsPaused = true;
        }

        public void TogglePause()
        {
            if (IsPaused)
                Resume();
            else Pause();
        }

        public void Back()
        {
            switch (_currentState)
            {
                case GameStates.Menu:
                    ShouldEndApplication = true;
                    break;

                case GameStates.Playing:
                    _game.StopMusic();
                    SetMenuState();
                    break;

                case GameStates.GameOver:
                case GameStates.Score:
                    SetMenuState();
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            if (IsPaused)
                return;

            // Disegno tutto su un render target...
            graphics.SetRenderTarget(_renderTarget);
            graphics.Clear(Color.Black);

            switch (_currentState)
            {
                case GameStates.Menu:
                    _menu.Draw(spriteBatch);
                    break;

                case GameStates.Playing:
                    _game.Draw(spriteBatch);
                    break;

                case GameStates.GameOver:
                    _gameOver.Draw(spriteBatch);
                    break;

                case GameStates.Score:
                    _score.Draw(spriteBatch);
                    break;
            }

            // ...per poter fare il fade dei vari componenti in modo indipendente
            graphics.SetRenderTarget(null);
            graphics.Clear(Color.Black);
            spriteBatch.Begin(transformMatrix: _matrixScaleProvider.ScaleMatrix);
            spriteBatch.Draw(_renderTarget, Vector2.Zero, _stateTransition.OverlayColor);
            spriteBatch.End();
        }
    }
}