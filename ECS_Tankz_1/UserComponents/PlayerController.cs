using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public class PlayerController : UserComponent
    {
        private int myID;
        public int PlayerID
        {
            get { return myID; }
        }

        private FSM myFSM;
        private float hp;
        private float currentHP;
        public float CurrentHP
        {
            get { return currentHP; }
            set
            {
                currentHP = value;
                if (currentHP < 0) currentHP = 0;
                if (currentHP > hp) currentHP = hp;
                UIController.SetPlayerHealth(myID, currentHP / hp);
            }
        }


        //Reference
        private UIController UIController;

        public PlayerController(GameObject owner, int myID, float hp) : base(owner)
        {
            this.myID = myID;
            this.hp = hp;
            currentHP = hp;
        }

        public override void Awake()
        {
            myFSM = GameObject.GetComponent<FSM>();
            UIController = GameObject.Find("UIController").GetComponent<UIController>();
        }

        public void StartTurn()
        {
            myFSM.Switch((int)FSMState.MoveState);
        }

        public void StopTurn()
        {
            myFSM.Switch((int)FSMState.Idle);
        }

        public override void OnCollide(Collision CollisionInfo)
        {
            if (CollisionInfo.Collider.GameObject.Tag == "Bullet")
            {
                Bullet bullet = CollisionInfo.Collider.GetComponent<Bullet>();
                if (bullet.PlayerID != PlayerID)
                {
                    CurrentHP -= bullet.Damage;
                    if (currentHP <= 0)
                    {
                        //notificare che il player è nonmorto
                    }
                    bullet.DestroyBullet();
                }
            }
        }

        public override Component Clone(GameObject owner)
        {
            throw new System.NotImplementedException();
        }
    }
}
