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
        public const string BestNumberOfMetersScoreKey = "BestNumberOfMetersScoreKey";
        public const string BestVegetablesEatenScoreKey = "BestHamburgersEatenScoreKey";
        public const string BestFartsScoreKey = "BestFartsScoreKey";

        public const string NumberOfMetersString = "NumberOfMetersKey";
        public const string NumberOfVegetablesEaten = "NumberOfHamburgersEaten";
        public const string NumbersOfFartsString = "NumbersOfFartsString";

        public const string MetriTimeString = "MetriStringKey";
        public const string MetriRecordPopupString = "MetriRecordPopupString";

        public const string BeanPowerupText = "BeanPowerupTextKey";
        public const string BroccoloPowerupText = "BroccoloPowerupTextKey";

        public GameStringsLoader(ILocalizedStringsRepository localizedStringsRepository, CultureInfo cultureInfo)
        {
            if (cultureInfo.TwoLetterISOLanguageName == "it")
            {
                localizedStringsRepository.AddString(PlayButtonString, "gioca");
                localizedStringsRepository.AddString(ScoreButtonString, "punteggi");
                localizedStringsRepository.AddString(FartButtonString, "scoreggia");

                localizedStringsRepository.AddString(ScorePageTitleString, "Punteggi");
                localizedStringsRepository.AddString(BestNumberOfMetersScoreKey, "Maggior numero di metri percorsi: ");
                localizedStringsRepository.AddString(BestVegetablesEatenScoreKey, "Maggior numero di verdure mangiate: ");
                localizedStringsRepository.AddString(BestFartsScoreKey, "Maggior numero di scoregge: ");

                localizedStringsRepository.AddString(MetriTimeString, " metri");
                localizedStringsRepository.AddString(NumberOfMetersString, "Numero di metri: ");
                localizedStringsRepository.AddString(NumberOfVegetablesEaten, "Numero di verdure mangiate: ");
                localizedStringsRepository.AddString(NumbersOfFartsString, "Numero di scoregge flatulate: ");

                localizedStringsRepository.AddString(BeanPowerupText, "SEDERE IN TEMPESTA!");
                localizedStringsRepository.AddString(BroccoloPowerupText, "CAVALCA LA CACCA!");
            }
            else
            {
                localizedStringsRepository.AddString(PlayButtonString, "play");
                localizedStringsRepository.AddString(ScoreButtonString, "score");
                localizedStringsRepository.AddString(FartButtonString, "fart");

                localizedStringsRepository.AddString(ScorePageTitleString, "Score");
                localizedStringsRepository.AddString(BestNumberOfMetersScoreKey, "Highest number of meters: ");
                localizedStringsRepository.AddString(BestVegetablesEatenScoreKey, "Highest number of vegetables eaten: ");
                localizedStringsRepository.AddString(BestFartsScoreKey, "Highest number of farts: ");

                localizedStringsRepository.AddString(MetriTimeString, " meters");
                localizedStringsRepository.AddString(NumberOfMetersString, "Number of meters: ");
                localizedStringsRepository.AddString(NumberOfVegetablesEaten, "Number of vegetables eaten: ");
                localizedStringsRepository.AddString(NumbersOfFartsString, "Number of farts farted: ");

                localizedStringsRepository.AddString(BeanPowerupText, "ASS STORM!");
                localizedStringsRepository.AddString(BroccoloPowerupText, "RIDING ON A POO!");
            }
        }
    }
}