using Infart.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Infart.Background
{
    public class GrattacieliAutogeneranti
    {
        private readonly List<Grattacielo> GrattacieliToDraw;
        private Grattacielo FirstOnePointer = null;
        private readonly Texture2D _textureReference;
        private readonly bool _innestGemma;
        private const int MaxGrattacieloPositionOffset = 20;
        private const int NumGrattacieliToDraw = 16;
        private Camera CurrentCamera;
        private int CameraPositionX;
        private Vector2 _nextGrattacieloPosition;
        private readonly int _cameraW;
        private readonly int _resolutionH;

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
            _cameraW = CurrentCamera.ViewPortWidth;

            _textureReference = texture;

            _resolutionH = playerCamera.ViewPortHeight;

            NextGrattacieloPosition = new Vector2(0.0f, _resolutionH);

            CachedObjectList = new List<Grattacielo>();
            GrattacieliToDraw = new List<Grattacielo>();

            LoadGrattacieli(entryName, grattaRects, grattaNumber);

            if (entryName == "ground")
            {
                _innestGemma = true;
            }
            else
            {
                _innestGemma = false;
            }
        }

        public Vector2 NextGrattacieloPosition { get => _nextGrattacieloPosition; set => _nextGrattacieloPosition = value; }

        public List<GameObject> DrawnObjectsList
        {
            get { return GrattacieliToDraw.Cast<GameObject>().ToList(); }
        }

        public List<Grattacielo> CachedObjectList { get; }

        public void MoveX(float xamount)
        {
            for (int i = 0; i < GrattacieliToDraw.Count; ++i)
            {
                GrattacieliToDraw[i].PositionX += xamount;
            }
        }

        public void Reset(Camera camera)
        {
            if (FirstOnePointer != null)
            {
                CurrentCamera = camera;

                for (int i = 0; i < GrattacieliToDraw.Count; ++i)
                {
                    CachedObjectList.Add(GrattacieliToDraw[i]);
                    GrattacieliToDraw.RemoveAt(i);
                    --i;
                }

                int index;
                for (index = 0; index < CachedObjectList.Count; ++index)
                {
                    if (CachedObjectList[index] == FirstOnePointer)
                    {
                        break;
                    }
                }

                while (CachedObjectList[0] != FirstOnePointer)
                {
                    CachedObjectList.Add(CachedObjectList[0]);
                    CachedObjectList.RemoveAt(0);
                }

                NextGrattacieloPosition = new Vector2(0.0f, _resolutionH);

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
                        _textureReference);
                CachedObjectList.Add(tmpObj);

                if (FirstOnePointer == null)
                {
                    FirstOnePointer = tmpObj;
                }
            }
        }

        private void AddGrattacieloForDrawingInit()
        {
            for (int i = 0; i < 16; ++i)
            {
                CachedObjectList[0].Position = NextGrattacieloPosition;

                _nextGrattacieloPosition.X +=
                     CachedObjectList[0].Width +
                     FbonizziMonoGame.Numbers.RandomBetween(1, MaxGrattacieloPositionOffset);

                GrattacieliToDraw.Add(CachedObjectList[0]);
                CachedObjectList.RemoveAt(0);
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
            {
                AddGrattacieloForDrawing();
            }

            for (int i = 0; i < GrattacieliToDraw.Count(); ++i)
            {
                Grattacielo g = GrattacieliToDraw[i];

                g.Update(gametime);

                if (ToBeRemoved(CameraPositionX, (int)g.Position.X, g.Width))
                {
                    CachedObjectList.Add(g);
                    GrattacieliToDraw.RemoveAt(i);
                    --i;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < GrattacieliToDraw.Count; ++i)
            {
                GrattacieliToDraw[i].Draw(spriteBatch);
            }
        }

        private void AddGrattacieloForDrawing()
        {
            float cameraXFirst = CameraPositionX;
            float cameraXLast = cameraXFirst + _cameraW;

            while (GrattacieliToDraw.Count < NumGrattacieliToDraw
                && CachedObjectList.Count > 0
                && NextGrattacieloPosition.X <= cameraXLast)
            {
                CachedObjectList[0].Position = NextGrattacieloPosition;

                _nextGrattacieloPosition.X +=
                     CachedObjectList[0].Width +
                     FbonizziMonoGame.Numbers.RandomBetween(1, MaxGrattacieloPositionOffset);

                if (_innestGemma)
                {
                    if (FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) < _gameManagerReference.GemmaProbability)
                    {
                        _gameManagerReference.AddGemma(
                            (new Vector2(
                                  CachedObjectList[0].Position.X + 15,
                                  _resolutionH - CachedObjectList[0].Height - 70)));
                    }
                    else if (FbonizziMonoGame.Numbers.RandomBetween(0D, 1D) < _gameManagerReference.PowerUpProbability)
                    {
                        if (!_gameManagerReference.JalapenosModeActive && !_gameManagerReference.MerdaModeActive)
                        {
                            _gameManagerReference.AddPowerUp(new Vector2(
                                    CachedObjectList[0].PositionAtTopLeftCorner().X,
                                    CachedObjectList[0].PositionAtTopLeftCorner().Y - 180));
                        }
                    }
                }

                GrattacieliToDraw.Add(CachedObjectList[0]);
                CachedObjectList.RemoveAt(0);
            }
        }
    }
}