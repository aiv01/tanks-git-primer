using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Input.AddUserAxis("P_Horizontal", new UserAxis(new AxisMatch[] { new KeyAxisMatch(KeyCode.A, KeyCode.D) }));
            Input.AddUserAxis("P_Vertical", new UserAxis(new AxisMatch[] { new KeyAxisMatch(KeyCode.W, KeyCode.S) }));
            Input.AddUserButton("P_Shoot", new UserButton(new ButtonMatch[] { new KeyButtonMatch(KeyCode.Space) }));
            Game.Gravity = 13f;
            Game.Init("Tankz", 1280, 720, new PlayScene(), 720, 10);
            Game.Play();
        }
    }
}
