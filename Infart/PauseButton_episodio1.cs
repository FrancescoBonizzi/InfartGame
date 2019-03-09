using Microsoft.Xna.Framework;

namespace fge
{
    public class PauseButton_episodio1 : Button
    {
        public PauseButton_episodio1(
            Loader_episodio1 Loader)
            : base(
            "Pause", new Vector2(720, 430),
            Loader.textures_rectangles_["Pause"], Loader.textures_rectangles_["Play"],
            Color.White, false, true, Loader.font_, Loader.textures_)
        { }

            
    }
}
