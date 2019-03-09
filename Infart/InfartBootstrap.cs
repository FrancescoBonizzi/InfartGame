using FbonizziMonoGame.Assets;
using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Input;
using FbonizziMonoGame.Input.Abstractions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.StringsLocalization;
using FbonizziMonoGame.StringsLocalization.Abstractions;

using Infart.Assets;
using Infart.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Infart
{
    public class InfartBootstrap : Game, IFbonizziGame
    {
        private const string _gameName = "INFART";

        private enum RunningStates
        {
            Splashscreen,
            Running
        }

        private readonly Uri _rateMeUri;

        private RunningStates _currentState;

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        private SpriteBatch _spriteBatch;

        private SplashScreenLoader _splashScreenAssetsLoader;

        private readonly ISettingsRepository _settingsRepository;

        private readonly IWebPageOpener _webPageOpener;

        private List<IInputListener> _inputListeners;

        private readonly CultureInfo _gameCulture;

        public event EventHandler ExitGameRequested;

        private IScreenTransformationMatrixProvider _matrixScaleProvider;

        private readonly ITextFileLoader _textFileAssetsLoader;

        private GameOrchestrator _orchestrator;

        private readonly SoundManager _soundManager;

        private readonly bool _isPc;

        private readonly ILocalizedStringsRepository _localizedStringsRepository;

        public InfartBootstrap()
        {
        }

        public InfartBootstrap(
           ITextFileLoader textFileAssetsLoader,
           ISettingsRepository settingsRepository,
           IWebPageOpener webPageOpener,
           CultureInfo gameCulture,
           bool isPc,
           bool isFullScreen,
           Uri rateMeUri,
           int? deviceWidth = null,
           int? deviceHeight = null)
        {
            _isPc = isPc;

            Window.Title = _gameName;

            _rateMeUri = rateMeUri;
            _currentState = RunningStates.Splashscreen;

            _textFileAssetsLoader = textFileAssetsLoader;
            _settingsRepository = settingsRepository;
            _webPageOpener = webPageOpener;
            _gameCulture = gameCulture;

            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitDown,
                IsFullScreen = isFullScreen
            };

            if (deviceWidth != null && deviceHeight != null)
            {
                GraphicsDeviceManager.PreferredBackBufferWidth = deviceWidth.Value;
                GraphicsDeviceManager.PreferredBackBufferHeight = deviceHeight.Value;
            }

            _localizedStringsRepository = new InMemoryLocalizedStringsRepository(new Dictionary<string, string>());
        }

        protected override void Initialize()
        {
            _matrixScaleProvider = new DynamicScalingMatrixProvider(
                  new GameWindowScreenSizeChangedNotifier(Window),
                  GraphicsDeviceManager.GraphicsDevice,
                  800, 480,
                  !GraphicsDeviceManager.IsFullScreen);
            base.Initialize();
        }

        private void LoadGameAssets()
        {
            new GameStringsLoader(_localizedStringsRepository, _gameCulture);
            
            var loaderEpisodio1 = new Loader(Content, GraphicsDevice);
            var soundManager = new SoundManager(true, loaderEpisodio1);
            var gameFactory = new Func<InfartGame>(
                () => new InfartGame(
                    loaderEpisodio1,
                    //        new Loader_menu(Content),
                    soundManager,
                    100, "Metri", "Pausa"));

            _orchestrator = new GameOrchestrator(
                gameFactory,

                GraphicsDevice,
                _matrixScaleProvider,
                _soundManager,
                _webPageOpener);



            _inputListeners = new List<IInputListener>();

            if (_isPc)
            {
                var mouseListener = new MouseListener(_matrixScaleProvider);
                mouseListener.MouseDown += MouseListener_MouseClicked;
                _inputListeners.Add(mouseListener);
            }
            else
            {
                var touchListener = new TouchListener(_matrixScaleProvider);
                touchListener.TouchStarted += TouchListener_TouchEnded;

                var gamepadListener = new GamePadListener();
                gamepadListener.ButtonDown += GamepadListener_ButtonDown;

                _inputListeners.Add(touchListener);
                _inputListeners.Add(gamepadListener);
            }


            var keyboardListener = new KeyboardListener();
            keyboardListener.KeyPressed += KeyboardListener_KeyPressed;
            _inputListeners.Add(keyboardListener);
        }

        private void GamepadListener_ButtonDown(object sender, GamePadEventArgs e)
        {
            if (e.Button == Buttons.Back)
            {
                _orchestrator.Back();
                if (_orchestrator.ShouldEndApplication)
                {
                    if (ExitGameRequested != null)
                        ExitGameRequested(this, EventArgs.Empty);
                    else
                        Exit();
                }
            }
        }

        private void KeyboardListener_KeyPressed(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.Escape)
            {
                _orchestrator.Back();
                if (_orchestrator.ShouldEndApplication)
                {
                    if (ExitGameRequested != null)
                        ExitGameRequested(this, EventArgs.Empty);
                    else
                        Exit();
                }
            }
            else if (e.Key == Keys.Back)
            {
                _orchestrator.Back();
                if (_orchestrator.ShouldEndApplication)
                {
                    if (ExitGameRequested != null)
                        ExitGameRequested(this, EventArgs.Empty);
                    else
                        Exit();
                }
            }
        }

        private void TouchListener_TouchEnded(object sender, TouchEventArgs e)
            => _orchestrator.HandleTouchInput(e.Position);

        private void MouseListener_MouseClicked(object sender, MouseEventArgs e)
            => _orchestrator.HandleTouchInput(e.Position);

        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _splashScreenAssetsLoader = new SplashScreenLoader(
                LoadGameAssets,
                Content,
                "Splashscreen");
            _splashScreenAssetsLoader.Load();

            _splashScreenAssetsLoader.Completed += _splashScreenAssetsLoader_Completed;
        }

        private void _splashScreenAssetsLoader_Completed(object sender, EventArgs e)
        {
            _splashScreenAssetsLoader = null;
            _orchestrator.Start();
            _currentState = RunningStates.Running;
        }

        public void Pause()
            => _orchestrator?.Pause();

        public void Resume()
            => _orchestrator?.Resume();

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            var elapsed = gameTime.ElapsedGameTime;

            if (_currentState == RunningStates.Splashscreen)
            {
                _splashScreenAssetsLoader.Update(elapsed);
                return;
            }

            foreach (var listener in _inputListeners)
                listener.Update(gameTime);

            _orchestrator.Update(elapsed);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!IsActive)
                return;

            if (_currentState == RunningStates.Splashscreen)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin(transformMatrix: _matrixScaleProvider.ScaleMatrix);
                _splashScreenAssetsLoader.Draw(_spriteBatch);
                _spriteBatch.End();
                return;
            }

            _orchestrator.Draw(_spriteBatch, GraphicsDevice);

            if (_isPc)
            {
                if (!_orchestrator.IsPaused)
                {








                }
            }

            base.Draw(gameTime);
        }
    }
}
