using FbonizziMonoGame.StringsLocalization.Abstractions;
using System.Globalization;

namespace Infart.Assets
{
    public class GameStringsLoader
    {
        // public const string YellowColorKey = "ColorsYellowKey";

        public GameStringsLoader(ILocalizedStringsRepository localizedStringsRepository, CultureInfo cultureInfo)
        {
            if (cultureInfo.TwoLetterISOLanguageName == "it")
            {
                // localizedStringsRepository.AddString(YellowColorKey, "Giallo");
            }
            else
            {

            }
        }
    }
}
