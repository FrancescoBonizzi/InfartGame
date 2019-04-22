using FbonizziMonoGameUWP;
using MonoGame.Framework;
using System;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Infart.UWP
{
    public sealed partial class MainPage : Page
    {
        private const int _width = 800;
        private const int _height = 480;

        private readonly InfartBootstrap _game;

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(_width, _height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            var launchArguments = string.Empty;

            _game = XamlGame<InfartBootstrap>.Create(
            gameConstructor: () => new InfartBootstrap(
                textFileAssetsLoader: new UwpTextFileImporter(),
                settingsRepository: new UwpSettingsRepository(),
                webPageOpener: new UwpWebPageOpener(Window.Current),
                gameCulture: CultureInfo.CurrentCulture,
                isPc: true,
                isFullScreen: false,
                rateMeUri: new Uri("https://www.microsoft.com/store/apps/9WZDNCRDHRJH"),
                deviceWidth: _width, deviceHeight: _height),
            window: Window.Current.CoreWindow,
            launchParameters: launchArguments,
            swapChainPanel: swapChainPanel);

            Window.Current.VisibilityChanged += Current_VisibilityChanged;
        }

        private void Current_VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            if (!e.Visible)
            {
                _game.Pause();
            }
            else
            {
                _game.Resume();
            }
        }

    }
}
