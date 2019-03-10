using Infart.Assets;
using Infart.Astronaut;
using Infart.Drawing;
using Infart.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Infart
{
    public class GemmaManager
    {
        private readonly Gemma _jalapenos = null;

        private readonly Gemma _broccolo = null;

        protected const int MaxGemmeAttive = 10;

        protected List<Gemma> GemmeAttive;

        protected List<Gemma> GemmeInactive;

        protected Random Random;

        protected Camera CurrentCamera;

        private enum PowerUps
        { Jalapeno, Merda };

        private PowerUps _nextPowerUp = PowerUps.Jalapeno;

        public GemmaManager(
            Camera cameraReference,
            AssetsLoader assetsLoader)
        {
            Random = FbonizziHelper.Random;
            CurrentCamera = cameraReference;
            GemmeAttive = new List<Gemma>();
            GemmeInactive = new List<Gemma>();

            for (int i = 0; i < MaxGemmeAttive; ++i)
                GemmeInactive.Add(new Gemma(
                    assetsLoader.Textures,
                    assetsLoader.TexturesRectangles["Burger"]));

            _jalapenos = new Gemma(assetsLoader.Textures, assetsLoader.TexturesRectangles["Jalapenos"]);
            _broccolo = new Gemma(assetsLoader.Textures, assetsLoader.TexturesRectangles["Verdura"]);
        }

        public void Reset(Camera cameraReference)
        {
            for (int i = 0; i < GemmeAttive.Count; ++i)
            {
                GemmeInactive.Add(GemmeAttive[i]);
                GemmeAttive.RemoveAt(i);
                --i;
            }

            CurrentCamera = cameraReference;
        }

        public void AddPowerUp(Vector2 position)
        {
            switch (_nextPowerUp)
            {
                case PowerUps.Jalapeno:
                    AddJalapenos(position);

                    break;

                case PowerUps.Merda:
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
                _nextPowerUp = PowerUps.Merda;
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

        public bool CheckMerdaCollisionWithPlayer(Player p)
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
            if (GemmeInactive.Count > 0)
            {
                GemmeInactive[0].Position = startingPosition;
                GemmeInactive[0].Active = true;
                GemmeAttive.Add(GemmeInactive[0]);
                GemmeInactive.RemoveAt(0);
            }
        }

        private void RemoveGemma(int index)
        {
            GemmeAttive[index].Active = false;
            GemmeInactive.Add(GemmeAttive[index]);
            GemmeAttive.RemoveAt(index);
        }

        protected bool ToBeRemoved(Vector2 position, float width)
        {
            if (position.X + width < CurrentCamera.Position.X)
                return true;
            return false;
        }

        public bool CheckCollisionWithPlayer(Player p)
        {
            for (int i = 0; i < GemmeAttive.Count; ++i)
            {
                if (GemmeAttive[i].CollisionRectangle.Intersects(p.CollisionRectangle))
                {
                    RemoveGemma(i);
                    --i;
                    return true;
                }
            }
            return false;
        }

        public void Update(double gametime)
        {
            for (int i = 0; i < GemmeAttive.Count; ++i)
                GemmeAttive[i].Update(gametime);

            _jalapenos.Update(gametime);
            _broccolo.Update(gametime);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < GemmeAttive.Count; ++i)
            {
                Gemma g = GemmeAttive[i];

                if (ToBeRemoved(g.Position, g.Width))
                {
                    RemoveGemma(i);
                    --i;
                }
                else
                    g.Draw(spritebatch);
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