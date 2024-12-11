using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ECS_Tankz_1
{
    public class Transform
    {
        private Vector2 position;
        private float rotation;
        private Vector2 scale;

        public Vector2 Position { get { return position; } set {  position = value; } }
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public Vector2 Scale { get { return scale; } set { scale = value; } }

        public Vector2 Forward
        {
            get
            {
                return new Vector2((float)Math.Cos(DegreesToRadiants(rotation)), (float)Math.Sin(DegreesToRadiants(rotation)));
            }
            set
            {
                rotation = RadiantsToDegrees((float)Math.Atan2(value.Y, value.X));
            }
        }



        public Transform(Vector2 position, Vector2 scale, float rotation = 0)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public static float RadiantsToDegrees(float radiants)
        {
            return (180 / (float)Math.PI) * radiants;
        }

        public static float DegreesToRadiants(float degrees)
        {
            return (float)Math.PI * degrees / 180;
        }
    }
}
