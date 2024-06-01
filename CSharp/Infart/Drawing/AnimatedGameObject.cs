using Infart.Astronaut;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Infart.Drawing
{
    public abstract class AnimatedGameObject : GameObject
    {
        protected Dictionary<string, AnimationManager> Animations = new Dictionary<string, AnimationManager>();

        protected string CurrentAnimation;

        protected int CurrentFrameWidth;

        protected int CurrentFrameHeight;

        public AnimatedGameObject()
        {
            _collisionRectangle = Rectangle.Empty;
        }

        public int Width
        {
            get { return CurrentFrameWidth; }
        }

        public int Height
        {
            get { return CurrentFrameHeight; }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                _collisionRectangle.X = (int)value.X;
                _collisionRectangle.Y = (int)value.Y;
                base.Position = value;
            }
        }

        private void UpdateAnimation(double gameTime)
        {
            if (Animations.ContainsKey(CurrentAnimation))
            {
                if (Animations[CurrentAnimation].FinishedPlaying)
                {
                    PlayAnimation(Animations[CurrentAnimation].NextAnimation);
                }
                else
                {
                    Animations[CurrentAnimation].Update(gameTime);
                }
            }
        }

        public void PlayAnimation(string name)
        {
            if (!(name == null) && Animations.ContainsKey(name))
            {
                CurrentAnimation = name;
                Animations[name].Play();
                CurrentFrameHeight = (int)Animations[CurrentAnimation].FrameHeight;
                CurrentFrameWidth = (int)Animations[CurrentAnimation].FrameWidth;
                _collisionRectangle.Width = CurrentFrameWidth;
                _collisionRectangle.Height = CurrentFrameHeight;
            }
        }

        public override void Update(double gameTime)
        {
            UpdateAnimation(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Animations.ContainsKey(CurrentAnimation))
            {
                spriteBatch.Draw(
                       Animations[CurrentAnimation].Texture,
                       base.Position,
                       Animations[CurrentAnimation].FrameRectangle,
                       _overlayColor,
                       _rotation,
                       _origin,
                       _scale,
                       _flip,
                       _depth);
            }
        }
    }
}