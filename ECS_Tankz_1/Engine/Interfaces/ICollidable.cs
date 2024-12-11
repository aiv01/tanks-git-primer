using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    interface ICollidable
    {
        void OnCollide(Collision CollisionInfo);
    }
}
