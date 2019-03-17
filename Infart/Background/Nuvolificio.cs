using Infart.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Infart.Background
{
    public class Nuvolificio
    {
        private List<Nuvola> _nuvole;
        private readonly int _nuvoleNumber = 5;
        private readonly Vector2 _scale;
        private readonly Color _overlayColor;
        private readonly Vector2 _speedRange;
        private Vector2 _nuvoleSpawnYRange;
        private bool _active = true;
        private float _elapsed = 0.0f;
        private Camera _currentCamera;
        private float _cameraPosY;
        private float _cameraPosX;
        private readonly float _cameraW;
        private readonly float _cameraH;

        public Nuvolificio(
            Color overlayColor,
            float scale,
            Vector2 speedRange,
            Vector2 nuvoleSpawnYrange,
            Camera currentCamera,
            List<Rectangle> nuvolaRectangles,
            Texture2D nuvolaTexture)
        {
            _currentCamera = currentCamera;
            _overlayColor = overlayColor;
            _scale = new Vector2(scale, scale);

            if (nuvoleSpawnYrange.X > nuvoleSpawnYrange.Y)
            {
                float tmp = nuvoleSpawnYrange.X;
                nuvoleSpawnYrange.X = nuvoleSpawnYrange.Y;
                nuvoleSpawnYrange.Y = tmp;
            }
            _nuvoleSpawnYRange = nuvoleSpawnYrange;

            _speedRange = speedRange;

            _cameraW = _currentCamera.ViewPortWidth;
            _cameraH = _currentCamera.ViewPortHeight;

            Initialize(nuvolaTexture, nuvolaRectangles);
        }

        private void Initialize(Texture2D nuvolaTexture, List<Rectangle> nuvolaRectangles)
        {
            List<Rectangle> nuvoleRects = nuvolaRectangles;

            _nuvole = new List<Nuvola>();
            for (int i = 0; i < _nuvoleNumber; ++i)
                _nuvole.Add(new Nuvola(
                    nuvolaTexture,
                    nuvoleRects[FbonizziMonoGame.Numbers.RandomBetween(0, nuvoleRects.Count)]));
        }

        public Vector2 NuvoleYSpawnRange
        {
            get { return _nuvoleSpawnYRange; }
            set
            {
                if (value.X > value.Y)
                {
                    float tmp = value.X;
                    value.X = value.Y;
                    value.Y = tmp;
                }
                _nuvoleSpawnYRange = value;
            }
        }

        public void Reset(Camera camera)
        {
            _currentCamera = camera;
            _elapsed = 0.0f;
        }

        public void MoveX(float amount)
        {
            for (int i = 0; i < _nuvole.Count; ++i)
            {
                _nuvole[i].PositionX += amount;
            }
        }

        private void SetNuvola(int index)
        {
            float yPos = FbonizziMonoGame.Numbers.RandomBetween(
                (int)_nuvoleSpawnYRange.X,
                (int)_nuvoleSpawnYRange.Y);

            float xPos;
            int direction;
            if (FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) > 0.5)
            {
                xPos = (int)_currentCamera.Position.X - 200 + FbonizziMonoGame.Numbers.RandomBetween(1, 20);
                direction = +1;
            }
            else
            {
                xPos = (int)_currentCamera.Position.X + (int)_currentCamera.ViewPortWidth + 200 - FbonizziMonoGame.Numbers.RandomBetween(1, 20);
                direction = -1;
            }

            Vector2 randPos = new Vector2(xPos, yPos);

            float randSpeed = FbonizziMonoGame.Numbers.RandomBetween((int)_speedRange.X, (int)_speedRange.Y);

            _nuvole[index].Set(
                    randPos,
                    randSpeed * direction,
                    _overlayColor,
                    _scale);
        }

        private bool ToBeRemoved(Nuvola n)
        {
            if (!n.Active)
                return true;

            Vector2 position = n.Position;
            if (position.X > _cameraPosX + _cameraW + 250
                || position.X < _cameraPosX - 250)
                return true;

            return false;
        }

        public void Update(double gametime)
        {
            _cameraPosY = _currentCamera.Position.Y;

            if (_cameraPosY + _cameraH >= _nuvoleSpawnYRange.X
                && _cameraPosY <= _nuvoleSpawnYRange.Y)
            {
                _active = true;
            }
            else
            {
                _active = false;
                return;
            }

            if (_active)
            {
                _elapsed += (float)gametime / 1000.0f;

                _cameraPosX = _currentCamera.Position.X;

                for (int i = 0; i < _nuvole.Count; ++i)
                {
                    if (ToBeRemoved(_nuvole[i]))
                        SetNuvola(i);
                    else
                        _nuvole[i].Update(gametime);
                }
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (_active)
            {
                for (int i = 0; i < _nuvole.Count; ++i)
                    _nuvole[i].Draw(spritebatch);
            }
        }
    }
}