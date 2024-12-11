using OpenTK;

namespace ECS_Tankz_1
{
    public class ShootingState : State {

        //Working variable
        private bool inShooting;
        private float currentSpeed; //velocità finale del proiettile quando molli oppure arriva alla fine
        private float X_Scale;


        //State variable
        private float maxSpeed;
        private float speedRate;
        private GameObject backgroundBar;
        private GameObject shootingBar;
        private GameObject cannon;
        private int nextState;
        private float cannonRotationSpeed;

        //Fast reference
        private BulletManager bulletMgr;



        public ShootingState (float maxSpeed,float speedRate, GameObject backgroundBar, GameObject shootingBar, GameObject cannon, int nextState, float cannonRotationSpeed) {
            this.maxSpeed = maxSpeed;
            this.speedRate = speedRate;
            this.backgroundBar = backgroundBar;
            this.shootingBar = shootingBar;
            this.cannon = cannon;
            this.nextState = nextState;
            this.cannonRotationSpeed = cannonRotationSpeed;
            X_Scale = shootingBar.Transform.Scale.X;
        }

        public override void Awake () {
            bulletMgr = GameObject.Find ("BulletMgr").GetComponent<BulletManager> (); 
        }

        public override void Enter () {
            currentSpeed = 0;
            inShooting = Input.GetButton ("P_Shoot");
            if (inShooting) PerformInShooting ();
        }

        public override void Update () {
            if (!inShooting) {
                cannon.Transform.Rotation += Input.GetAxis ("P_Vertical") * Game.DeltaTime * cannonRotationSpeed;
                if (Input.GetButtonDown ("P_Shoot")) {
                    inShooting = true;
                    PerformInShooting ();
                }
            } else {
                currentSpeed += speedRate * Game.DeltaTime;
                UpdateBarScale ();
                if (Input.GetButtonUp ("P_Shoot") || currentSpeed >= maxSpeed) {
                    Shoot ();
                    return;
                }
            }
        }

        private void PerformInShooting () {
            backgroundBar.IsActive = true;
            shootingBar.IsActive = true;
            shootingBar.Transform.Scale = new Vector2 (0 , shootingBar.Transform.Scale.Y);
        }

        private void UpdateBarScale () {
            shootingBar.Transform.Scale = new Vector2 (X_Scale * (currentSpeed / maxSpeed) , shootingBar.Transform.Scale.Y);
        }

        private void Shoot () {
            Bullet bullet = bulletMgr.GetBullet (BulletType.Default);
            if (bullet != null) {
                bullet.Shoot (cannon.Transform.Position + cannon.Transform.Forward * cannon.GetComponent<SpriteRenderer> ().Width , cannon.Transform.Forward * currentSpeed, machine.GetComponent<PlayerController>().PlayerID);
            }
            machine.Switch (nextState);
        }

        public override void Exit () {
            backgroundBar.IsActive = false;
            shootingBar.IsActive = false;
        }

    }
}
