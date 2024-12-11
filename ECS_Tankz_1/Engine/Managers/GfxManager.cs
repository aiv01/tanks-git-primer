using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace ECS_Tankz_1
{
    static class GfxManager
    {
        private static Dictionary<string, Texture> textures;

        static GfxManager()
        {
            textures = new Dictionary<string, Texture>();
        }

        public static Texture AddTexture(string name, string path)
        {
            Texture t = new Texture(path);
            textures.Add(name, t);
            return t;
        }

        public static Texture GetTexture(string name)
        {
            return textures[name];
        }

        public static void ClearAll()
        {
            textures.Clear();
        }
    }
}
