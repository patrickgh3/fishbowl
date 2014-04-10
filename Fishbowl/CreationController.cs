using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Fishbowl
{
    /// <summary>
    /// Manages the state of creating a bubble.
    /// </summary>
    class CreationController
    {
        bool active = false;
        Bubble bubble;
        TextBox textBox;

        public CreationController(TextBox tb)
        {
            this.textBox = tb;
        }

        public void createBubble(Point p, BubbleContainer bc)
        {
            
            bubble = bc.addBubble("type here!");
            bubble.setPosition(p.X, p.Y);
            bubble.setDragged(true);
            // todo: set bubble to blink
            textBox.Focus(FocusState.Keyboard);
            active = true;
        }

        public void relinquishBubble()
        {
            bubble.setDragged(false);
            // todo: maybe launch bubble, or another visual cue?\
            //textBox.Focus(FocusState.Unfocused);
            active = false;
            textBox.Text = "";
        }

        public void UpdateBubbleText()
        {
            if (active) bubble.setContentText(textBox.Text);
        }

        public bool isActive()
        {
            return active;
        }
    }
}
