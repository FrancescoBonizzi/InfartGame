#region Using


#endregion
namespace fge
{
    public class InfartExplosion_episodio1 : InfartExplosion
    {
        public InfartExplosion_episodio1(Loader_episodio1 Loader)
            : base(
            Loader.textures_,
            Loader.textures_rectangles_["Bang"],
            Loader.textures_rectangles_["Burger"],
            Loader.textures_rectangles_["Merda"])
        {
        }

    }
}
