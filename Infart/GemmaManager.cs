using Infart.Assets;
using Infart.Astronaut;
using Infart.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Infart
{
    public class GemmaManager
    {
        private readonly Gemma _jalapenos;
        private readonly Gemma _broccolo;

        private const int MaxGemmeAttive = 10;
        private readonly List<Gemma> _gemmeAttive;
        private readonly List<Gemma> _gemmeInactive;
        private Camera _currentCamera;

        private enum PowerUps
        {
            Jalapeno,
            Broccolo
        };

        private PowerUps _nextPowerUp;

        public GemmaManager(
            Camera cameraReference,
            AssetsLoader assetsLoader)
        {
            _currentCamera = cameraReference;
            _gemmeAttive = new List<Gemma>();
            _gemmeInactive = new List<Gemma>();

            for (int i = 0; i < MaxGemmeAttive; ++i)
                _gemmeInactive.Add(new Gemma(
                    assetsLoader.Textures,
                    assetsLoader.TexturesRectangles["Burger"]));

            _jalapenos = new Gemma(assetsLoader.Textures, assetsLoader.TexturesRectangles["Jalapenos"]);
            _broccolo = new Gemma(assetsLoader.Textures, assetsLoader.TexturesRectangles["Verdura"]);
        }

        public void Reset(Camera cameraReference)
        {
            for (int i = 0; i < _gemmeAttive.Count; ++i)
            {
                _gemmeInactive.Add(_gemmeAttive[i]);
                _gemmeAttive.RemoveAt(i);
                --i;
            }

            _currentCamera = cameraReference;
        }

        public void AddPowerUp(Vector2 position)
        {
            switch (_nextPowerUp)
            {
                case PowerUps.Jalapeno:
                    AddJalapenos(position);
                    break;

                case PowerUps.Broccolo:
                    AddMerda(position);
                    break;
            }
        }

        private void AddJalapenos(Vector2 position)
        {
            if (!_jalapenos.Active && !_broccolo.Active)
            {
                _jalapenos.Position = position;
                _jalapenos.Active = true;
                _nextPowerUp = PowerUps.Broccolo;
            }
        }

        private void AddMerda(Vector2 position)
        {
            if (!_jalapenos.Active && !_broccolo.Active)
            {
                _broccolo.Position = position;
                _broccolo.Active = true;
                _nextPowerUp = PowerUps.Jalapeno;
            }
        }

        public bool CheckJalapenoCollisionWithPlayer(Player p)
        {
            if (_jalapenos.Active)
            {
                if (_jalapenos.CollisionRectangle.Intersects(p.CollisionRectangle))
                {
                    _jalapenos.Active = false;
                    return true;
                }
            }
            return false;
        }

        public bool CheckBroccoloCollisionWithPlayer(Player p)
        {
            if (_broccolo.Active)
            {
                if (_broccolo.CollisionRectangle.Intersects(p.CollisionRectangle))
                {
                    _broccolo.Active = false;
                    return true;
                }
            }
            return false;
        }

        public void AddGemma(Vector2 startingPosition)
        {
            if (_gemmeInactive.Count > 0)
            {
                _gemmeInactive[0].Position = startingPosition;
                _gemmeInactive[0].Active = true;
                _gemmeAttive.Add(_gemmeInactive[0]);
                _gemmeInactive.RemoveAt(0);
            }
        }

        private void RemoveGemma(int index)
        {
            _gemmeAttive[index].Active = false;
            _gemmeInactive.Add(_gemmeAttive[index]);
            _gemmeAttive.RemoveAt(index);
        }

        private bool ToBeRemoved(Vector2 position, float width)
        {
            return position.X + width < _currentCamera.Position.X;
        }

        public bool CheckCollisionWithPlayer(Player p)
        {
            for (int i = 0; i < _gemmeAttive.Count; ++i)
            {
                if (_gemmeAttive[i].CollisionRectangle.Intersects(p.CollisionRectangle))
                {
                    RemoveGemma(i);
                    return true;
                }
            }
            return false;
        }

        public void Update(double gametime)
        {
            for (int i = 0; i < _gemmeAttive.Count; ++i)
                _gemmeAttive[i].Update(gametime);

            _jalapenos.Update(gametime);
            _broccolo.Update(gametime);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < _gemmeAttive.Count; ++i)
            {
                Gemma g = _gemmeAttive[i];

                if (ToBeRemoved(g.Position, g.Width))
                {
                    RemoveGemma(i);
                    --i;
                }
                else
                {
                    g.Draw(spritebatch);
                }
            }

            if (_jalapenos.Active)
            {
                if (ToBeRemoved(_jalapenos.Position, _jalapenos.Width))
                    _jalapenos.Active = false;
                else
                    _jalapenos.Draw(spritebatch);
            }

            if (_broccolo.Active)
            {
                if (ToBeRemoved(_broccolo.Position, _broccolo.Width))
                    _broccolo.Active = false;
                else
                    _broccolo.Draw(spritebatch);
            }
        }
    }
}