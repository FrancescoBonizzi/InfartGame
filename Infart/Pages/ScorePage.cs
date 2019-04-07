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
    public class ScorePage
    {
        private readonly Sprite _background;
        private readonly IScreenTransformationMatrixProvider _matrixScaleProvider;
        private readonly SpriteFont _font;

        private readonly ScalingObject _titleScalingObject;
        private readonly DrawingInfos _titleDrawingInfos;
        private readonly string _titleText;

        private readonly List<ScoreRecordText> _scoreInfos;
        private readonly int _nTexts;

        private readonly FadeObject _fadeObject;
        private int _currentTextId;

        public ScorePage(
            AssetsLoader assets,
            ISettingsRepository settingsRepository,
            IScreenTransformationMatrixProvider matrixScaleProvider,
            ILocalizedStringsRepository localizedStringsRepository)
        {
            _font = assets.Font;
            _background = assets.OtherSprites["scoreBackground"];
            _matrixScaleProvider = matrixScaleProvider;

            _titleText = localizedStringsRepository.Get(GameStringsLoader.ScorePageTitleString);
            _titleScalingObject = new ScalingObject(1f, 1.2f, 1.0f);
            _titleDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(matrixScaleProvider.VirtualWidth / 2f, 100f),
                Origin = _font.GetTextCenter(_titleText)
            };

            float textsScale = 0.4f;

            var bestFarts = settingsRepository.GetOrSetInt(GameScores.BestFartsScoreKey, default(int));
            var bestAliveTime = settingsRepository.GetOrSetTimeSpan(GameScores.BestNumberOfMetersScoreKey, default(TimeSpan));
            var bestVegetablesEaten = settingsRepository.GetOrSetInt(GameScores.BestVegetablesEatenScoreKey, default(int));

            _scoreInfos = new List<ScoreRecordText>()
            {
                new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.BestAliveTimeString)}{bestAliveTime.ToMinuteSecondsFormat()}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_titleDrawingInfos.Position.X / 2, _titleDrawingInfos.Position.Y + 100f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    }),

               new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.BestVegetablesEatenScoreKey)}{bestVegetablesEaten}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_titleDrawingInfos.Position.X / 2, _titleDrawingInfos.Position.Y + 140f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    }),

                new ScoreRecordText(
                    $"{localizedStringsRepository.Get(GameStringsLoader.BestFartsScoreKey)}{bestFarts}",
                    new DrawingInfos()
                    {
                        Position = new Vector2(_titleDrawingInfos.Position.X / 2, _titleDrawingInfos.Position.Y + 180f),
                        OverlayColor = Color.White.WithAlpha(0),
                        Scale = textsScale
                    }),
            };

            _nTexts = _scoreInfos.Count;
            _currentTextId = 0;
            _fadeObject = new FadeObject(TimeSpan.FromMilliseconds(500), Color.White);
            _fadeObject.FadeInCompleted += _textFadeObject_FadeInCompleted;
            _fadeObject.FadeIn();
        }

        private void _textFadeObject_FadeInCompleted(object sender, EventArgs e)
        {
            _fadeObject.FadeIn();
            ++_currentTextId;
        }

        public void Update(TimeSpan elapsed)
        {
            _titleScalingObject.Update(elapsed);
            _titleDrawingInfos.Scale = _titleScalingObject.Scale;

            if (_currentTextId < _nTexts)
            {
                _scoreInfos[_currentTextId].TextDrawingInfos.OverlayColor = _fadeObject.OverlayColor;
                _fadeObject.Update(elapsed);
            }

            foreach (var text in _scoreInfos)
            {
                text.Update(elapsed);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _matrixScaleProvider.ScaleMatrix);

            spriteBatch.Draw(_background);
            spriteBatch.DrawString(_font, _titleText, _titleDrawingInfos);
            foreach (var score in _scoreInfos)
            {
                score.Draw(spriteBatch, _font);
            }

            spriteBatch.End();
        }
    }
}
