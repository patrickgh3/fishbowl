using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.Foundation;

namespace Fishbowl
{
    /// <summary>
    /// Manages a group of bubbles.
    /// </summary>
    class BubbleContainer
    {
        private List<Bubble> bubbles;
        public static Canvas canvas;

        public BubbleContainer()
        {
            bubbles = new List<Bubble>();
        }

        public void tick()
        {
            for (int i = 0; i < bubbles.Count; i++)
            {
                for (int j = 0; j < bubbles.Count; j++)
                {
                    if (i != j) bubbles[i].collide(bubbles[j]);
                }
            }
            for (int i = 0; i < bubbles.Count; i++)
            {
                bubbles[i].tick();
            }
        }

        public void addRandomBubble()
        {
            Bubble b = new SwimBubble();
            bubbles.Add(b);
            Rect bounds = Window.Current.Bounds;
            b.setPosition(FishUtil.random.NextDouble() * bounds.Width, FishUtil.random.NextDouble() * bounds.Height);
        }
    }
}
