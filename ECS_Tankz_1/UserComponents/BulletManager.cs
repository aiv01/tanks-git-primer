using OpenTK;

namespace ECS_Tankz_1
{

    public enum BulletType {
        Default,
        Last
    }

    public class BulletManager : UserComponent {

        private Bullet[,] pool;

        public BulletManager (GameObject owner, int poolSize) : base (owner) {
            pool = new Bullet[(int) BulletType.Last , poolSize];
            for (int i = 0; i < pool.GetLength (0); i++) {
                for (int j= 0; j < pool.GetLength (1); j++) {
                    switch (i) {
                        case (int) BulletType.Default:
                            pool[i,j] = CreateDefaultBullet (j);
                            break;
                    }
                }
            }
        }

        public Bullet GetBullet (BulletType type) {
            for (int i = 0; i < pool.GetLength (1); i++) {
                if (!pool[(int) type, i].GameObject.IsActive) {
                    return pool[(int) type , i];
                }
            }
            return null;
        }

        private Bullet CreateDefaultBullet (int i) {
            GameObject bullet = new GameObject ("DefaultBullet_" + i , Vector2.Zero);
            bullet.Tag = "Bullet";
            RigidBody rigidbody = bullet.AddComponent<RigidBody> ();
            rigidbody.IsGravityAffected = true;
            rigidbody.Type = (uint)RigidBodyType.bullet;
            rigidbody.AddCollisionType((uint)RigidBodyType.player);
            rigidbody.AddCollisionType((uint)RigidBodyType.tile);
            bullet.AddComponent (SpriteRenderer.Factory (bullet , "Bullet_1" , new Vector2 (0.5f , 0.5f) , DrawLayer.Playground));
            bullet.AddComponent (ColliderFactory.CreateCircleFor (bullet));
            return bullet.AddComponent<Bullet> (1 , BulletType.Default);
        }

        public override Component Clone (GameObject owner) {
            throw new System.NotImplementedException (); //eccezione componente non clonabile
        }
    }
}
