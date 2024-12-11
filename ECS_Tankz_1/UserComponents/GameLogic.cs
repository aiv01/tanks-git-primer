using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public class GameLogic : UserComponent
    {
        private const float turnDuration = 15f;
        private const float waitingNextTurnDuration = 1;

        //Reference
        private PlayerController p1;
        private PlayerController p2;

        //Working variables
        private bool p1Turn;
        private bool waitingNextTurn;
        private float currentTurnDuration;
        private float currentWaitingNextTurn;
        private PlayerController currentPlayer;

        private UIController UIController;


        public GameLogic(GameObject owner) : base(owner)
        {

        }

        public override void Awake()
        {
            p1 = GameObject.Find("Player_1").GetComponent<PlayerController>();
            p2 = GameObject.Find("Player_2").GetComponent<PlayerController>();

            UIController = GameObject.Find("UIController").GetComponent<UIController>();
        }

        public void OnBulletSpawned(Bullet bullet)
        {
            CameraManager.target = bullet.GameObject;
        }

        public void OnBulletDestroyed()
        {
            StopTurn();
        }

        public override void Start()
        {
            StartTurn();
        }

        public override void Update()
        {
            if (waitingNextTurn)
            {
                currentWaitingNextTurn -= Game.DeltaTime;
                if (currentWaitingNextTurn <= 0)
                {
                    waitingNextTurn = false;
                    StartTurn();
                }
            }
            else
            {
                currentTurnDuration -= Game.DeltaTime;
                UIController.SetTurnTimer((int)currentTurnDuration);
                if (currentTurnDuration <= 0)
                {
                    StopTurn();
                }
            }
        }

        private void StartTurn()
        {
            p1Turn = !p1Turn;
            currentPlayer = p1Turn ? p1 : p2;
            currentPlayer.StartTurn();
            CameraManager.target = currentPlayer.GameObject;
            currentTurnDuration = turnDuration;
            UIController.SetTurnFeedback(currentPlayer.PlayerID);
        }

        private void StopTurn()
        {
            currentPlayer.StopTurn();
            ChangeTurn();
        }

        private void ChangeTurn()
        {
            waitingNextTurn = true;
            currentWaitingNextTurn = waitingNextTurnDuration;
        }

        public override Component Clone(GameObject owner)
        {
            return new GameLogic(owner);
        }
    }
}
