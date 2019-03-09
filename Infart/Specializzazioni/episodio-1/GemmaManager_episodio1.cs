
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace fge
{
    public class GemmaManager_episodio1 : GemmaManager
    {
        private Gemma jalapenos_ = null;
        private Gemma broccolo_ = null;

        enum PowerUps { Jalapeno, Merda };
        PowerUps next_power_up_ = PowerUps.Jalapeno;

        public GemmaManager_episodio1(
            Camera CameraReference,
            Loader_episodio1 Loader)
            : base(CameraReference, Loader.textures_, Loader.textures_rectangles_["Burger"])
        {
            jalapenos_ = new Gemma(Loader.textures_, Loader.textures_rectangles_["Jalapenos"]);
            broccolo_ = new Gemma(Loader.textures_, Loader.textures_rectangles_["Verdura"]);
        }

        public override void Reset(Camera CameraReference)
        {
            base.Reset(CameraReference);
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

        
        public bool CheckJalapenoCollisionWithPlayer(Player p)
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

        //// Bool perché qui deve partire il merda explosion
        public bool CheckMerdaCollisionWithPlayer(Player p)
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

        
        
        public override void Update(double gametime)
        {
            base.Update(gametime);

            jalapenos_.Update(gametime);
            broccolo_.Update(gametime);
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

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




