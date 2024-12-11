using OpenTK;
using System;
using System.Collections.Generic;

namespace ECS_Tankz_1
{
    public class FollowTransform : UserComponent
    {
        private Transform transformToFollow;
        private Vector2 offset;

        public FollowTransform(GameObject owner, Transform transformToFollow, Vector2 offset) : base(owner)
        {
            this.offset = offset;
            this.transformToFollow = transformToFollow;
        }

        public override void LateUpdate()
        {
            Transform.Position = transformToFollow.Position + offset;
        }

        public override Component Clone(GameObject owner)
        {
            return new FollowTransform(owner, transformToFollow, offset);
        }
    }
}
