using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Fishbowl.BubbleAttributes
{
    /// <summary>
    /// Manages a bubble's body. (probably will only ever be a circle)
    /// </summary>
    class BubbleShape
    {
        private Bubble parent;
        private Shape shape;

        public BubbleShape(Bubble parent)
        {
            this.parent = parent;
            double radius = parent.getRadius();
            shape = new Ellipse()
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = new SolidColorBrush(ColorSettings.BubbleColor)
            };
            BubbleContainer.canvas.Children.Add(shape);
        }

        public void UpdateAppearance()
        {
            // TODO: update based on parent state (e.g. parent radius)
            // and global state (e.g. custom bubble color)
            ((SolidColorBrush)shape.Fill).Color = ColorSettings.BubbleColor;
            shape.Width = parent.getRadius() * 2;
            shape.Height = parent.getRadius() * 2;
            UpdateCanvasPos();
        }

        public void UpdateCanvasPos()
        {
            Canvas.SetLeft(shape, parent.getPosition().x - shape.Width / 2);
            Canvas.SetTop(shape, parent.getPosition().y - shape.Height / 2);
        }
    }
}
