using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.TransformationObjects;
using System;

namespace Infart.Drawing
{
    public class PopupText
    {
        public string Text { get; set; }
        public DrawingInfos DrawingInfos { get; set; }
        public PopupObject PopupObject { get; set; }
        private readonly ScalingObject _scalingObject;

        public PopupText()
        {
            _scalingObject = new ScalingObject(1f, 1.8f, 2f);
        }

        public void Update(TimeSpan elapsed)
        {
            _scalingObject.Update(elapsed);
            PopupObject.Update(elapsed);
            DrawingInfos.Position = PopupObject.Position;
            DrawingInfos.OverlayColor = PopupObject.OverlayColor;
            DrawingInfos.Scale = _scalingObject.Scale;
        }
    }
}
