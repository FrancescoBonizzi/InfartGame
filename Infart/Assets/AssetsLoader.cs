using FbonizziMonoGame.Assets;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
