using FbonizziMonoGame.StringsLocalization.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
