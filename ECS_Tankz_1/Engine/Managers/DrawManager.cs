using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public enum DrawLayer
    {
        Background, Middleground, Playground, Foreground, GUI, LAST
    }

    static class DrawManager
    {
        private static List<IDrawable>[] items;

        static DrawManager()
        {
            items = new List<IDrawable>[(int)DrawLayer.LAST];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new List<IDrawable> ();
            }
        }

        public static void AddItem(IDrawable item)
        {
            items[(int)item.Layer].Add(item);
        }

        public static void ClearAll()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Clear();
            }
        }

        public static void Draw()
        {
            for (int i = 0; i < items.Length; i++)
            {
                for (int j = 0; j < items[i].Count; j++)
                {
                    if(items[i][j].Enabled)
                    {
                        items[i][j].Draw();
                    }
                }
            }
        }
    }
}
