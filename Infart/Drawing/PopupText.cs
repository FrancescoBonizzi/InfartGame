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

        public void Update(TimeSpan elapsed)
        {
            PopupObject.Update(elapsed);
            DrawingInfos.Position = PopupObject.Position;
            DrawingInfos.OverlayColor = PopupObject.OverlayColor;
        }
    }
}
