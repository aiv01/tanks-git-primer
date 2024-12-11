using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public class RandomTimer
    {
        private int timeMin;
        private int timeMax;
        private float timer;
        private Random random;

        public RandomTimer(int minT,int maxT)
        {
            timeMin = minT;
            timeMax = maxT;
            random = new Random();

            Reset();
        }

        public void Tick()
        {
            timer -= Game.DeltaTime;
            if(timer <= 0.0f)
            {
                timer = 0.0f;
            }
        }

        public void Reset()
        {
            timer = random.Next(timeMin, timeMax);
        }

        public bool IsOver()
        {
            return timer <= 0.0f;
        }
    }
}
