using FbonizziMonoGame.Assets;
using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Input;
using FbonizziMonoGame.Input.Abstractions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.StringsLocalization;
using FbonizziMonoGame.StringsLocalization.Abstractions;
using FbonizziMonoGame.UI.RateMe;
using Infart.Assets;
using Infart.Pages;
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
        private const string GameName = "INFART";

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

        private readonly ITextFileLoader _textFileAssetsLoader;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IWebPageOpener _webPageOpener;
        private List<IInputListener> _inputListeners;

        private readonly CultureInfo _gameCulture;

        public event EventHandler ExitGameRequested;

        private AssetsLoader _assetsLoader;
        private IScreenTransformationMatrixProvider _matrixScaleProvider;

        private Sprite _mousePointer;

        private GameOrchestrator _orchestrator;
        private SoundManager _soundManager;
        private readonly bool _isPc;

        private readonly ILocalizedStringsRepository _localizedStringsRepository;

        // Just for MonoGame.Framework.WindowsUniversal bootstrapping requirement
        public InfartBootstrap() { }

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

            Window.Title = GameName;

            _rateMeUri = rateMeUri;
            _currentState = RunningStates.Splashscreen;

            _textFileAssetsLoader = textFileAssetsLoader;
            _settingsRepository = settingsRepository;
            _webPageOpener = webPageOpener;
            _gameCulture = gameCulture;

            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeLeft,
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
            
            _assetsLoader = new AssetsLoader(Content);
            _mousePointer = _assetsLoader.OtherSprites["manina"];
            _soundManager = new SoundManager(_assetsLoader);

            var gameFactory = new Func<InfartGame>(
                () => new InfartGame(
                    _assetsLoader,
                    _soundManager,
                    _orchestrator,
                    _settingsRepository,
                    _localizedStringsRepository));

            var dialogDefinition = new Rectangle(
                _matrixScaleProvider.VirtualWidth / 2 - 350, 24, 700, _matrixScaleProvider.VirtualHeight - 60);

            var rateMeDialog = new RateMeDialog(
                launchesUntilPrompt: 2,
                maxRateShowTimes: 2,
                rateAppUri: _rateMeUri,
                dialogDefinition: dialogDefinition,
                font: _assetsLoader.Font,
                localizedStringsRepository: _localizedStringsRepository,
                rateMeDialogStrings: _gameCulture.TwoLetterISOLanguageName == "it" 
                    ? (RateMeDialogStrings)new DefaultItalianRateMeDialogStrings(GameName) 
                    : (RateMeDialogStrings)new DefaultEnglishRateMeDialogStrings(GameName),
                webPageOpener: _webPageOpener,
                settingsRepository: _settingsRepository,
                buttonADefinition: new Rectangle(
                    dialogDefinition.X + 150,
                    dialogDefinition.Y + 350,
                    140, 40),
                buttonBDefinition: new Rectangle(
                    dialogDefinition.X + dialogDefinition.Width - 140 - 150,
                    dialogDefinition.Y + 350,
                    140, 40),
                backgroundColor: Color.DarkGray.WithAlpha(1f),
                buttonsBackgroundColor: (new Color(255, 18, 67)).WithAlpha(1f),
                buttonsShadowColor: Color.Black,
                backgroundShadowColor: Color.Black.WithAlpha(1f),
                titleColor: Color.Black,
                buttonsTextColor: new Color(255, 234, 135),
                titlePositionOffset: new Vector2(dialogDefinition.Width / 2, 80f),
                buttonTextPadding: 40f,
                titlePadding: 160f);

            var menuFactory = new Func<MainMenuPage>(
                () => new MainMenuPage(
                    _assetsLoader,
                    rateMeDialog,
                    _soundManager,
                    _settingsRepository,
                    _matrixScaleProvider,
                    _localizedStringsRepository));

            var scoreFactory = new Func<ScorePage>(
                () => new ScorePage(
                    _assetsLoader,
                    _settingsRepository,
                    _matrixScaleProvider,
                    _localizedStringsRepository));

            _orchestrator = new GameOrchestrator(
                gameFactory,
                menuFactory,
                scoreFactory,
                GraphicsDevice,
                _assetsLoader,
                _settingsRepository,
                _matrixScaleProvider,
                _webPageOpener,
                _localizedStringsRepository);

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
            else
            {
                _orchestrator.HandleInput(null);
            }
        }

        private void TouchListener_TouchEnded(object sender, TouchEventArgs e)
            => _orchestrator.HandleInput(e.Position);

        private void MouseListener_MouseClicked(object sender, MouseEventArgs e)
            => _orchestrator.HandleInput(e.Position);

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
            _orchestrator.SetMenuState();
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
                    var mouseState = Mouse.GetState();
                    var mousePosition = new Vector2(mouseState.X - 32, mouseState.Y);
                    if (mouseState.X != 0 && mouseState.Y != 0)
                    {
                        _spriteBatch.Begin();
                        _spriteBatch.Draw(_mousePointer.Sheet, mousePosition, _mousePointer.SourceRectangle, Color.White);
                        _spriteBatch.End();
                    }
                }
            }

            base.Draw(gameTime);
        }
    }
}