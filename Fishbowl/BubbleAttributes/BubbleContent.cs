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

        public BubbleContent(Bubble parent, string text = "")
        {
            this.parent = parent;
            double radius = parent.getRadius();
            textblock = new TextBlock()
            {
                Text = text,
                Foreground = new SolidColorBrush(ColorSettings.TextColor),
                TextAlignment = Windows.UI.Xaml.TextAlignment.Center,
                FontSize = 32,
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

        public void UpdateAppearance()
        {
            // TODO: update TextBlock and Border sizes, font size, font family, text color, etc.
            // based on global state (e.g. custom bubble collor setting),
            // and parent state (e.g. what's the current radius at?).
            textblock.FontFamily = Preferences.FontFamily;
            ((SolidColorBrush)textblock.Foreground).Color = ColorSettings.TextColor;
            textblock.Width = parent.getRadius() * 2;
            textblock.MaxHeight = textcontainer.Height = parent.getRadius() * 2;
        }

        public void UpdateCanvasPos()
        {
            Canvas.SetLeft(textcontainer, parent.getPosition().x - parent.getRadius());
            Canvas.SetTop(textcontainer, parent.getPosition().y - parent.getRadius());
        }
    }
}
