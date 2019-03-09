
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;




namespace fge
{
    public class FPScounter
    {
        SpriteFont _spr_font;
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;
        StringBuilder fpscache_;

        public FPScounter(SpriteFont font)
        {
            _spr_font = font;
            fpscache_ = new StringBuilder();
            fpscache_.Clear();
        }

        public void Update(double gameTime)
        {
            
            _elapsed_time += (float)gameTime;

            
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
                fpscache_.Clear();
                fpscache_.Append("FPS: ");
                StringBuilderExtensions.AppendNumber(fpscache_, _fps);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            _total_frames++;

            spriteBatch.DrawString(
                _spr_font,
                fpscache_,
                new Vector2(10.0f, 20.0f),
                Color.White);
        }


    }
}
