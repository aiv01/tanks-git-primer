using OpenTK;
using System;

namespace ECS_Tankz_1
{
    public class MoveState : State {

        //Working Variables
        private float lastXPosition;
        private float movementDone;

        //Reference
        private RigidBody rb;

        //State Variable
        private float maxMovement;
        private int nextState;
        private Transform cannonTransform;
        private float speed;
        private float cannonRotationSpeed;

        public MoveState (float maxMovement, int nextState, Transform cannonTransform, float speed, float cannonRotationSpeed) {
            this.maxMovement = maxMovement;
            this.nextState = nextState;
            this.cannonTransform = cannonTransform;
            this.speed = speed;
            this.cannonRotationSpeed = cannonRotationSpeed;
        }

        public override void Awake () {
            rb = machine.GetComponent<RigidBody> ();
        }

        public override void Enter () {
            lastXPosition = rb.Transform.Position.X;
            movementDone = 0;
        }

        public override void Update () {
            rb.Velocity = new Vector2 (Input.GetAxis ("P_Horizontal") * speed , rb.Velocity.Y);
            cannonTransform.Rotation += Input.GetAxis ("P_Vertical") * Game.DeltaTime * cannonRotationSpeed;
            if (Input.GetButtonDown ("P_Shoot")) {
                machine.Switch (nextState);
                return;
            }
            movementDone += Math.Abs (lastXPosition - rb.Transform.Position.X);
            if (movementDone > maxMovement) {
                machine.Switch (nextState);
                return;
            }
            lastXPosition = rb.Transform.Position.X;
        }

        public override void Exit () {
            rb.Velocity = Vector2.Zero;
        }

    }
}
