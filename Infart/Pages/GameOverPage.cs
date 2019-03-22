using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.StringsLocalization.Abstractions;
using FbonizziMonoGame.TransformationObjects;
using Infart.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Infart.Pages
{
    public class GameOverPage
    {
        private readonly SpriteFont _font;

        private const string _gameOverText = "Game Over";
        private readonly DrawingInfos _gameOverTextDrawingInfos;
        private readonly ScalingObject _gameOverScalingObject;

        private List<ScoreRecordText> _scoreInfos;
        private readonly int _nTexts;

        private FadeObject _fadeObject;
        private int _currentTextId;

        private readonly Sprite _background;
        private readonly IScreenTransformationMatrixProvider _matrixScaleProvider;

        public GameOverPage(
            IScreenTransformationMatrixProvider matrixScaleProvider,
            AssetsLoader assets,
            ISettingsRepository settingsRepository,
            int thisGameNumberOfHamburgersEaten,
            int thisGameNumberOfMeters,
            int thisGameNumberOfFarts,
            ILocalizedStringsRepository localizedStringsRepository)
        {
            _font = assets.Font;
            _background = assets.OtherSprites["gameoverBackground"];
            _matrixScaleProvider = matrixScaleProvider;

            _gameOverTextDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2() { X = matrixScaleProvider.VirtualWidth / 2f, Y = 100f },
                Origin = _font.GetTextCenter(_gameOverText)
            };
            _gameOverScalingObject = new ScalingObject(1f, 1.2f, 1.0f);

            var bestFarts = settingsRepository.GetOrSetInt(GameScores.BestHamburgersEatenScoreKey, default(int));
            var bestNumberOfMeters = settingsRepository.GetOrSetInt(GameScores.BestNumberOfMetersScoreKey, default(int));
            var bestHamburgersEaten = settingsRepository.GetOrSetInt(GameScores.BestFartsScoreKey, default(int));

            var bestNumberOfHamburgersEatenRecord = false;
            if (thisGameNumberOfHamburgersEaten < bestHamburgersEaten)
            {
                settingsRepository.SetInt(GameScores.BestHamburgersEatenScoreKey, thisGameNumberOfHamburgersEaten);
                bestNumberOfHamburgersEatenRecord = true;
            }

            var bestAliveTimeRecord = false;
            if (thisGameNumberOfMeters > bestNumberOfMeters)
            {
                settingsRepository.SetInt(GameScores.BestNumberOfMetersScoreKey, thisGameNumberOfMeters);
                bestAliveTimeRecord = true;
            }

            var bestNumberOfFartsRecord = false;
            if (thisGameNumberOfFarts > bestFarts)
            {
                settingsRepository.SetInt(GameScores.BestFartsScoreKey, thisGameNumberOfFarts);
                bestNumberOfFartsRecord = true;
            }

            const float textsScale = 0.4f;

            _scoreInfos = new List<ScoreRecordText>()
            {
                 new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.NumberOfMetersString)}: {thisGameNumberOfMeters}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_gameOverTextDrawingInfos.Position.X / 2, _gameOverTextDrawingInfos.Position.Y + 200f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    },
                    !bestAliveTimeRecord ? null : "Record!"),

                 new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.NumbersOfFartsString)}: {thisGameNumberOfFarts}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_gameOverTextDrawingInfos.Position.X / 2, _gameOverTextDrawingInfos.Position.Y + 125f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    },
                    !bestNumberOfFartsRecord ? null : "Record!"),

                new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.NumberOfHamburgersEaten)}: {thisGameNumberOfHamburgersEaten}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_gameOverTextDrawingInfos.Position.X / 2, _gameOverTextDrawingInfos.Position.Y + 162f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    },
                    !bestNumberOfHamburgersEatenRecord ? null : "Record!"),
            };

            _nTexts = _scoreInfos.Count;
            _currentTextId = 0;
            _fadeObject = new FadeObject(TimeSpan.FromMilliseconds(200), Color.White);
            _fadeObject.FadeInCompleted += _textFadeObject_FadeInCompleted;
            _fadeObject.FadeIn();
        }

        private void _textFadeObject_FadeInCompleted(object sender, EventArgs e)
        {
            _fadeObject.FadeIn();
            ++_currentTextId;
        }

        public void HandleInput(GameOrchestrator orchestrator)
        {
            if (_currentTextId < _scoreInfos.Count - 1)
                return;

            orchestrator.SetMenuState();
        }

        public void Update(TimeSpan elapsed)
        {
            _gameOverScalingObject.Update(elapsed);
            _gameOverTextDrawingInfos.Scale = _gameOverScalingObject.Scale;

            if (_currentTextId < _nTexts)
            {
                _scoreInfos[_currentTextId].TextDrawingInfos.OverlayColor = _fadeObject.OverlayColor;
                _fadeObject.Update(elapsed);
            }

            foreach (var text in _scoreInfos)
                text.Update(elapsed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _matrixScaleProvider.ScaleMatrix);

            spriteBatch.Draw(_background);
            spriteBatch.DrawString(_font, _gameOverText, _gameOverTextDrawingInfos);
            foreach (var score in _scoreInfos)
                score.Draw(spriteBatch, _font);

            spriteBatch.End();
        }
    }
}
