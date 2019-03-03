#region Using

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace fge
{
    public class GroundManager_episodio1 : GroundManager
    {
        #region Dichiarazioni

        private GrattacieliAutogeneranti grattacieli_camminabili_ = null;
        private float min_time_to_next_buco_ = 2000.0f;
        private float elapsed_ = 0.0f;

        private InfartGame game_manager_reference_;

        #endregion

        #region Costruttore / Distruttore

        public GroundManager_episodio1(
            Camera CurrentCamera,
            Loader_episodio1 Loader,
            InfartGame GameManagerReference)
            : base(CurrentCamera)
        {
            grattacieli_camminabili_ = new GrattacieliAutogeneranti_episodio1(
                Loader.textures_gratta_ground_,
                Loader.textures_rectangles_,
                "ground",
                69,
                CurrentCamera,
                GameManagerReference);

            // TODO Verificare che sia davvero reference e non per valore
            game_manager_reference_ = GameManagerReference;
        }

        #endregion

        #region Proprietà

        public override List<GameObject> WalkableObjects()
        {
            return grattacieli_camminabili_.DrawnObjectsList;
        }

        /* TODO Serviva a qualcosa?
        public List<Grattacielo> GeneratedObjects
        {
            get { return grattacieli_camminabili_.CachedObjectList; }
        }
         */

        #endregion

        #region Metodi

        public override void Reset(Camera camera)
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

        #endregion

        #region Update/Draw

        public override void Update(double gametime)
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

        public override void Draw(SpriteBatch spritebatch)
        {
            grattacieli_camminabili_.Draw(spritebatch);
        }



        #endregion
    }
}
