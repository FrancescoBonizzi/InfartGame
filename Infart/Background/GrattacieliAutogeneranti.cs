using Infart.Drawing;
using Infart.Extensions;
using Infart.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infart.Background
{
    public class GrattacieliAutogeneranti
    {
        protected List<Grattacielo> grattacieli_to_draw_;

        protected List<Grattacielo> grattacieli_in_coda_;

        protected Grattacielo firstOne_pointer_ = null;

        protected Texture2D texture_reference_;

        protected bool has_finestre_;

        protected bool background_;

        protected bool innest_gemma_;

        protected Vector2 next_avaible_position_;

        protected const int max_grattacielo_position_offset_ = 20;

        protected int last_grattacielo_height_ = 4;

        protected const int num_grattacieli_to_draw_ = 16;

        protected static Random random_;

        protected Camera current_camera_;

        protected int camera_position_x_;

        protected int camera_w_;

        protected float scale_finestra_;

        protected int[] GrattasW_;

        protected int resolution_h_;

        private InfartGame game_manager_reference_;

        public GrattacieliAutogeneranti(
            Texture2D Texture,
            IDictionary<string, Rectangle> GrattaRects,
            string EntryName,
            int GrattaNumber,
            Camera PlayerCamera,
            InfartGame GameManagerReference)
        {
            game_manager_reference_ = GameManagerReference;

            current_camera_ = PlayerCamera;
            random_ = fbonizziHelper.random;
            camera_w_ = current_camera_.ViewPortWidth;

            texture_reference_ = Texture;

            resolution_h_ = PlayerCamera.ViewPortHeight;

            next_avaible_position_ = new Vector2(0.0f, resolution_h_);

            grattacieli_in_coda_ = new List<Grattacielo>();
            grattacieli_to_draw_ = new List<Grattacielo>();

            LoadGrattacieli(EntryName, GrattaRects, GrattaNumber);

            if (EntryName == "ground")
                innest_gemma_ = true;
            else innest_gemma_ = false;
        }

        public int LastGrattacieloHeight
        {
            get { return last_grattacielo_height_; }
            set { last_grattacielo_height_ = value; }
        }

        public Vector2 NextGrattacieloPosition
        {
            get { return next_avaible_position_; }
            set { next_avaible_position_ = value; }
        }

        public List<GameObject> DrawnObjectsList
        {
            get { return grattacieli_to_draw_.Cast<GameObject>().ToList(); }
        }

        public List<Grattacielo> CachedObjectList
        {
            get { return grattacieli_in_coda_; }
        }

        public void MoveX(float Xamount)
        {
            for (int i = 0; i < grattacieli_to_draw_.Count; ++i)
                grattacieli_to_draw_[i].PositionX += Xamount;
        }

        public void Reset(Camera camera)
        {
            if (firstOne_pointer_ != null)
            {
                current_camera_ = camera;

                for (int i = 0; i < grattacieli_to_draw_.Count; ++i)
                {
                    grattacieli_in_coda_.Add(grattacieli_to_draw_[i]);
                    grattacieli_to_draw_.RemoveAt(i);
                    --i;
                }

                int index;
                for (index = 0; index < grattacieli_in_coda_.Count; ++index)
                {
                    if (grattacieli_in_coda_[index] == firstOne_pointer_)
                    {
                        break;
                    }
                }

                while (grattacieli_in_coda_[0] != firstOne_pointer_)
                {
                    grattacieli_in_coda_.Add(grattacieli_in_coda_[0]);
                    grattacieli_in_coda_.RemoveAt(0);
                }

                next_avaible_position_ = new Vector2(0.0f, resolution_h_);

                AddGrattacieloForDrawingInit();
            }
        }

        private void LoadGrattacieli(string EntryName, IDictionary<string, Rectangle> GrattaRects, int GrattaNumber)
        {
            for (int i = 1; i <= GrattaNumber; ++i)
            {
                Grattacielo tmp_obj =
                    new Grattacielo(
                        GrattaRects[EntryName + i],
                        texture_reference_);
                grattacieli_in_coda_.Add(tmp_obj);

                if (firstOne_pointer_ == null)
                    firstOne_pointer_ = tmp_obj;
            }
        }

        private void AddGrattacieloForDrawingInit()
        {
            for (int i = 0; i < 16; ++i)
            {
                grattacieli_in_coda_[0].Position = next_avaible_position_;

                next_avaible_position_.X +=
                     grattacieli_in_coda_[0].Width +
                     random_.Next(1, max_grattacielo_position_offset_);

                grattacieli_to_draw_.Add(grattacieli_in_coda_[0]);
                grattacieli_in_coda_.RemoveAt(0);
            }
        }

        private bool ToBeRemoved(float camera_position_x, int obj_position_x, int Width)
        {
            return camera_position_x > obj_position_x + Width;
        }

        public void Update(double gametime, Camera current_camera)
        {
            camera_position_x_ = (int)current_camera_.Position.X;

            if (grattacieli_to_draw_.Count < num_grattacieli_to_draw_)
                AddGrattacieloForDrawing();

            for (int i = 0; i < grattacieli_to_draw_.Count(); ++i)
            {
                Grattacielo g = grattacieli_to_draw_[i];

                g.Update(gametime);

                if (ToBeRemoved(camera_position_x_, (int)g.Position.X, (int)g.Width))
                {
                    grattacieli_in_coda_.Add(g);
                    grattacieli_to_draw_.RemoveAt(i);
                    --i;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < grattacieli_to_draw_.Count; ++i)
                grattacieli_to_draw_[i].Draw(spriteBatch);
        }

        private void AddGrattacieloForDrawing()
        {
            float camera_x_first = camera_position_x_;
            float camera_x_last = camera_x_first + camera_w_;

            while (grattacieli_to_draw_.Count < num_grattacieli_to_draw_
                && grattacieli_in_coda_.Count > 0

                && next_avaible_position_.X <= camera_x_last)
            {
                grattacieli_in_coda_[0].Position = next_avaible_position_;

                next_avaible_position_.X +=
                     grattacieli_in_coda_[0].Width +
                     random_.Next(1, max_grattacielo_position_offset_);

                if (innest_gemma_)
                {
                    if (random_.NextDouble() < game_manager_reference_.GemmaProbability)
                    {
                        game_manager_reference_.AddGemma(
                            (new Vector2(
                                  grattacieli_in_coda_[0].Position.X + 15,
                                  resolution_h_ - grattacieli_in_coda_[0].Height - 70)));
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

                grattacieli_to_draw_.Add(grattacieli_in_coda_[0]);
                grattacieli_in_coda_.RemoveAt(0);
            }
        }
    }
}