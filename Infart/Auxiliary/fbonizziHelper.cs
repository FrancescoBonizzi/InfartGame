using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fge
{
    public static class fbonizziHelper
    {
        public static Random random = new Random();
        public static Loader_menu LoaderMenu;

        public static void ReferenceToLoaderMenu(Loader_menu reference)
        {
            if (LoaderMenu == null)
                LoaderMenu = reference;
        }

        public static float RandomBetween(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}
