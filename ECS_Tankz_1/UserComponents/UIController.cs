using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ECS_Tankz_1
{
    public class UIController : UserComponent
    {
        private const string player1Turn = "Player 1 Turn";
        private const string player2Turn = "Player 2 Turn";

        // Refs
        private SpriteRenderer player1Bar;
        private SpriteRenderer player2Bar;

        private TextBox turnFeedback;
        private TextBox turnTimer;

        public UIController(GameObject owner) : base(owner)
        {

        }

        public void SetPlayerHealth(int id, float perc)
        {
            Transform barToScale = null;

            switch(id)
            {
                case 1:
                    barToScale = player1Bar.Transform;
                    break;
                case 2:
                    barToScale = player2Bar.Transform;
                    break;
            }

            barToScale.Scale = new Vector2(perc, barToScale.Scale.Y);
        }

        public void SetTurnFeedback(int id)
        {
            switch (id)
            {
                case 1:
                    turnFeedback.SetText(player1Turn);
                    break;
                case 2:
                    turnFeedback.SetText(player2Turn);
                    break;
            }
        }

        public void SetTurnTimer(int time)
        {
            turnTimer.SetText(time.ToString());
        }

        public override void Awake()
        {
            player1Bar = GameObject.Find("Player1Bar").GetComponent<SpriteRenderer>();
            player2Bar = GameObject.Find("Player2Bar").GetComponent<SpriteRenderer>();

            turnFeedback = GameObject.Find("TurnFeedback").GetComponent<TextBox>();
            turnTimer = GameObject.Find("TurnTimer").GetComponent<TextBox>();
        }

        public override Component Clone(GameObject owner)
        {
            throw new NotImplementedException();
        }
    }
}
