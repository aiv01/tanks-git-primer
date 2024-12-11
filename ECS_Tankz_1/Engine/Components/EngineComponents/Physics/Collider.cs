using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ECS_Tankz_1
{
    public abstract class Collider : Component
    {
        public Vector2 Position { get { return Transform.Position + Offset; } }
        public Vector2 Offset;

        public Collider(GameObject owner, Vector2 offset) : base(owner)
        {
            this.Offset = offset;
            // RB ref
            RigidBody rb = GameObject.GetComponent(typeof(RigidBody)) as RigidBody;

            if(rb != null)
            {
                rb.Collider = this;
            }
        }

        public abstract bool Collides(Collider aCollider, ref Collision collision);
        public abstract bool Collides(CircleCollider circle, ref Collision collision);
        public abstract bool Collides(BoxCollider box, ref Collision collision);
        public abstract bool Contains(Vector2 point);
    }
}
