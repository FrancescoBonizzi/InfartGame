
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace fge
{
    
    
    
    public abstract class BackgroundManager
    {
        
        protected Camera current_camera_;
        protected float old_camera_x_pos_;
        protected float parallax_dir_ = +1;

        protected Texture2D texture_reference_;
        protected Rectangle sfondo_rectangle_;
        protected Vector2 sfondo_origin_;
        protected Vector2 sfondo_scale_;

        

        
        public BackgroundManager(
            Camera CameraInstance,
            Rectangle SfondoRectangle,
            Texture2D TextureReference)
        {
            current_camera_ = CameraInstance;
            old_camera_x_pos_ = current_camera_.Position.X;

            sfondo_rectangle_ = SfondoRectangle;
            texture_reference_ = TextureReference;
            sfondo_origin_ = new Vector2(0.0f, sfondo_rectangle_.Height);
            sfondo_scale_ = Vector2.One;
        }

        

        
        public abstract void IncreaseParallaxSpeed();
        public abstract void DecreaseParallaxSpeed();

        public abstract void Reset(Camera camera);

        

        
        public virtual void Update(double gametime)
        {
            
            if (old_camera_x_pos_ - current_camera_.Position.X < 0)
            {
                parallax_dir_ = +1;
            }
            else if (old_camera_x_pos_ - current_camera_.Position.X > 0)
            {
                parallax_dir_ = -1;
            }
            else
            {
                parallax_dir_ = 0;
            }

            old_camera_x_pos_ = current_camera_.Position.X;

            float dt = (float)gametime / 1000.0f;

        }

        public abstract void DrawSpecial(SpriteBatch spritebatch);

        
        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(
                texture_reference_,
                new Vector2(current_camera_.Position.X, current_camera_.ViewPortHeight),
                sfondo_rectangle_,
                Color.White,
                0.0f,
                sfondo_origin_,
                sfondo_scale_,
                SpriteEffects.None,
                1.0f);
        }

        
    }
}
