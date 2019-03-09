
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace fge
{
    public class BackgroundManager 
    {
        
        private GrattacieliAutogeneranti_episodio1 grattacieli_fondo_ = null;
        private GrattacieliAutogeneranti_episodio1 grattacieli_mid_ = null;

        private float parallax_speed_fondo_;
        private float parallax_speed_mid_;
        private const float default_parallax_speed_fondo_ = -10.0f;
        private const float default_parallax_speed_mid_ = -18.0f;

        private Nuvolificio nuvolificio_vicino_;
        private Nuvolificio nuvolificio_medio_;
        private Nuvolificio nuvolificio_lontano_;
        private Vector2 nuvole_default_spawn_y_range_ = new Vector2(300.0f, -300.0f);

        private StarFieldParticleSystem starfield_;
        private Vector2 cielo_stellato_spawn_y_range_ = new Vector2(-960.0f, -300.0f);
        const double timeBetweenNewStar_ = 20.0f;
        double timeTillNewStar_ = 0.0f;


        protected Camera current_camera_;
        protected float old_camera_x_pos_;
        protected float parallax_dir_ = +1;

        protected Texture2D texture_reference_;
        protected Rectangle sfondo_rectangle_;
        protected Vector2 sfondo_origin_;
        protected Vector2 sfondo_scale_;


        public BackgroundManager(
            Camera CameraInstance,
            Loader Loader,
            InfartGame GameManagerReference)
        {
            current_camera_ = CameraInstance;
            old_camera_x_pos_ = current_camera_.Position.X;

            sfondo_rectangle_ = Loader.textures_rectangles_["background"];
            texture_reference_ = Loader.textures_;
            sfondo_origin_ = new Vector2(0.0f, sfondo_rectangle_.Height);
            sfondo_scale_ = Vector2.One;

            grattacieli_fondo_ = new GrattacieliAutogeneranti_episodio1(
                Loader.textures_gratta_back_, Loader.textures_rectangles_, "back", 69, CameraInstance, GameManagerReference);

            sfondo_scale_ = new Vector2(1.8f);

            grattacieli_mid_ = new GrattacieliAutogeneranti_episodio1(
                Loader.textures_gratta_mid_, Loader.textures_rectangles_, "mid", 69, CameraInstance, GameManagerReference);

            parallax_speed_fondo_ = default_parallax_speed_fondo_;
            parallax_speed_mid_ = default_parallax_speed_mid_;


            List<Rectangle> tmp = new List<Rectangle>();
            tmp.Add(Loader.textures_rectangles_["nuvola1"]);
            tmp.Add(Loader.textures_rectangles_["nuvola2"]);
            tmp.Add(Loader.textures_rectangles_["nuvola3"]);


            nuvolificio_vicino_ = new Nuvolificio(
                Color.White, 0.6f, new Vector2(45, 60f),
                nuvole_default_spawn_y_range_, CameraInstance, tmp, Loader.textures_);

            nuvolificio_medio_ = new Nuvolificio(
               new Color(9, 50, 67), 0.4f, new Vector2(30f, 40f),
                nuvole_default_spawn_y_range_, CameraInstance, tmp, Loader.textures_);

            nuvolificio_lontano_ = new Nuvolificio(
                 new Color(5, 23, 40), 0.2f, new Vector2(10f, 20f),
                nuvole_default_spawn_y_range_, CameraInstance, tmp, Loader.textures_);

            starfield_ = new StarFieldParticleSystem(8, Loader);

            current_camera_ = CameraInstance;
            old_camera_x_pos_ = current_camera_.Position.X;
        }
        
        
        public void IncreaseParallaxSpeed()
        {
            parallax_speed_fondo_ -= 4.0f;
            parallax_speed_mid_ -= 4.0f;
        }

        public void DecreaseParallaxSpeed()
        {
            parallax_speed_fondo_ += 4.0f;
            parallax_speed_mid_ += 4.0f;
        }

        public void Reset(Camera camera)
        {
            current_camera_ = camera;
            old_camera_x_pos_ = current_camera_.Position.X;
            grattacieli_fondo_.Reset(camera);
            grattacieli_mid_.Reset(camera);

            nuvolificio_lontano_.Reset(camera);
            nuvolificio_medio_.Reset(camera);
            nuvolificio_vicino_.Reset(camera);

            parallax_speed_fondo_ = default_parallax_speed_fondo_;
            parallax_speed_mid_ = default_parallax_speed_mid_;
        }

        

        
        public void Update(double gametime)
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
            
            grattacieli_fondo_.MoveX(parallax_speed_fondo_ * dt * parallax_dir_);
            grattacieli_mid_.MoveX(parallax_speed_mid_ * dt * parallax_dir_);
            nuvolificio_lontano_.MoveX((float)((parallax_speed_fondo_) * dt * parallax_dir_));
            nuvolificio_medio_.MoveX((float)((parallax_speed_mid_) * dt * parallax_dir_));

            grattacieli_fondo_.Update(gametime, current_camera_);
            grattacieli_mid_.Update(gametime, current_camera_);

            starfield_.Update(gametime);

            if (current_camera_.Position.Y >= cielo_stellato_spawn_y_range_.X
                && current_camera_.Position.Y <= cielo_stellato_spawn_y_range_.Y)
                GenerateStars(gametime);

            nuvolificio_lontano_.Update(gametime);
            nuvolificio_medio_.Update(gametime);
            nuvolificio_vicino_.Update(gametime);
        }

        private void GenerateStars(double dt)
        {
            timeTillNewStar_ -= dt;
            if (timeTillNewStar_ < 0)
            {
                Vector2 where = new Vector2(
                    fbonizziHelper.random.Next((int)current_camera_.Position.X, (int)current_camera_.Position.X + current_camera_.ViewPortWidth),
                    fbonizziHelper.random.Next((int)cielo_stellato_spawn_y_range_.X, (int)cielo_stellato_spawn_y_range_.Y));

                starfield_.AddParticles(where);

                timeTillNewStar_ = timeBetweenNewStar_;
            }
        }

        public void Draw(SpriteBatch spritebatch)
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

            starfield_.Draw(spritebatch);

            nuvolificio_lontano_.Draw(spritebatch);
            grattacieli_fondo_.Draw(spritebatch);
            nuvolificio_medio_.Draw(spritebatch);
            grattacieli_mid_.Draw(spritebatch);
        }

        public void DrawSpecial(SpriteBatch spritebatch)
        {
            nuvolificio_vicino_.Draw(spritebatch);
        }

        
    }
}
