using Microsoft.Xna.Framework;

namespace Infart.Drawing
{
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;
        public int ViewPortWidth;
        public int ViewPortHeight;
        private float _zoom = 1f;
        private Matrix _transform;
        private Vector2 _velocity = Vector2.Zero;

        public Camera(Vector2 startingPosition, Vector2 viewPortSize, float zoom)
        {
            Position = startingPosition;

            _zoom = zoom;

            ViewPortWidth = 1000;
            ViewPortHeight = 600;
        }

        public void Reset(Vector2 position)
        {
            Position = position;
        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f) _zoom = 0.1f;
            }
        }

        public float Rotation { get; set; } = 0.0f;

        public Vector2 ScreenToWorld(Vector2 pos)
        {
            return Vector2.Transform(pos, Matrix.Invert(_transform));
        }

        public Vector2 WorldToScreen(Vector2 pos)
        {
            return Vector2.Transform(pos, _transform);
        }

        public Matrix GetTransformation()
        {
            _transform =
              Matrix.CreateTranslation(
                    new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
            return _transform;
        }

    }
}