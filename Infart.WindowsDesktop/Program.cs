using FbonizziMonoGameWindowsDesktop;
using System;
using System.Globalization;

namespace Infart.WindowsDesktop
{
#if WINDOWS || LINUX

    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (var game = new InfartBootstrap(
                textFileAssetsLoader: new WindowsTextFileImporter(),
                settingsRepository: new FileWindowsSettingsRepository("infart-settings.txt"),
                webPageOpener: new WindowsWebSiteOpener(),
                gameCulture: CultureInfo.CreateSpecificCulture("it-IT"),
                isPc: true,
                isFullScreen: false,
                rateMeUri: new Uri("https://www.fbonizzi.it"),
                deviceWidth: 800, deviceHeight: 480))
            {
                game.Run();
            }
        }
    }

#endif
}