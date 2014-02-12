using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.Foundation;
using System.Diagnostics;

namespace Fishbowl
{
    /// <summary>
    /// Stores the info for a bubble.
    /// </summary>
    class Bubble
    {
        protected static double stdRadius = 100;
        protected static double pushStrength = 0.005;

        protected Shape shape;
        protected TextBlock text;
        protected Border textcontainer;
        protected Point position;
        protected Point velocity;
        protected double radius;
        protected bool beingdragged;

        public Bubble(String t = "")
        {
            position = new Point();
            velocity = new Point(0.1, 0.1);
            radius = stdRadius;
            shape = new Ellipse()
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = new SolidColorBrush(Colors.White)
            };
            text = new TextBlock()
            {
                Text = t,
                Foreground = new SolidColorBrush(Colors.Black),
                TextAlignment = Windows.UI.Xaml.TextAlignment.Center,
                FontSize = 32,
                Width = radius * 2,
                MaxHeight = radius * 2,
                TextWrapping = TextWrapping.WrapWholeWords,
                TextTrimming = TextTrimming.CharacterEllipsis,
                VerticalAlignment = VerticalAlignment.Center,
            };
            textcontainer = new Border()
            {
                Height = radius * 2,
                Child = text,
            };
            BubbleContainer.canvas.Children.Add(shape);
            BubbleContainer.canvas.Children.Add(textcontainer);
            tick();
        }

        public virtual void tick()
        {
            if (beingdragged) return;

            position.x += velocity.x;
            position.y += velocity.y;

            Rect bounds = Window.Current.Bounds;
            if (position.x - radius < 0 || position.x + radius > bounds.Width) velocity.x *= -1;
            if (position.y - radius < 0 || position.y + radius > bounds.Height) velocity.y *= -1;
            while (position.x - radius < 0) position.x++;
            while (position.y - radius < 0) position.y++;
            while (position.x + radius > bounds.Width) position.x--;
            while (position.y + radius > bounds.Height) position.y--;
            updateCanvasPos();
        }

        public void collide(Bubble other)
        {
            Point otherpos = other.getPosition();
            double otherrad = other.getRadius();
            double distancesquared = (otherpos.x - position.x) * (otherpos.x - position.x)
                                    + (otherpos.y - position.y) * (otherpos.y - position.y);
            double radiussquared = (otherrad + radius) * (otherrad + radius);
            if (distancesquared < radiussquared)
            {
                double magnitude = (1 - distancesquared / radiussquared) * pushStrength;
                double pushangle = Math.Atan((otherpos.y - position.y) / (otherpos.x - position.x)) + Math.PI;
                if (otherpos.x < position.x) pushangle += Math.PI;

                velocity.x += magnitude * Math.Cos(pushangle);
                velocity.y += magnitude * Math.Sin(pushangle);
            }
        }

        private void updateCanvasPos()
        {
            Canvas.SetLeft(shape, position.x - radius);
            Canvas.SetTop(shape, position.y - radius);
            Canvas.SetLeft(textcontainer, position.x - radius);
            Canvas.SetTop(textcontainer, position.y - radius);
        }

        public void setPosition(double x, double y)
        {
            position.x = x;
            position.y = y;
            updateCanvasPos();
        }

        public void moveBy(double dx, double dy)
        {
            position.x += dx;
            position.y += dy;
            updateCanvasPos();
        }

        public void setVelocity(double x, double y)
        {
            velocity.x = x;
            velocity.y = y;
        }

        public void setDragged(bool beingdragged)
        {
            this.beingdragged = beingdragged;
        }

        public Point getPosition()
        {
            return position;
        }

        public double getRadius()
        {
            return radius;
        }

        public struct Point
        {
            public double x;
            public double y;
            public Point(double x = 0, double y = 0)
            {
                this.x = x;
                this.y = y;
            }
        }
    }
}
