
namespace ECS_Tankz_1
{
    public class ClampRotation : UserComponent {

        private float minValue;
        private float maxValue;

        public ClampRotation (GameObject owner, float minValue, float maxValue) : base (owner) {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public override void LateUpdate () {
            float rotation = Transform.Rotation;
            rotation = rotation < minValue ? minValue : rotation > maxValue ? maxValue : rotation;
            Transform.Rotation = rotation;
        }

        public override Component Clone (GameObject owner) {
            return new ClampRotation (owner , minValue , maxValue);
        }

    }
}
