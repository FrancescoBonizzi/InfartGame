#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace fge
{
    public class InfartExplosion
    {
        #region Dichiarazioni

        private double elapsed_ = 0.0;
        private const double time_to_finish_ = 5000.0;
        private const float scritta_scale_increase_amount_ = 0.004f;

        private const int particelle_number_ = 100;
        private List<ParticleExplosion> particelle_;
        private ParticleExplosion scritta_ = null;

        private List<Color> fart_colors_ = new List<Color> { 
            new Color(111, 86, 41),
            new Color(65, 44, 32),
            Color.LawnGreen
        };

        private Vector2 emitter_location_ = Vector2.Zero;

        private static Random random_;
        private bool finished_ = false;
        private bool started_ = false;
        private bool active_ = false;
        private bool with_text_;

        #endregion

        #region Costruttore / Distruttore

        public InfartExplosion(
            Texture2D TextureScrittaEParticelle,
            Rectangle ScrittaRectangle,
            Rectangle ParticleRectangle,
            Rectangle ParticleRectangle2)
        {
            random_ = fbonizziHelper.random;
            particelle_ = new List<ParticleExplosion>();

            AddNewScritta(TextureScrittaEParticelle, ScrittaRectangle);
            AddRandomParticles(TextureScrittaEParticelle, ParticleRectangle, ParticleRectangle2);
        }

        #endregion

        #region Proprietà

        public bool Finished
        {
            get { return finished_; }
        }

        public bool Started
        {
            get { return started_; }
        }

        #endregion

        #region Metodi

        // Attenzione!

        public void Explode(Vector2 CenterPosition, bool WithText, SoundManager_episodio1 SoundManager)
        {
            started_ = true;
            with_text_ = WithText;

            active_ = true;
            SoundManager.PlayExplosion();
            SetEmitterLocation(CenterPosition);
        }

        public void Reset()
        {
            finished_ = false;
            started_ = false;
            elapsed_ = 0.0;
            active_ = false;

            ResetParticles();
            ResetScritta();
        }

        private void SetEmitterLocation(Vector2 EmitterLocation)
        {
            scritta_.position_ = EmitterLocation;

            for (int i = 0; i < particelle_number_; ++i)
                particelle_[i].position_ = EmitterLocation;
        }

        private void ResetParticles()
        {
            for (int i = 0; i < particelle_number_; ++i)
            {
                float angle = random_.Next(0, 360);
                angle = MathHelper.ToRadians(-angle);
                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle))
                    * random_.Next(80, 140);

                particelle_[i].Refactor(
                        velocity,
                        angle,
                        (float)(random_.NextDouble() * 5),
                        fart_colors_[random_.Next(fart_colors_.Count)],
                        (float)random_.NextDouble() - 0.3f,
                        500);
            }
        }

        private void ResetScritta()
        {
            scritta_.Refactor(
               Vector2.Zero,
               (float)random_.NextDouble(),
               (float)random_.NextDouble() * 10,
               Color.White,
               1.0f,
               500);
        }

        private void AddRandomParticles(
            Texture2D Texture,
            Rectangle ParticleRectangle1,
            Rectangle ParticleRectangle2)
        {
            for (int i = 0; i < particelle_number_; ++i)
            {
                float angle = random_.Next(0, 360);
                angle = MathHelper.ToRadians(-angle);
                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle),
                    (float)Math.Sin(angle))
                    * random_.Next(80, 140);

                particelle_.Add(
                    new ParticleExplosion(
                       Texture,
                        random_.NextDouble() < 0.5 ? ParticleRectangle1 : ParticleRectangle2,
                        emitter_location_,
                        velocity,
                        angle,
                        (float)(random_.NextDouble() * 5),
                        fart_colors_[random_.Next(fart_colors_.Count)],
                        (float)random_.NextDouble() - 0.3f,
                        500));
            }
        }



        public void AddNewScritta(
            Texture2D TextureScritta,
            Rectangle TextureRectangle)
        {
            scritta_ = new ParticleExplosion(
                TextureScritta,
                TextureRectangle,
                emitter_location_,
                Vector2.Zero,
                (float)random_.NextDouble(),
                (float)random_.NextDouble() * 10,
                Color.White,
                1.0f,
                500);
        }


        #endregion

        #region Update / Draw

        public void Update(double gameTime)
        {
            if (active_)
            {
                elapsed_ += gameTime;

                if (elapsed_ >= time_to_finish_)
                    finished_ = true;

                for (int i = 0; i < particelle_.Count; ++i)
                    particelle_[i].Update(gameTime);

                if (with_text_)
                {
                    scritta_.Scale += scritta_scale_increase_amount_ * ((float)elapsed_ / 1000.0f);
                    scritta_.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active_)
            {
                for (int i = 0; i < particelle_.Count; ++i)
                    particelle_[i].Draw(spriteBatch);

                if (with_text_)
                    scritta_.Draw(spriteBatch);
            }
        }

        #endregion
    }
}
