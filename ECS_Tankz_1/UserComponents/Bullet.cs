using OpenTK;


namespace ECS_Tankz_1
{
    public class Bullet : UserComponent 
    {
        private int playerID;
        public int PlayerID
        {
            get { return playerID; }
        }

        private float damage;
        public float Damage
        {
            get { return damage; }
        }
        private BulletType myType;

        private RigidBody rb;
        private GameLogic gameLogic;

        public Bullet (GameObject owner, float damage, BulletType type) : base (owner) {
            this.damage = damage;
            myType = type;
        }

        public override void Awake () {
            rb = GameObject.GetComponent<RigidBody> ();
            GameObject.IsActive = false;
            gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        }

        public void Shoot (Vector2 startPosition, Vector2 velocity, int playerID) {
            Transform.Position = startPosition;
            rb.Velocity = velocity;
            GameObject.IsActive = true;
            this.playerID = playerID;
            gameLogic.OnBulletSpawned(this);
        }

        public override void Update () {
            Transform.Forward = rb.Velocity;
        }

        public override void LateUpdate () {
            if (!CameraManager.InsideCameraLimits (Transform.Position) && Transform.Position.Y > 0) {
                DestroyBullet ();
            }
        }

        public void DestroyBullet () {
            GameObject.IsActive = false;
            gameLogic.OnBulletDestroyed();
        }

        public override void OnCollide(Collision CollisionInfo)
        {
            if (CollisionInfo.Collider.GameObject.Tag == "Box")
            {
                CollisionInfo.Collider.GameObject.IsActive = false;
                DestroyBullet();
            }
        }


        public override Component Clone (GameObject owner) {
            return new Bullet (owner , damage, myType);
        }

    }
}
