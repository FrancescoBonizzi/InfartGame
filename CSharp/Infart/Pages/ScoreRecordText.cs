using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.TransformationObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Infart.Pages
{
    public class ScoreRecordText
    {
        public string ScoreText { get; }
        public DrawingInfos TextDrawingInfos { get; }

        public string RecordText { get; }
        public DrawingInfos RecordTextDrawingInfos { get; }
        private Color _recordColor = new Color(255, 234, 0);
        private readonly ScalingObject _recordScalingObject;

        public ScoreRecordText(
            string scoreText,
            DrawingInfos textDrawingInfos,
            string recordText = null)
        {
            ScoreText = scoreText;
            TextDrawingInfos = textDrawingInfos;
            RecordText = recordText;
            int positionToAdd = recordText == null ? 0 : 100;
            textDrawingInfos.Position = new Vector2(
                textDrawingInfos.Position.X + positionToAdd,
                textDrawingInfos.Position.Y);
            RecordTextDrawingInfos = new DrawingInfos()
            {
                Position = textDrawingInfos.Position - new Vector2(150f, 0f),
                Scale = textDrawingInfos.Scale,
                OverlayColor = _recordColor
            };

            _recordScalingObject = new ScalingObject(textDrawingInfos.Scale - 0.01f, textDrawingInfos.Scale + 0.01f);
        }

        public void Update(TimeSpan elapsed)
        {
            _recordScalingObject.Update(elapsed);
            RecordTextDrawingInfos.Scale = _recordScalingObject.Scale;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (RecordText != null)
            {
                spriteBatch.DrawString(
                    font,
                    RecordText,
                    RecordTextDrawingInfos);
            }

            spriteBatch.DrawString(
                font,
                ScoreText,
                TextDrawingInfos);
        }
    }
}

