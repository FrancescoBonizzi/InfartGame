#region Using

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace fge
{
    public class GrattacieliAutogeneranti_episodio1 : GrattacieliAutogeneranti
    {
        #region Dichiarazioni

        private InfartGame game_manager_reference_;

       
        #endregion

        #region Costruttore / Distruttore

        public GrattacieliAutogeneranti_episodio1(
            Texture2D Texture,
            Dictionary<string, Rectangle> GrattaRects,
            string EntryName,
            int GrattaNumber,
            Camera PlayerCamera,
            InfartGame GameManagerReference)
            : base(
            Texture,
            GrattaRects,
            EntryName,
            GrattaNumber,
            PlayerCamera)
        {
            game_manager_reference_ = GameManagerReference;
        }

        #endregion

    
        protected override void AddGrattacieloForDrawing()
        {
            // La posizione l'aggiorno qua per evitare il sovrapporsi dei grattacieli
            float camera_x_first = camera_position_x_;
            float camera_x_last = camera_x_first + camera_w_;

            while (grattacieli_to_draw_.Count < num_grattacieli_to_draw_
                && grattacieli_in_coda_.Count > 0
                //   && next_avaible_position_.X >= camera_x_first
                && next_avaible_position_.X <= camera_x_last)
            {
                grattacieli_in_coda_[0].Position = next_avaible_position_;

                next_avaible_position_.X +=
                     grattacieli_in_coda_[0].Width +
                     random_.Next(1, max_grattacielo_position_offset_);

                #region Innesto gemme / Powerups

                if (innest_gemma_)
                {
                    if (random_.NextDouble() < game_manager_reference_.GemmaProbability)
                    {
                        game_manager_reference_.AddGemma(
                            (new Vector2(
                                  grattacieli_in_coda_[0].Position.X + 15,
                                  resolution_h_ - grattacieli_in_coda_[0].Height - 70))); // Perchè ho messo in basso l'origine
                    }
                    else
                        if (random_.NextDouble() < game_manager_reference_.PowerUpProbability)
                        {
                            if (!game_manager_reference_.jalapenos_mode_active_ && !game_manager_reference_.merda_mode_active_)
                            {
                                game_manager_reference_.AddPowerUp(new Vector2(
                                        grattacieli_in_coda_[0].PositionAtTopLeftCorner().X,
                                        grattacieli_in_coda_[0].PositionAtTopLeftCorner().Y - 180));
                            }
                        }

                }
                #endregion

                grattacieli_to_draw_.Add(grattacieli_in_coda_[0]);
                grattacieli_in_coda_.RemoveAt(0);
            }
        }

    }
}
