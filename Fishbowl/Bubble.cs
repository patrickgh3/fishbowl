using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Fishbowl
{
    /// <summary>
    /// Stores the info for a bubble.
    /// </summary>
    class Bubble
    {
        private Shape shape;
        private Point position;
        private Point velocity;

        private struct Point
        {
            public double x;
            public double y;
            public Point(double x = 0, double y = 0)
            {
                this.x = x;
                this.y = y;
            }
        }

        public Bubble()
        {
            position = new Point();
            velocity = new Point(0.1, 0.1);
            shape = new Ellipse()
            {
                Width = 50,
                Height = 50,
                Fill = new SolidColorBrush(Colors.White)
            };

            BubbleContainer.canvas.Children.Add(shape);
            tick();
        }

        public void tick()
        {
            position.x += velocity.x;
            position.y += velocity.y;
            Canvas.SetLeft(shape, position.x);
            Canvas.SetTop(shape, position.y);
        }
    }
}
