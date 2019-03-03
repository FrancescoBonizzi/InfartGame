#region Using

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


#endregion

namespace fge
{
    public /*abstract*/ class StatusBar
    {
        #region Dichiarazioni

        private Vector2 position_;

        private StatusBarSprite[] status_burgers_;
        private Texture2D texture_reference_;

        private SoundManager_episodio1 sound_manager_reference_;

        private Color empty_color = Color.Gray * 0.5f;
        private Color full_color = Color.White;

        private double elapsed_ = 0.0;

        private int jump_count_ = 0;
        private const int jump_needed_to_remove_ham_ = 1;

        private int hamburgers_ = 0;
        private int hamburger_mangiati_totale_ = 0;
        private const int soglia_hamburger_per_star_male_ = 4;
        private float overlay_death_opacity_ = 0.01f;

        private Rectangle sfondo_morte_rect_;

        private bool infart_ = false;

        #endregion

        #region Costruttore / Distruttore

        public StatusBar(
            Vector2 position,
            Loader_episodio1 Loader,
            SoundManager_episodio1 SoundManagerReference)
        {
            position_ = position;
            texture_reference_ = Loader.textures_;

            sound_manager_reference_ = SoundManagerReference;

            status_burgers_ = new StatusBarSprite[4];
            Vector2 tmp_pos = new Vector2(position.X, position.Y + 20);
            int ham_width = Loader.textures_rectangles_["Burger"].Width;
            Vector2 ham_scale = new Vector2(0.9f);
            sfondo_morte_rect_ = Loader.textures_rectangles_["death_screen"];

            for (int i = 0; i < 4; ++i)
            {
                status_burgers_[i] = new StatusBarSprite(
                    Loader.textures_,
                    Loader.textures_rectangles_["Burger"],
                    tmp_pos,
                    empty_color,
                    ham_scale);
                tmp_pos.X += ham_width;
                // La distanza tra loro è già ottenuta dalla diminuzione della scala
            }
        }

        public void Reset()
        {
            infart_ = false;
            Hamburgers = 0;

            for (int i = 0; i < 4; ++i)
            {
                status_burgers_[i].Reset();
                status_burgers_[i].FillColor = empty_color;
            }
            overlay_death_opacity_ = 0.01f;
            hamburger_mangiati_totale_ = 0;
            jump_count_ = 0;
        }

        #endregion

        #region Proprietà

        public int CurrentHamburgers
        {
            get { return hamburgers_; }
        }

        public int HamburgerMangiatiInTotale
        {
            get { return hamburger_mangiati_totale_; }
        }

        public bool IsInfarting()
        {
            if (hamburgers_ > soglia_hamburger_per_star_male_ || infart_)
            {
                infart_ = true;

                return true;
            }

            return false;
        }

        #endregion

        #region Metodi

        public void HamburgerEaten()
        {
            Hamburgers += 1;
            status_burgers_[hamburgers_ - 1].Taken();
            status_burgers_[hamburgers_ - 1].FillColor = full_color;
            jump_count_ = 0;

            if (hamburgers_ >= soglia_hamburger_per_star_male_)
            {
                status_burgers_[hamburgers_ - 1].FillColor = Color.LightCoral;

                // TODO
                // Il modo migliore per rendere soundmanager universale è inserire 
                // ogni canzone in un dizionario... così che si possa fare sound_manager.Play("Scoreggia");
                sound_manager_reference_.PlayHeartBeat();
            }

            ++hamburger_mangiati_totale_;
        }

        public void PlayerJumped()
        {
            ++jump_count_;

            if (jump_count_ == jump_needed_to_remove_ham_)
            {
                --Hamburgers;
                status_burgers_[hamburgers_].Lost();
                status_burgers_[hamburgers_].FillColor = empty_color;
                jump_count_ = 0;
            }

            if (hamburgers_ < soglia_hamburger_per_star_male_)
            {
                sound_manager_reference_.StopHeartBeat();
            }
        }

        public void SetInfart()
        {
            infart_ = true;
        }

        private int Hamburgers
        {
            get { return hamburgers_; }
            set
            {
                int amount = value - hamburgers_;

                if (value < 0) hamburgers_ = 0;
                else if (value > 4) infart_ = true;
                else hamburgers_ = value;
            }
        }

        public void ComputeJalapenos()
        {
            for (int i = 0; i < hamburgers_; ++i)
            {
                status_burgers_[i].Lost();
                status_burgers_[i].FillColor = empty_color;
            }

            Hamburgers = 0;
            sound_manager_reference_.StopHeartBeat();
        }

        public void ComputeMerda()
        {
            for (int i = 0; i < hamburgers_; ++i)
            {
                status_burgers_[i].Lost();
                status_burgers_[i].FillColor = empty_color;
            }

            Hamburgers = 0;
            sound_manager_reference_.StopHeartBeat();
        }

        #endregion

        #region Update/Draw

        public void Update(double gametime)
        {
            elapsed_ += gametime;

            if (!infart_)
            {
                for (int i = 0; i < 4; ++i)
                    status_burgers_[i].Update(gametime);

                if (hamburgers_ == soglia_hamburger_per_star_male_)
                {
                    if (overlay_death_opacity_ < 1.0f)
                        overlay_death_opacity_ += 0.01f;
                }
                else
                {
                    if (overlay_death_opacity_ >= 0.01)
                        overlay_death_opacity_ -= 0.01f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 4; ++i)
                status_burgers_[i].Draw(spriteBatch);

            if (overlay_death_opacity_ > 0.01f)
            {
                spriteBatch.Draw(
                    texture_reference_,
                    Vector2.Zero,
                    sfondo_morte_rect_,
                    Color.White * overlay_death_opacity_);
            }
        }

        #endregion
    }
}
