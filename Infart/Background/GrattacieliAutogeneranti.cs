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
        protected List<Grattacielo> GrattacieliToDraw;

        protected List<Grattacielo> GrattacieliInCoda;

        protected Grattacielo FirstOnePointer = null;

        protected Texture2D TextureReference;

        protected bool HasFinestre;

        protected bool Background;

        protected bool InnestGemma;

        protected Vector2 NextAvaiblePosition;

        protected const int MaxGrattacieloPositionOffset = 20;

        protected int LastGrattacieloHeight = 4;

        protected const int NumGrattacieliToDraw = 16;

        protected static Random Random;

        protected Camera CurrentCamera;

        protected int CameraPositionX;

        protected int CameraW;

        protected float ScaleFinestra;

        protected int[] GrattasW;

        protected int ResolutionH;

        private readonly InfartGame _gameManagerReference;

        public GrattacieliAutogeneranti(
            Texture2D texture,
            IDictionary<string, Rectangle> grattaRects,
            string entryName,
            int grattaNumber,
            Camera playerCamera,
            InfartGame gameManagerReference)
        {
            _gameManagerReference = gameManagerReference;

            CurrentCamera = playerCamera;
            Random = FbonizziHelper.Random;
            CameraW = CurrentCamera.ViewPortWidth;

            TextureReference = texture;

            ResolutionH = playerCamera.ViewPortHeight;

            NextAvaiblePosition = new Vector2(0.0f, ResolutionH);

            GrattacieliInCoda = new List<Grattacielo>();
            GrattacieliToDraw = new List<Grattacielo>();

            LoadGrattacieli(entryName, grattaRects, grattaNumber);

            if (entryName == "ground")
                InnestGemma = true;
            else InnestGemma = false;
        }
        
        public Vector2 NextGrattacieloPosition
        {
            get { return NextAvaiblePosition; }
            set { NextAvaiblePosition = value; }
        }

        public List<GameObject> DrawnObjectsList
        {
            get { return GrattacieliToDraw.Cast<GameObject>().ToList(); }
        }

        public List<Grattacielo> CachedObjectList
        {
            get { return GrattacieliInCoda; }
        }

        public void MoveX(float xamount)
        {
            for (int i = 0; i < GrattacieliToDraw.Count; ++i)
                GrattacieliToDraw[i].PositionX += xamount;
        }

        public void Reset(Camera camera)
        {
            if (FirstOnePointer != null)
            {
                CurrentCamera = camera;

                for (int i = 0; i < GrattacieliToDraw.Count; ++i)
                {
                    GrattacieliInCoda.Add(GrattacieliToDraw[i]);
                    GrattacieliToDraw.RemoveAt(i);
                    --i;
                }

                int index;
                for (index = 0; index < GrattacieliInCoda.Count; ++index)
                {
                    if (GrattacieliInCoda[index] == FirstOnePointer)
                    {
                        break;
                    }
                }

                while (GrattacieliInCoda[0] != FirstOnePointer)
                {
                    GrattacieliInCoda.Add(GrattacieliInCoda[0]);
                    GrattacieliInCoda.RemoveAt(0);
                }

                NextAvaiblePosition = new Vector2(0.0f, ResolutionH);

                AddGrattacieloForDrawingInit();
            }
        }

        private void LoadGrattacieli(string entryName, IDictionary<string, Rectangle> grattaRects, int grattaNumber)
        {
            for (int i = 1; i <= grattaNumber; ++i)
            {
                Grattacielo tmpObj =
                    new Grattacielo(
                        grattaRects[entryName + i],
                        TextureReference);
                GrattacieliInCoda.Add(tmpObj);

                if (FirstOnePointer == null)
                    FirstOnePointer = tmpObj;
            }
        }

        private void AddGrattacieloForDrawingInit()
        {
            for (int i = 0; i < 16; ++i)
            {
                GrattacieliInCoda[0].Position = NextAvaiblePosition;

                NextAvaiblePosition.X +=
                     GrattacieliInCoda[0].Width +
                     Random.Next(1, MaxGrattacieloPositionOffset);

                GrattacieliToDraw.Add(GrattacieliInCoda[0]);
                GrattacieliInCoda.RemoveAt(0);
            }
        }

        private bool ToBeRemoved(float cameraPositionX, int objPositionX, int width)
        {
            return cameraPositionX > objPositionX + width;
        }

        public void Update(double gametime, Camera currentCamera)
        {
            CameraPositionX = (int)CurrentCamera.Position.X;

            if (GrattacieliToDraw.Count < NumGrattacieliToDraw)
                AddGrattacieloForDrawing();

            for (int i = 0; i < GrattacieliToDraw.Count(); ++i)
            {
                Grattacielo g = GrattacieliToDraw[i];

                g.Update(gametime);

                if (ToBeRemoved(CameraPositionX, (int)g.Position.X, (int)g.Width))
                {
                    GrattacieliInCoda.Add(g);
                    GrattacieliToDraw.RemoveAt(i);
                    --i;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < GrattacieliToDraw.Count; ++i)
                GrattacieliToDraw[i].Draw(spriteBatch);
        }

        private void AddGrattacieloForDrawing()
        {
            float cameraXFirst = CameraPositionX;
            float cameraXLast = cameraXFirst + CameraW;

            while (GrattacieliToDraw.Count < NumGrattacieliToDraw
                && GrattacieliInCoda.Count > 0

                && NextAvaiblePosition.X <= cameraXLast)
            {
                GrattacieliInCoda[0].Position = NextAvaiblePosition;

                NextAvaiblePosition.X +=
                     GrattacieliInCoda[0].Width +
                     Random.Next(1, MaxGrattacieloPositionOffset);

                if (InnestGemma)
                {
                    if (Random.NextDouble() < _gameManagerReference.GemmaProbability)
                    {
                        _gameManagerReference.AddGemma(
                            (new Vector2(
                                  GrattacieliInCoda[0].Position.X + 15,
                                  ResolutionH - GrattacieliInCoda[0].Height - 70)));
                    }
                    else
                        if (Random.NextDouble() < _gameManagerReference.PowerUpProbability)
                    {
                        if (!_gameManagerReference.JalapenosModeActive && !_gameManagerReference.MerdaModeActive)
                        {
                            _gameManagerReference.AddPowerUp(new Vector2(
                                    GrattacieliInCoda[0].PositionAtTopLeftCorner().X,
                                    GrattacieliInCoda[0].PositionAtTopLeftCorner().Y - 180));
                        }
                    }
                }

                GrattacieliToDraw.Add(GrattacieliInCoda[0]);
                GrattacieliInCoda.RemoveAt(0);
            }
        }
    }
}