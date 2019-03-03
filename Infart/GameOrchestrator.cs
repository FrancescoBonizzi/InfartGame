using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.TransformationObjects;
using fge;
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
            Playing
        }

        private GameStates _currentState;

        private readonly Func<InfartGame> _gameFactory;
        private InfartGame _game;

        //private readonly Func<MainMenuPage> _menuFactory;
        //private MainMenuPage _menu;

        private readonly IWebPageOpener _webPageOpener;
        private readonly GraphicsDevice _graphicsDevice;
        private FadeObject _stateTransition;
        private Action _afterTransitionAction;

        private RenderTarget2D _renderTarget;
        private readonly IScreenTransformationMatrixProvider _matrixScaleProvider;

        public bool ShouldEndApplication { get; set; }
        public bool IsPaused { get; set; }

        private readonly TimeSpan _fadeDuration = TimeSpan.FromMilliseconds(800);
        private readonly Uri _aboutUri = new Uri("https://www.fbonizzi.it");

        public GameOrchestrator(
             Func<InfartGame> gameFactory,
             //Func<MainMenuPage> menuFactory,
             GraphicsDevice graphicsDevice,
             IScreenTransformationMatrixProvider matrixScaleProvider,
             SoundManager soundManager,
             IWebPageOpener webPageOpener)
        {
            _gameFactory = gameFactory ?? throw new ArgumentNullException(nameof(gameFactory));
            //_menuFactory = menuFactory ?? throw new ArgumentNullException(nameof(menuFactory));
            _webPageOpener = webPageOpener ?? throw new ArgumentNullException(nameof(webPageOpener));
            //_soundManager = soundManager ?? throw new ArgumentNullException(nameof(soundManager));
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));

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

        public void Start()
        {
            //_currentState = GameStates.Menu;
            //_menu = _menuFactory();
            //_stateTransition.FadeIn();
            //_soundManager.PlayMenu();

            _currentState = GameStates.Playing;
            _game = _gameFactory();
            _stateTransition.FadeIn();
        }

        public void SetMenuState()
        {
            //if (_currentState == GameStates.Menu)
            //    return;

            //if (_stateTransition.IsFading)
            //    return;

            //_soundManager.StopSounds();
            //_soundManager.PlayMenu();

            //_stateTransition.FadeOut();
            //_afterTransitionAction = new Action(
            //    () =>
            //    {
            //        _stateTransition.FadeIn();

            //        _currentState = GameStates.Menu;
            //        _game = null;
            //        _menu = _menuFactory();
            //    });
        }

        public void Replay()
        {
            ShouldEndApplication = false;
            _stateTransition.FadeOut();
            _afterTransitionAction = new Action(
                () =>
                {
                    _stateTransition.FadeIn();
                    _currentState = GameStates.Playing;
                    _game = _gameFactory();
                    //_menu = null;
                });
        }

        public void HandleTouchInput(Vector2? touchLocation = null)
        {
            if (touchLocation == null)
                return;

            switch (_currentState)
            {
                case GameStates.Menu:
                    //_menu.HandleInput(touchLocation.Value);
                    break;

                case GameStates.Playing:
                    _game.HandleInput(touchLocation.Value);
                    break;
            }
        }

        public void SetGameState()
        {
            if (_currentState == GameStates.Playing)
                return;

            if (_stateTransition.IsFading)
                return;

            //_soundManager.StopSounds();
            //_soundManager.PlayPlaying();

            Replay();
        }

        public void RegenerateRenderTarget()
        {
            _renderTarget = new RenderTarget2D(
                _graphicsDevice,
                _matrixScaleProvider.RealScreenWidth,
                _matrixScaleProvider.RealScreenHeight);
        }

        public void SetAboutState()
            => _webPageOpener.OpenWebpage(_aboutUri);

        private void _stateTransition_FadeOutCompleted(object sender, EventArgs e)
            => _afterTransitionAction();

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
                    throw new NotImplementedException();
                    //_menu.Update(elapsed);
                    break;

                case GameStates.Playing:
                    _game.Update(elapsed);
                    break;
            }
        }

        public void Resume()
        {
            _game?.Resume();
            IsPaused = false;
         //   _soundManager.ResumeAll();
        }

        public void Pause()
        {
            _game?.Pause();
            IsPaused = true;
      //      _soundManager.PauseAll();
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
                    //ShouldEndApplication = true;
                    //_soundManager.StopSounds();
                    throw new NotImplementedException();
                    break;

                case GameStates.Playing:
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
                    throw new NotImplementedException();
                    //_menu.Draw(spriteBatch);
                    break;

                case GameStates.Playing:
                    _game.Draw(spriteBatch);
                    break;
            }

            // ...per poter fare il fade dei vari componenti in modo indipendente
            graphics.SetRenderTarget(null);
            graphics.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(_renderTarget, Vector2.Zero, _stateTransition.OverlayColor);
            spriteBatch.End();
        }
    }
}
