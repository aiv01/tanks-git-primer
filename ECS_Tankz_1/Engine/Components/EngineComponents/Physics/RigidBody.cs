using OpenTK;
using System;
using System.Collections.Generic;

namespace ECS_Tankz_1
{
    public enum RigidBodyType { player = 1, bullet = 2, tile = 4 }

    public class RigidBody : Component, IFixedUpdatable
    {
        public uint Type;   // RB type
        protected uint collisionMask;
        public Collider Collider {  get; set; }

        private Vector2 velocity;
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        private float friction;
        public float Friction{ get { return friction; } set { if (value >= 0) friction = value; } }

        public bool IsGravityAffected;
        public bool IsCollisionAffected;

        public RigidBody(GameObject owner) : base(owner)
        {
            IsGravityAffected = false;
            IsCollisionAffected = true;

            Collider = GameObject.GetComponent(typeof(Collider)) as Collider;
            PhysicsManager.AddItem(this);
        }

        public virtual void FixedUpdate()
        {
            if(IsGravityAffected)
            {
                velocity.Y += Game.Gravity * Game.DeltaTime;
            }

            if (velocity.LengthSquared != 0)
            {
                float fAmount = friction * Game.DeltaTime;
                float newVelocityLength = velocity.Length - fAmount;
                if (newVelocityLength < 0) newVelocityLength = 0;
                velocity = velocity.Normalized() * newVelocityLength;
            }

            Transform.Position += Velocity * Game.DeltaTime;
        }

        public virtual bool Collides(RigidBody other, ref Collision Collision)
        {
            return Collider.Collides(other.Collider, ref Collision);
        }

        public void AddCollisionType(uint add)
        {
            collisionMask |= add;
        }

        public bool CollisionTypeMatches(uint type)
        {
            return (collisionMask & type) != 0;
        }

        public override Component Clone(GameObject owner)
        {
            RigidBody clone = new RigidBody(owner);
            clone.IsCollisionAffected = IsCollisionAffected;
            clone.IsGravityAffected = IsGravityAffected;
            clone.Velocity = Velocity;
            clone.collisionMask = collisionMask;
            clone.Type = Type;
            return clone;
        }
    }
}
