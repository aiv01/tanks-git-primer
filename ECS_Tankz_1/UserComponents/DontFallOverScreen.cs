using OpenTK;

namespace ECS_Tankz_1
{
    public class DontFallOverScreen : UserComponent {

        private float offset;
        public bool OnGround = false;

        private RigidBody myRigidbody;

        public DontFallOverScreen (GameObject owner, float offset)  : base (owner) {
            this.offset = offset;
        }

        public override void Awake () {
            myRigidbody = GetComponent<RigidBody> ();
        }

        public override void LateUpdate () {
            if (Transform.Position.Y > Game.Win.OrthoHeight - offset) {
                ReachGround (Game.Win.OrthoHeight - offset);
            }
        }

        private void ReachGround (float yPosition) {
            Transform.Position = new Vector2 (Transform.Position.X , yPosition);
            myRigidbody.Velocity = new Vector2 (myRigidbody.Velocity.X , 0);
            myRigidbody.IsGravityAffected = false;
            OnGround = true;
        }

        public override Component Clone (GameObject owner) {
            return new DontFallOverScreen (owner , offset);
        }

    }
}
