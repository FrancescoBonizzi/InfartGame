using Infart.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infart.HUD
{
    public class StatusBar
    {
        private readonly StatusBarSprite[] _statusBurgers;
        private readonly Texture2D _textureReference;
        private readonly SoundManager _soundManagerReference;
        private readonly Color _emptyColor = Color.Gray * 0.5f;
        private readonly Color _fullColor = Color.White;

        private int _jumpCount;
        public int TotalJumps { get; private set; }

        private const int JumpNeededToRemoveHam = 1;
        private const int SogliaHamburgerPerStarMale = 4;
        private float _overlayDeathOpacity = 0.01f;

        private readonly Rectangle _sfondoMorteSourceRect;
        private readonly Rectangle _sfondoMorteDestinationRect = new Rectangle(0, 0, 800, 480);

        private bool _infart;

        public StatusBar(
            Vector2 position,
            AssetsLoader assetsLoader,
            SoundManager soundManagerReference)
        {
            _textureReference = assetsLoader.Textures;

            _soundManagerReference = soundManagerReference;

            _statusBurgers = new StatusBarSprite[4];
            Vector2 tmpPos = new Vector2(position.X, position.Y + 20);
            int hamWidth = assetsLoader.TexturesRectangles["Burger"].Width;
            Vector2 hamScale = new Vector2(0.9f);
            _sfondoMorteSourceRect = assetsLoader.TexturesRectangles["death_screen"];

            for (int i = 0; i < 4; ++i)
            {
                _statusBurgers[i] = new StatusBarSprite(
                    assetsLoader.Textures,
                    assetsLoader.TexturesRectangles["Burger"],
                    tmpPos,
                    _emptyColor,
                    hamScale);
                tmpPos.X += hamWidth;
            }
        }

        public void Reset()
        {
            _infart = false;
            SetHamburgers(0);

            for (int i = 0; i < 4; ++i)
            {
                _statusBurgers[i].Reset();
                _statusBurgers[i].FillColor = _emptyColor;
            }
            _overlayDeathOpacity = 0.01f;
            VerdureMangiateInTotale = 0;
            _jumpCount = 0;
        }

        public int CurrentHamburgers { get; private set; } = 0;

        public int VerdureMangiateInTotale { get; private set; } = 0;

        public bool IsInfarting()
        {
            if (CurrentHamburgers > SogliaHamburgerPerStarMale || _infart)
            {
                _infart = true;
                return true;
            }

            return false;
        }

        public void HamburgerEaten()
        {
            SetHamburgers(GetHamburgers() + 1);
            _statusBurgers[CurrentHamburgers - 1].Taken();
            _statusBurgers[CurrentHamburgers - 1].FillColor = _fullColor;
            _jumpCount = 0;

            if (CurrentHamburgers >= SogliaHamburgerPerStarMale)
            {
                _statusBurgers[CurrentHamburgers - 1].FillColor = Color.LightCoral;
                _soundManagerReference.PlayHeartBeat();
            }
        }

        public void PlayerJumped()
        {
            ++_jumpCount;
            TotalJumps++;

            if (_jumpCount == JumpNeededToRemoveHam)
            {
                SetHamburgers(GetHamburgers() - 1);
                _statusBurgers[CurrentHamburgers].Lost();
                _statusBurgers[CurrentHamburgers].FillColor = _emptyColor;
                _jumpCount = 0;
            }

            if (CurrentHamburgers < SogliaHamburgerPerStarMale)
            {
                _soundManagerReference.StopHeartBeat();
            }
        }

        public void SetInfart()
        {
            _infart = true;
        }

        private int GetHamburgers()
        {
            return CurrentHamburgers;
        }

        private void SetHamburgers(int value)
        {
            if (value < 0)
            {
                CurrentHamburgers = 0;
            }
            else if (value > 4)
            {
                _infart = true;
            }
            else
            {
                CurrentHamburgers = value;
            }
        }

        public void ComputeJalapenos()
        {
            for (int i = 0; i < CurrentHamburgers; ++i)
            {
                _statusBurgers[i].Lost();
                _statusBurgers[i].FillColor = _emptyColor;
            }

            SetHamburgers(0);
            ++VerdureMangiateInTotale;
            _soundManagerReference.StopHeartBeat();
        }

        public void ComputeBroccolo()
        {
            for (int i = 0; i < CurrentHamburgers; ++i)
            {
                _statusBurgers[i].Lost();
                _statusBurgers[i].FillColor = _emptyColor;
            }

            SetHamburgers(0);
            ++VerdureMangiateInTotale;
            _soundManagerReference.StopHeartBeat();
        }

        public void Update(double gametime)
        {
            if (!_infart)
            {
                for (int i = 0; i < 4; ++i)
                {
                    _statusBurgers[i].Update(gametime);
                }

                if (CurrentHamburgers == SogliaHamburgerPerStarMale)
                {
                    if (_overlayDeathOpacity < 1.0f)
                    {
                        _overlayDeathOpacity += 0.01f;
                    }
                }
                else
                {
                    if (_overlayDeathOpacity >= 0.01)
                    {
                        _overlayDeathOpacity -= 0.01f;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 4; ++i)
            {
                _statusBurgers[i].Draw(spriteBatch);
            }

            if (_overlayDeathOpacity > 0.01f)
            {
                spriteBatch.Draw(
                    _textureReference,
                    _sfondoMorteDestinationRect,
                    _sfondoMorteSourceRect,
                    Color.White * _overlayDeathOpacity);
            }
        }
    }
}