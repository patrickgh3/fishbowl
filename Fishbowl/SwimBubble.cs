using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fishbowl
{
    class SwimBubble : Bubble
    {
        private static double swimVelocity = 0.4;
        private static double swimSlow = 0.998;
        private static double swimProb = 0.001;

        public SwimBubble(String t = "") : base(t)
        {
        }

        override public void tick()
        {
            velocity.x *= swimSlow;
            velocity.y *= swimSlow;
            if (FishUtil.random.NextDouble() < swimProb)
            {
                double angle = FishUtil.random.NextDouble() * Math.PI * 2;
                velocity.x += swimVelocity * Math.Cos(angle);
                velocity.y += swimVelocity * Math.Sin(angle);
            }
            base.tick();
        }
    }
}
