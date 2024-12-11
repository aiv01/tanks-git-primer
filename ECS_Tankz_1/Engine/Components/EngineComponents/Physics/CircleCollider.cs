using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public class CircleCollider : Collider
    {
        public float Radius;

        public CircleCollider(GameObject owner, float radius, Vector2 offset) : base(owner, offset)
        {
            Radius = radius;
        }

        public override bool Collides(Collider aCollider, ref Collision Collision)
        {
            return aCollider.Collides(this, ref Collision);
        }

        //Circle vs Circle
        public override bool Collides(CircleCollider other, ref Collision Collision)
        {
            Vector2 dist = other.Position - Position;
            return dist.LengthSquared <= Math.Pow(Radius + other.Radius, 2);
        }

        //Circle vs Box
        public override bool Collides(BoxCollider box, ref Collision Collision)
        {
            return box.Collides(this, ref Collision);
        }

        public override bool Contains(Vector2 point)
        {
            Vector2 distFromCenter = point - Position;

            return distFromCenter.Length <= Radius;
        }

        public override Component Clone(GameObject owner)
        {
            return new CircleCollider(owner, Radius, Offset);
        }
    }
}
