using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.StringsLocalization.Abstractions;
using FbonizziMonoGame.TransformationObjects;
using FbonizziMonoGame.UI.RateMe;
using Infart.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Infart.Pages
{
    public class MainMenuPage
    {
        private readonly IScreenTransformationMatrixProvider _matrixScaleProvider;

        private readonly SpriteFont _font;
        private readonly RateMeDialog _rateMeDialog;
        private readonly SoundManager _soundManager;

        private readonly Sprite _background;

        private readonly ScalingObject _titleScalingObject;
        private readonly DrawingInfos _titleDrawingInfos;
        private readonly Sprite _titleImage;

        private readonly ScalingObject _playScalingObject;
        private readonly DrawingInfos _playDrawingInfos;
        private readonly string _playText;
        private readonly Vector2 _playTextSize;

        private readonly ScalingObject _fartScalingObject;
        private readonly DrawingInfos _fartDrawingInfos;
        private readonly string _fartText;
        private readonly Vector2 _fartTextSize;

        private readonly ScalingObject _scoreScalingObject;
        private readonly DrawingInfos _scoreDrawingInfos;
        private readonly string _scoreText;
        private readonly Vector2 _scoreTextSize;

        private readonly ScalingObject _aboutScalingObject;
        private readonly DrawingInfos _aboutDrawingInfos;
        private readonly string _achievementText;
        private readonly Vector2 _aboutTextSize;

        public MainMenuPage(
            AssetsLoader assets,
            RateMeDialog rateMeDialog,
            SoundManager soundManager,
            ISettingsRepository settingsRepository,
            IScreenTransformationMatrixProvider matrixScaleProvider,
            ILocalizedStringsRepository localizedStringsRepository)
        {
            _font = assets.Font;
            _matrixScaleProvider = matrixScaleProvider;
            _rateMeDialog = rateMeDialog ?? throw new ArgumentNullException(nameof(rateMeDialog));
            _soundManager = soundManager ?? throw new ArgumentNullException(nameof(soundManager));

            _background = assets.OtherSprites["menuBackground"];
            _playText = localizedStringsRepository.Get(GameStringsLoader.PlayButtonString);
            _scoreText = localizedStringsRepository.Get(GameStringsLoader.ScoreButtonString);
            _fartText = localizedStringsRepository.Get(GameStringsLoader.FartButtonString);
            _titleImage = assets.OtherSprites["gameTitle"];
            _achievementText = "about";

            _titleScalingObject = new ScalingObject(1f, 1.2f, 1.0f);
            _titleDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(matrixScaleProvider.VirtualWidth / 2f, 100f),
                Origin = _titleImage.SpriteCenter
            };

            _playScalingObject = new ScalingObject(0.5f, 0.7f, 1f);
            _playDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(140f, 250f),
                Origin = _font.GetTextCenter(_playText)
            };
            _playTextSize = _font.MeasureString(_playText);

            _fartScalingObject = new ScalingObject(0.5f, 0.7f, 1f);
            _fartDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(140f, 320f),
                Origin = _font.GetTextCenter(_fartText),
                OverlayColor = new Color(155, 88, 48)
            };
            _fartTextSize = _font.MeasureString(_fartText);

            _scoreTextSize = _font.MeasureString(_scoreText);
            _scoreScalingObject = new ScalingObject(0.5f, 0.7f, 1f);
            _scoreDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(matrixScaleProvider.VirtualWidth - 150f, 250f),
                Origin = _font.GetTextCenter(_scoreText)
            };

            _aboutTextSize = _font.MeasureString(_achievementText);
            _aboutScalingObject = new ScalingObject(0.5f, 0.7f, 1f);
            _aboutDrawingInfos = new DrawingInfos()
            {
                Position = new Vector2(matrixScaleProvider.VirtualWidth - 150f, 320f),
                Origin = _font.GetTextCenter(_achievementText)
            };

            _soundManager.PlayMenuBackground();
        }

        public void HandleInput(
            Vector2 touchPoint,
            GameOrchestrator orchestrator)
        {
            if (_rateMeDialog.ShouldShowDialog)
            {
                _rateMeDialog.HandleInput(touchPoint);
            }
            else
            {
                if (_playDrawingInfos.HitBox((int)_playTextSize.X, (int)_playTextSize.Y)
                    .Contains(touchPoint))
                {
                    orchestrator.SetGameState();
                }
                else if (_fartDrawingInfos.HitBox((int)_fartTextSize.X, (int)_fartTextSize.Y)
                    .Contains(touchPoint))
                {
                    _soundManager.PlayFart();
                }
                else if (_scoreDrawingInfos.HitBox((int)_scoreTextSize.X, (int)_scoreTextSize.Y)
                    .Contains(touchPoint))
                {
                    orchestrator.SetScoreState();
                }
                else if (_aboutDrawingInfos.HitBox((int)_aboutTextSize.X, (int)_aboutTextSize.Y)
                    .Contains(touchPoint))
                {
                    orchestrator.SetAboutState();
                }
            }
        }

        public void Update(TimeSpan elapsed)
        {
            _titleScalingObject.Update(elapsed);
            _titleDrawingInfos.Scale = _titleScalingObject.Scale;

            _playScalingObject.Update(elapsed);
            _playDrawingInfos.Scale = _playScalingObject.Scale;

            _fartScalingObject.Update(elapsed);
            _fartDrawingInfos.Scale = _fartScalingObject.Scale;

            _scoreScalingObject.Update(elapsed);
            _scoreDrawingInfos.Scale = _scoreScalingObject.Scale;

            _aboutScalingObject.Update(elapsed);
            _aboutDrawingInfos.Scale = _aboutScalingObject.Scale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _matrixScaleProvider.ScaleMatrix);

            spriteBatch.Draw(_background);

            spriteBatch.Draw(_titleImage, _titleDrawingInfos);
            spriteBatch.DrawString(_font, _playText, _playDrawingInfos);
            spriteBatch.DrawString(_font, _fartText, _fartDrawingInfos);
            spriteBatch.DrawString(_font, _scoreText, _scoreDrawingInfos);
            spriteBatch.DrawString(_font, _achievementText, _aboutDrawingInfos);

            if (_rateMeDialog.ShouldShowDialog)
                _rateMeDialog.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
