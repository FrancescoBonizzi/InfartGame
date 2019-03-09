using Infart.Assets;
using Infart.Drawing;
using Infart.Extensions;
using Infart.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Infart.Background
{
    public class GroundManager
    {
        protected Random random_;

        protected Camera current_camera_;

        private GrattacieliAutogeneranti grattacieli_camminabili_ = null;

        private float min_time_to_next_buco_ = 2000.0f;

        private float elapsed_ = 0.0f;

        private InfartGame game_manager_reference_;

        public GroundManager(
            Camera CurrentCamera,
            AssetsLoader AssetsLoader,
            InfartGame GameManagerReference)
        {
            current_camera_ = CurrentCamera;
            random_ = fbonizziHelper.random;

            grattacieli_camminabili_ = new GrattacieliAutogeneranti(
                AssetsLoader.TexturesGrattaGround,
                AssetsLoader.TexturesRectangles,
                "ground",
                69,
                CurrentCamera,
                GameManagerReference);

            game_manager_reference_ = GameManagerReference;
        }

        public List<GameObject> WalkableObjects()
        {
            return grattacieli_camminabili_.DrawnObjectsList;
        }

        public void Reset(Camera camera)
        {
            current_camera_ = camera;
            grattacieli_camminabili_.Reset(camera);
            elapsed_ = 0.0f;
        }

        private void GenerateBuco()
        {
            int first_x = (int)grattacieli_camminabili_.NextGrattacieloPosition.X;
            int space = random_.Next((int)game_manager_reference_.LarghezzaBuchi.X, (int)game_manager_reference_.LarghezzaBuchi.Y);

            grattacieli_camminabili_.NextGrattacieloPosition = new Vector2(
                first_x + space,
                (int)grattacieli_camminabili_.NextGrattacieloPosition.Y);
        }

        public void Update(double gametime)
        {
            elapsed_ += (float)gametime;
            if (elapsed_ >= min_time_to_next_buco_)
            {
                if (random_.NextDouble() < game_manager_reference_.BucoProbability)
                {
                    GenerateBuco();
                    elapsed_ = 0.0f;
                }
            }

            grattacieli_camminabili_.Update(gametime, current_camera_);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            grattacieli_camminabili_.Draw(spritebatch);
        }
    }
}