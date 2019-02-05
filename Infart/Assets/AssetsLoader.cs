using FbonizziMonoGame.Assets;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infart.Assets
{
    public class AssetsLoader
    {
        private readonly ContentManager _contentManager;

        public IDictionary<string, Sprite> Sprites { get; } = new Dictionary<string, Sprite>();
        public IDictionary<string, SoundEffect> Sounds { get; } = new Dictionary<string, SoundEffect>();

        public AssetsLoader(
            ContentManager contentManager,
            ITextFileLoader fileLoader)
        {
            _contentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
            LoadResources();
        }

        private void LoadResources()
        {
            // TitleFont = _contentManager.Load<SpriteFont>("font-title");
            

            //const string gameSpriteSheetName = "Spritesheet";
            //var gameSpriteSheet = _contentManager.Load<Texture2D>($"{gameSpriteSheetName}");
            //var gameSpritesDescriptions = _textureImporter.Import($"Content/{gameSpriteSheetName}.txt");

            //AddSpritesFromDictionary(gameSpritesDescriptions, gameSpriteSheet);

            //Sounds.Add("effect-win", _contentManager.Load<SoundEffect>("effect-ok"));
            //Sounds.Add("effect-loose", _contentManager.Load<SoundEffect>("effect-wrong"));
            //Sounds.Add("music-menu", _contentManager.Load<SoundEffect>("music-menu"));
            //Sounds.Add("music-playing", _contentManager.Load<SoundEffect>("music-play"));
        }

        private void AddSpritesFromDictionary(
            IDictionary<string, SpriteDescription> textureDictionary,
            Texture2D spriteSheet)
        {
            foreach (var texture in textureDictionary)
            {
                Sprites.Add(texture.Key, new Sprite(
                    texture.Value,
                    spriteSheet));
            }
        }
    }
}
