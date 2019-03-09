
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace fge
{
    public class GemmaManager_episodio1
    {
        private Gemma jalapenos_ = null;
        private Gemma broccolo_ = null;



        protected const int max_gemme_attive_ = 10;
        protected List<Gemma> gemme_attive_;
        protected List<Gemma> gemme_inactive_;

        protected Random random_;
        protected Camera current_camera_;


        enum PowerUps { Jalapeno, Merda };
        PowerUps next_power_up_ = PowerUps.Jalapeno;

        public GemmaManager_episodio1(
            Camera CameraReference,
            Loader_episodio1 Loader)
        {
            random_ = fbonizziHelper.random;
            current_camera_ = CameraReference;
            gemme_attive_ = new List<Gemma>();
            gemme_inactive_ = new List<Gemma>();

            for (int i = 0; i < max_gemme_attive_; ++i)
                gemme_inactive_.Add(new Gemma(
                    Loader.textures_,
                    Loader.textures_rectangles_["Burger"]));

            jalapenos_ = new Gemma(Loader.textures_, Loader.textures_rectangles_["Jalapenos"]);
            broccolo_ = new Gemma(Loader.textures_, Loader.textures_rectangles_["Verdura"]);
        }

        public void Reset(Camera CameraReference)
        {
            for (int i = 0; i < gemme_attive_.Count; ++i)
            {
                gemme_inactive_.Add(gemme_attive_[i]);
                gemme_attive_.RemoveAt(i);
                --i;
            }

            current_camera_ = CameraReference;
        }

        public void AddPowerUp(Vector2 Position)
        {
            switch (next_power_up_)
            {
                case PowerUps.Jalapeno:
                    AddJalapenos(Position);

                    break;

                case PowerUps.Merda:
                    AddMerda(Position);

                    break;
            }
        }

        private void AddJalapenos(Vector2 Position)
        {
            if (!jalapenos_.Active && !broccolo_.Active)
            {
                jalapenos_.Position = Position;
                jalapenos_.Active = true;
                next_power_up_ = PowerUps.Merda;
            }
        }

        private void AddMerda(Vector2 Position)
        {
            if (!jalapenos_.Active && !broccolo_.Active)
            {
                broccolo_.Position = Position;
                broccolo_.Active = true;
                next_power_up_ = PowerUps.Jalapeno;
            }
        }

        
        public bool CheckJalapenoCollisionWithPlayer(Player_episodio1 p)
        {
            if (jalapenos_.Active)
            {
                if (jalapenos_.CollisionRectangle.Intersects(p.CollisionRectangle))
                {
                    jalapenos_.Active = false;
                    return true;
                }
            }
            return false;
        }

        
        public bool CheckMerdaCollisionWithPlayer(Player_episodio1 p)
        {
            if (broccolo_.Active)
            {
                if (broccolo_.CollisionRectangle.Intersects(p.CollisionRectangle))
                {
                    broccolo_.Active = false;
                    return true;
                }
            }
            return false;
        }



        public void AddGemma(Vector2 StartingPosition)
        {
            if (gemme_inactive_.Count > 0)
            {
                gemme_inactive_[0].Position = StartingPosition;
                gemme_inactive_[0].Active = true;
                gemme_attive_.Add(gemme_inactive_[0]);
                gemme_inactive_.RemoveAt(0);
            }
        }



        private void RemoveGemma(int index)
        {
            gemme_attive_[index].Active = false;
            gemme_inactive_.Add(gemme_attive_[index]);
            gemme_attive_.RemoveAt(index);
        }


        protected bool ToBeRemoved(Vector2 position, float width)
        {
            if (position.X + width < current_camera_.Position.X)
                return true;
            return false;
        }


        public bool CheckCollisionWithPlayer(Player_episodio1 p)
        {
            for (int i = 0; i < gemme_attive_.Count; ++i)
            {
                if (gemme_attive_[i].CollisionRectangle.Intersects(p.CollisionRectangle))
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
            for (int i = 0; i < gemme_attive_.Count; ++i)
                gemme_attive_[i].Update(gametime);

            jalapenos_.Update(gametime);
            broccolo_.Update(gametime);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < gemme_attive_.Count; ++i)
            {
                Gemma g = gemme_attive_[i];

                if (ToBeRemoved(g.Position, g.Width))
                {
                    RemoveGemma(i);
                    --i;
                }
                else
                    g.Draw(spritebatch);
            }

            if (jalapenos_.Active)
            {
                if (ToBeRemoved(jalapenos_.Position, jalapenos_.Width))
                    jalapenos_.Active = false;
                else
                    jalapenos_.Draw(spritebatch);
            }

            if (broccolo_.Active)
            {
                if (ToBeRemoved(broccolo_.Position, broccolo_.Width))
                    broccolo_.Active = false;
                else
                    broccolo_.Draw(spritebatch);
            }
        }

        
    }
}




