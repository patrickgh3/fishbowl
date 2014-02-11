using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;

namespace Fishbowl
{
    /// <summary>
    /// Manages dragging and dropping bubbles.
    /// </summary>
    class DragController
    {
        private Bubble currentBubble;
        private bool pressed = false;
        private Point lastposition;

        public DragController()
        {
            lastposition = new Point();
        }

        public void Pressed(Point p, BubbleContainer bc)
        {
            pressed = true;
            lastposition = p;
            currentBubble = bc.catchBubble(p);
            if (currentBubble != null) currentBubble.setDragged(true);
        }

        public void Moved(Point p)
        {
            if (!pressed || currentBubble == null) return;
            Point delta = new Point(p.X - lastposition.X, p.Y - lastposition.Y);
            currentBubble.moveBy(delta.X, delta.Y);
            currentBubble.setVelocity(delta.X * 0.2, delta.Y * 0.2);
            lastposition = p;
        }

        public void Released()
        {
            pressed = false;
            if (currentBubble != null)
            {
                currentBubble.setDragged(false);
            }
            currentBubble = null;
        }
    }
}
