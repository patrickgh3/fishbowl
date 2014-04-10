using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.Diagnostics;

namespace Fishbowl.BubbleAttributes
{
    /// <summary>
    /// Manages a bubble's content. (for now, only text)
    /// </summary>
    class BubbleContent
    {
        private Bubble parent;
        private TextBlock textblock;
        private Border textcontainer;
        bool flashing = false;
        bool flashState = false;
        private static int flashTime = 400;
        private static int flashCount = 0;

        public BubbleContent(Bubble parent, string text = "")
        {
            this.parent = parent;
            double radius = parent.getRadius();
            textblock = new TextBlock()
            {
                Text = text,
                Foreground = new SolidColorBrush(ColorSettings.TextColor),
                TextAlignment = Windows.UI.Xaml.TextAlignment.Center,
                FontSize = Preferences.FontSize,
                FontFamily = Preferences.FontFamily,
                Width = radius * 2,
                MaxHeight = radius * 2,
                TextWrapping = TextWrapping.WrapWholeWords,
                TextTrimming = TextTrimming.CharacterEllipsis,
                VerticalAlignment = VerticalAlignment.Center,
            };
            textcontainer = new Border()
            {
                Height = radius * 2,
                Child = textblock,
            };
            BubbleContainer.canvas.Children.Add(textcontainer);
            Canvas.SetZIndex(textcontainer, Int16.MaxValue);
        }

        public void tick()
        {
            if (!flashing) return;

            flashCount++;
            if (flashCount == flashTime)
            {
                flashCount = 0;
                flashState = !flashState;
                if (flashState) addPipe();
                else if (!flashState) removePipe();
            }
            UpdateAppearance();
        }

        public void UpdateAppearance()
        {
            ((SolidColorBrush)textblock.Foreground).Color = ColorSettings.TextColor;
            textblock.FontFamily = Preferences.FontFamily;
            textblock.FontSize = Preferences.FontSize;
            textblock.Width = parent.getRadius() * 2;
            textblock.MaxHeight = parent.getRadius() * 2;
            textcontainer.Height = parent.getRadius() * 2;
            UpdateCanvasPos();
        }

        public void UpdateCanvasPos()
        {
            Canvas.SetLeft(textcontainer, parent.getPosition().x - parent.getRadius());
            Canvas.SetTop(textcontainer, parent.getPosition().y - parent.getRadius());
        }

        public void setText(String s)
        {
            textblock.Text = s;
            if (flashState) addPipe();
        }

        private void addPipe()
        {
            textblock.Text += "|";
        }

        private void removePipe()
        {
            textblock.Text = textblock.Text.Substring(0, textblock.Text.Length - 1);
        }

        public void setFlashing(bool b)
        {
            if (flashState && !b)
            {
                removePipe();
            }

            flashing = b;
            if (flashing)
            {
                flashCount = flashTime - 1;
                flashState = false;
            }
        }
    }
}
