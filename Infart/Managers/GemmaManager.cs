
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace fge
{
    public abstract class GemmaManager
    {
        
        
        protected const int max_gemme_attive_ = 10;
        protected List<Gemma> gemme_attive_;
        protected List<Gemma> gemme_inactive_;

        protected Random random_;
        protected Camera current_camera_;

        

        
        public GemmaManager(
            Camera CameraReference,
            Texture2D Texture,
            Rectangle GemmaRectangle)
        {
            random_ = fbonizziHelper.random;
            current_camera_ = CameraReference;
            gemme_attive_ = new List<Gemma>();
            gemme_inactive_ = new List<Gemma>();

            for (int i = 0; i < max_gemme_attive_; ++i)
                gemme_inactive_.Add(new Gemma(
                    Texture,
                    GemmaRectangle));
        }

        
        public virtual void Reset(Camera CameraReference)
        {
            for (int i = 0; i < gemme_attive_.Count; ++i)
            {
                gemme_inactive_.Add(gemme_attive_[i]);
                gemme_attive_.RemoveAt(i);
                --i;
            }

            current_camera_ = CameraReference;
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

        protected bool ToBeRemoved(Vector2 position, float width)
        {
            if (position.X + width < current_camera_.Position.X)
                return true;
            return false;
        }

        private void RemoveGemma(int index)
        {
            gemme_attive_[index].Active = false;
            gemme_inactive_.Add(gemme_attive_[index]);
            gemme_attive_.RemoveAt(index);
        }

        public bool CheckCollisionWithPlayer(Player p)
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

        

        
        public virtual void Update(double gametime)
        {
            for (int i = 0; i < gemme_attive_.Count; ++i)
                gemme_attive_[i].Update(gametime);
        }

        public virtual void Draw(SpriteBatch spritebatch)
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
        }

        
    }
}




