using Microsoft.Xna.Framework;
using System;

namespace Infart.Drawing
{
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;

        public int ViewPortWidth;

        public int ViewPortHeight;

        private float _zoom = 1.0f;

        private Matrix _transform;

        private float _rotation = 0.0f;

        private readonly float _movingAmount = 150.0f;

        private Vector2 _velocity = Vector2.Zero;

        private Vector2 _positionToGoTo;

        private readonly float _intornoUguaglianza = 10.0f;

        private bool _moving = false;

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
            _moving = false;
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

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public void MoveTo(Vector2 where)
        {
            _positionToGoTo = where;

            int xDir;
            if (Position.X < where.X)
                xDir = +1;
            else if (Position.X > where.X)
                xDir = -1;
            else xDir = 0;

            int yDir;
            if (Position.Y < where.Y)
                yDir = +1;
            else if (Position.Y > where.Y)
                yDir = -1;
            else yDir = 0;

            _velocity = new Vector2(
                _movingAmount * xDir,
                _movingAmount * yDir);

            _moving = true;
        }

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
                    Matrix.CreateRotationZ(_rotation) *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
            return _transform;
        }

        public void Update(double gametime)
        {
            if (_moving)
            {
                if (_positionToGoTo != Position)
                {
                    float elapsed = (float)gametime / 1000.0f;
                    Position += _velocity * elapsed;

                    if (Math.Abs(Position.X - _positionToGoTo.X) < _intornoUguaglianza)
                    {
                        Position.X = _positionToGoTo.X;
                        _velocity.X = 0.0f;
                    }

                    if (Math.Abs(Position.Y - _positionToGoTo.Y) < _intornoUguaglianza)
                    {
                        Position.Y = _positionToGoTo.Y;
                        _velocity.Y = 0.0f;
                    }
                }
                else
                    _moving = false;
            }
        }
    }
}