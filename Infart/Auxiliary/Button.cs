
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input.Touch;



namespace fge
{
    public class Button
    {
        
        private string name_;
        private string text_;

        private Rectangle state1_texture_ = Rectangle.Empty;
        private Rectangle state2_texture_ = Rectangle.Empty;
        private Rectangle current_drawn_texture_ = Rectangle.Empty;

        private Texture2D texture_reference_;

        private Vector2 position_ = Vector2.Zero;

        private Vector2 font_position_ = Vector2.Zero;
        private SpriteFont font_;
        private Color font_color_ = Color.Black;

        private Color overlay_color_ = Color.White;

        private Rectangle collision_rectangle_;

        private bool draw_font_ = true;
        private bool toggle_button_ = false;

        private Point finger_position_;

        enum CustomButtonState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
        private CustomButtonState current_button_state_ = CustomButtonState.UP;
        private double timer_ = 0.0;

        

        
        public Button(
            string text,
            Vector2 position,
            Rectangle ButtonTexture_State1,
            Rectangle ButtonTexture_State2,
            Color FontColor,
            bool DrawFont,
            bool ToggleButton,
            SpriteFont Font,
            Texture2D TextureReference)
        {
            toggle_button_ = ToggleButton;

            texture_reference_ = TextureReference;

            position_ = position;
            state1_texture_ = ButtonTexture_State1;
            font_color_ = FontColor;

            font_ = Font;

            font_position_ = new Vector2(
                position_.X + ButtonTexture_State1.Width / 2 - font_.MeasureString(text).X / 2,
                position_.Y + ButtonTexture_State1.Height / 2 - font_.MeasureString(text).Y / 2);

            collision_rectangle_ = new Rectangle(
                (int)position_.X,
                (int)position_.Y,
                state1_texture_.Width,
                state1_texture_.Height);

            current_drawn_texture_ = ButtonTexture_State1;

            draw_font_ = DrawFont;

            text_ = text;
            name_ = text;

            state2_texture_ = ButtonTexture_State2;
        }

        public void Reset()
        {
            current_drawn_texture_ = state1_texture_;
        }

        

        
        public Vector2 Position
        {
            set
            {
                collision_rectangle_.X = (int)value.X;
                collision_rectangle_.Y = (int)value.Y;

                font_position_ = new Vector2(
                    value.X + collision_rectangle_.Width / 2 - font_.MeasureString(text_).X / 2,
                    value.Y + collision_rectangle_.Height / 2 - font_.MeasureString(text_).Y / 2);

                position_ = value;
            }
        }

        public Rectangle CollisionRectangle
        {
            get { return collision_rectangle_; }
        }

        public bool Pressed
        {
            get
            {
                return current_button_state_ == CustomButtonState.JUST_RELEASED;
            }
        }

        public string Name
        {
            get { return name_; }
        }

        

        
        public void SwitchTexture()
        {
            if (current_drawn_texture_ == state1_texture_)
                current_drawn_texture_ = state2_texture_;
            else current_drawn_texture_ = state1_texture_;
        }

        

        
        public void Update(double gameTime, TouchCollection touch)
        {
            if (touch.Count != 0)
            {
                
                TouchLocation t = touch[0];

                finger_position_ = new Point((int)t.Position.X, (int)t.Position.Y);

                if (CollisionRectangle.Contains(finger_position_))
                {
                    timer_ = 0.0;

                    if (t.State == TouchLocationState.Pressed)
                    {
                        current_button_state_ = CustomButtonState.DOWN;
                        if (!toggle_button_)
                            current_drawn_texture_ = state2_texture_;

                    }
                    else if (t.State == TouchLocationState.Released)
                    {
                        if (current_button_state_ == CustomButtonState.DOWN)
                        {
                            
                            current_button_state_ = CustomButtonState.JUST_RELEASED;
                            if (toggle_button_)
                                SwitchTexture();
                        }
                    }
                }
                else
                {
                    current_button_state_ = CustomButtonState.UP;

                    if (timer_ > 0)
                    {
                        timer_ -= 0.017; 
                    }
                    else
                    {
                        if (!toggle_button_)
                            current_drawn_texture_ = state1_texture_;

                        overlay_color_ = Color.White;
                    }

                }
            }
            else
            {
                current_button_state_ = CustomButtonState.UP;

                if (timer_ > 0)
                {
                    timer_ -= 0.017; 
                }
                else
                {
                    if (!toggle_button_)
                        current_drawn_texture_ = state1_texture_;

                    overlay_color_ = Color.White;
                }

            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture_reference_,
                collision_rectangle_,
                current_drawn_texture_,
                overlay_color_);

            if (draw_font_)
            {
                spriteBatch.DrawString(
                    font_,
                    text_,
                    font_position_,
                    font_color_);
            }
        }

        
    }
}