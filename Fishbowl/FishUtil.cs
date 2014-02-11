using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Fishbowl
{
    // Utility function class.
    class FishUtil
    {
        public static Random random = new Random();

        public static bool containsPoint(Bubble bubble, Point point)
        {
            Bubble.Point bubblepos = bubble.getPosition();
            return (bubblepos.x - point.X) * (bubblepos.x - point.X) + (bubblepos.y - point.Y) * (bubblepos.y - point.Y)
                < bubble.getRadius() * bubble.getRadius();
        }
    }
}
