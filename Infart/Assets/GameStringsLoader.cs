using FbonizziMonoGame.StringsLocalization.Abstractions;
using System.Globalization;

namespace Infart.Assets
{
    public class GameStringsLoader
    {
        public const string PlayButtonString = "PlayButtonStringKey";
        public const string ScoreButtonString = "ScoreButtonStringKey";
        public const string FartButtonString = "FartButtonStringKey";

        public const string ScorePageTitleString = "ScorePageTitleStringKey";
        public const string BestAliveTimeString = "BestAliveTimeStringKey";
        public const string BestHamburgersEatenScoreKey = "BestHamburgersEatenScoreKey";
        public const string BestFartsScoreKey = "BestFartsScoreKey";

        public const string NumberOfMetersString = "NumberOfMetersKey";
        public const string NumberOfHamburgersEaten = "NumberOfHamburgersEaten";
        public const string NumbersOfFartsString = "NumbersOfFartsString";

        public const string MetriTimeString = "MetriStringKey";

        public GameStringsLoader(ILocalizedStringsRepository localizedStringsRepository, CultureInfo cultureInfo)
        {
            if (cultureInfo.TwoLetterISOLanguageName == "it")
            {
                localizedStringsRepository.AddString(PlayButtonString, "gioca");
                localizedStringsRepository.AddString(ScoreButtonString, "punteggi");
                localizedStringsRepository.AddString(FartButtonString, "scoreggia");

                localizedStringsRepository.AddString(ScorePageTitleString, "Punteggi");
                localizedStringsRepository.AddString(BestAliveTimeString, "Miglior tempo in vita: ");
                localizedStringsRepository.AddString(BestHamburgersEatenScoreKey, "Minor numero di hamburger mangiati: ");
                localizedStringsRepository.AddString(BestFartsScoreKey, "Maggior numero di scoregge: ");

                localizedStringsRepository.AddString(MetriTimeString, " metri");
                localizedStringsRepository.AddString(NumberOfMetersString, "Numero di metri: ");
            }
            else
            {
                localizedStringsRepository.AddString(PlayButtonString, "play");
                localizedStringsRepository.AddString(ScoreButtonString, "score");
                localizedStringsRepository.AddString(FartButtonString, "fart");

                localizedStringsRepository.AddString(ScorePageTitleString, "Score");
                localizedStringsRepository.AddString(BestAliveTimeString, "Best alive time: ");
                localizedStringsRepository.AddString(BestHamburgersEatenScoreKey, "Lowest number of hamburgers eaten: ");
                localizedStringsRepository.AddString(BestFartsScoreKey, "Highest number of farts: ");

                localizedStringsRepository.AddString(MetriTimeString, " meters");
                localizedStringsRepository.AddString(NumberOfMetersString, "Number of meters: ");
            }
        }
    }
}