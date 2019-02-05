#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


#endregion

namespace fge
{
    public class Nuvolificio
    {
        #region Dichiarazioni

        private List<Nuvola> nuvole_;

        private int nuvole_number_ = 5;
        private Vector2 scale_;
        private Color overlay_color_;
        private Vector2 speed_range_;

        private Vector2 nuvole_spawn_y_range_;

        private bool active_ = true;

        #region Tools

        private static Random random_;
        private float elapsed_ = 0.0f;
        private Camera current_camera_;

        private float camera_pos_y_;
        private float camera_pos_x_;
        private float camera_w_;
        private float camera_h_;

        #endregion

        #endregion

        #region Costruttore

        public Nuvolificio(
            Color OverlayColor,
            float Scale,
            Vector2 SpeedRange,
            Vector2 NuvoleSpawnYrange,
            Camera CurrentCamera,
            List<Rectangle> NuvolaRectangles,
            Texture2D NuvolaTexture)
        {
            random_ = fbonizziHelper.random;

            current_camera_ = CurrentCamera;
            overlay_color_ = OverlayColor;
            scale_ = new Vector2(Scale, Scale);

            if (NuvoleSpawnYrange.X > NuvoleSpawnYrange.Y)
            {
                float tmp = NuvoleSpawnYrange.X;
                NuvoleSpawnYrange.X = NuvoleSpawnYrange.Y;
                NuvoleSpawnYrange.Y = tmp;
            }
            nuvole_spawn_y_range_ = NuvoleSpawnYrange;

            speed_range_ = SpeedRange;

            camera_w_ = current_camera_.ViewPortWidth;
            camera_h_ = current_camera_.ViewPortHeight;

            Initialize(NuvolaTexture, NuvolaRectangles);
        }

        private void Initialize(Texture2D NuvolaTexture, List<Rectangle> NuvolaRectangles)
        {
            List<Rectangle> nuvole_rects_ = NuvolaRectangles;
            //  nuvole_rects_.Add(Loader.textures_rectangles_["nuvola1"]);
            // nuvole_rects_.Add(Loader.textures_rectangles_["nuvola2"]);
            // nuvole_rects_.Add(Loader.textures_rectangles_["nuvola3"]);

            nuvole_ = new List<Nuvola>();
            for (int i = 0; i < nuvole_number_; ++i)
                nuvole_.Add(new Nuvola(
                    NuvolaTexture,
                    nuvole_rects_[random_.Next(nuvole_rects_.Count)]));
        }

        #endregion

        #region Proprietà

        public Vector2 NuvoleYSpawnRange
        {
            get { return nuvole_spawn_y_range_; }
            set
            {

                if (value.X > value.Y)
                {
                    float tmp = value.X;
                    value.X = value.Y;
                    value.Y = tmp;
                }
                nuvole_spawn_y_range_ = value;
            }
        }

        #endregion

        #region Metodi

        public void Reset(Camera camera)
        {
            current_camera_ = camera;
            elapsed_ = 0.0f;
        }

        public void MoveX(float amount)
        {
            for (int i = 0; i < nuvole_.Count; ++i)
            {
                nuvole_[i].PositionX += amount;
            }
        }

        private void SetNuvola(int index)
        {
            float y_pos = random_.Next(
                (int)nuvole_spawn_y_range_.X,
                (int)nuvole_spawn_y_range_.Y);

            float x_pos;
            int direction;
            if (random_.NextDouble() > 0.5)
            {
                x_pos = (int)current_camera_.Position.X - 200 + random_.Next(1, 20);
                direction = +1;
            }
            else
            {
                x_pos = (int)current_camera_.Position.X + (int)current_camera_.ViewPortWidth + 200 - random_.Next(1, 20);
                direction = -1;
            }

            Vector2 rand_pos = new Vector2(x_pos, y_pos);

            float rand_speed = random_.Next((int)speed_range_.X, (int)speed_range_.Y);

            nuvole_[index].Set(
                    rand_pos,
                    rand_speed * direction,
                    overlay_color_,
                    scale_);
        }

        private bool ToBeRemoved(Nuvola n)
        {
            if (!n.Active)
                return true;

            Vector2 position = n.Position;
            if (position.X > camera_pos_x_ + camera_w_ + 250
                || position.X < camera_pos_x_ - 250)
                return true;

            return false;
        }

        #endregion

        #region Update/Draw

        public void Update(double gametime)
        {
            camera_pos_y_ = current_camera_.Position.Y;

            if (camera_pos_y_ + camera_h_ >= nuvole_spawn_y_range_.X
                && camera_pos_y_ <= nuvole_spawn_y_range_.Y)
            {
                active_ = true;
            }
            else
            {
                active_ = false;
                return;
            }

            if (active_)
            {
                elapsed_ += (float)gametime / 1000.0f;

                camera_pos_x_ = current_camera_.Position.X;

                for (int i = 0; i < nuvole_.Count; ++i)
                {

                    if (ToBeRemoved(nuvole_[i]))
                        SetNuvola(i);
                    else
                        nuvole_[i].Update(gametime);
                }
            }

        }

        public void Draw(SpriteBatch spritebatch)
        {
            if (active_)
            {
                for (int i = 0; i < nuvole_.Count; ++i)
                    nuvole_[i].Draw(spritebatch);
            }
        }

        #endregion
    }
}
