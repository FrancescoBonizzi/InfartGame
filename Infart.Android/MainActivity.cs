using Android.App;
using Android.Content.PM;
using Android.Views;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGameAndroid;
using System;
using System.Globalization;

namespace Infart.Android
{
    [Activity(
        Label = "INFART",
        Icon = "@drawable/icon",
        MainLauncher = true,
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.Orientation |
        ConfigChanges.KeyboardHidden |
        ConfigChanges.Keyboard)]
    public class MainActivity : FbonizziMonoGameActivity
    {
        private InfartBootstrap _game;

        protected override IFbonizziGame StartGame(CultureInfo cultureInfo)
        {
            _game = new InfartBootstrap(
                textFileAssetsLoader: new AndroidTextFileImporter(Assets),
                settingsRepository: new AndroidSettingsRepository(this),
                webPageOpener: new AndroidWebPageOpener(this),
                gameCulture: cultureInfo,
                isFullScreen: true,
                isPc: false,
                rateMeUri: new Uri("market://details?id=com.francescobonizzi.infart"));

            _game.Run();
            SetContentView((View)_game.Services.GetService(typeof(View)));

            return _game;
        }

        protected override void DisposeGame()
        {
            _game?.Dispose();
            _game = null;
        }
    }
}