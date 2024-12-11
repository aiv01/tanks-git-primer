using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ECS_Tankz_1
{
    public class StopOnBox : UserComponent
    {
        private RigidBody rb;

        public StopOnBox(GameObject owner) : base(owner)
        {

        }

        public override void Awake()
        {
            rb = GetComponent<RigidBody>();
        }

        public override void OnCollide(Collision CollisionInfo)
        {
            if(CollisionInfo.Collider.GameObject.Tag == "Box")
            {
                OnBoxCollide(CollisionInfo);
            }
        }

        private void OnBoxCollide(Collision CollisionInfo)
        {
            if(CollisionInfo.Delta.X < CollisionInfo.Delta.Y)
            {   // Hor Collisions
                if(Transform.Position.X < CollisionInfo.Collider.Position.X)
                {
                    // Left Collision
                    CollisionInfo.Delta.X = -CollisionInfo.Delta.X;
                }

                Transform.Position = new Vector2(Transform.Position.X + CollisionInfo.Delta.X, Transform.Position.Y);
                rb.Velocity = new Vector2(0.0f, rb.Velocity.Y);
            }
            else
            {   // Ver Collisions
                if (Transform.Position.Y < CollisionInfo.Collider.Position.Y)
                {
                    // Top Collision
                    CollisionInfo.Delta.Y = -CollisionInfo.Delta.Y;
                }

                if(Transform.Position.Y < CollisionInfo.Collider.Transform.Position.Y)
                {
                    rb.Velocity = new Vector2(rb.Velocity.X, 0.0f);
                    Transform.Position = new Vector2(Transform.Position.X, Transform.Position.Y + CollisionInfo.Delta.Y);

                }
            }
        }

        public override Component Clone(GameObject owner)
        {
            throw new NotImplementedException();
        }
    }
}
