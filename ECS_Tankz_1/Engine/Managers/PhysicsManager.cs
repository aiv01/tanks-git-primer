using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public enum CollisionType { None, RectsIntersection };

    public struct Collision
    {
        public Vector2 Delta;
        public Collider Collider;
        public CollisionType Type;
    }

    public static class PhysicsManager
    {
        private static List<RigidBody> items = new List<RigidBody>();
        static Collision Collision;

        public static void AddItem(RigidBody item)
        {
            items.Add(item);
        }

        public static void RemoveItem(RigidBody item)
        {
            items.Remove(item);
        }

        public static void ClearAll()
        {
            items.Clear();
        }

        public static void FixedUpdate()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].Enabled)
                {
                    items[i].FixedUpdate();
                }
            }
        }

        public static void CheckCollisions()
        {
            for (int i = 0; i < items.Count - 1; i++)
            {
                if (items[i].IsCollisionAffected && items[i].Enabled)
                {
                    //check collisions with next items
                    for (int j = i + 1; j < items.Count; j++)
                    {
                        if (items[j].IsCollisionAffected && items[j].Enabled)
                        {
                            //check if one of two rigid bodies is interested in collision
                            bool firstCheck = items[i].CollisionTypeMatches(items[j].Type);
                            bool secondCheck = items[j].CollisionTypeMatches(items[i].Type);

                            if ((firstCheck || secondCheck) && items[i].Collides(items[j], ref Collision))
                            {
                                //Console.WriteLine(items[i].Type + " Collides with " + items[j].Type);

                                if (firstCheck)
                                {
                                    Collision.Collider = items[j].Collider;
                                    items[i].GameObject.OnCollide(Collision);
                                }

                                if (secondCheck)
                                {
                                    Collision.Collider = items[i].Collider;
                                    items[j].GameObject.OnCollide(Collision);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
