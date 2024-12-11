using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ECS_Tankz_1
{
    public static class ColliderFactory
    {
        public static Collider CreateCircleFor(GameObject obj)
        {
            SpriteRenderer sr = obj.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

            return new CircleCollider(obj, (float)Math.Sqrt(sr.Width * sr.Width + sr.Height * sr.Height) * 0.5f, Vector2.Zero);
        }

        public static Collider CreateBoxFor(GameObject obj)
        {
            SpriteRenderer sr = obj.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

            return new BoxCollider(obj, sr.Width, sr.Height, new Vector2((0.5f - sr.Pivot.X) * sr.Width, (0.5f - sr.Pivot.Y) * sr.Height));
        }
    }
}
