using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Fishbowl
{
    public sealed partial class ColorSettings : SettingsFlyout
    {
        public static Color BackgroundColor;
        public static Color BubbleColor;
        public static Color TextColor;
        unsafe private Color* selectedcolor;
        private bool pressed;
        private double sat;
        private double val;

        public ColorSettings()
        {
            this.InitializeComponent();
            BackgroundColor = new Color()
            {
                R = 0xdd,
                G = 0xdd,
                B = 0xdd,
                A = 0xff
            };
            BubbleColor = new Color()
            {
                R = 0xff,
                G = 0xff,
                B = 0xff,
                A = 0xff
            };
            TextColor = new Color()
            {
                R = 0x00,
                G = 0x00,
                B = 0x00,
                A = 0xff
            };
        }

        public void SetControlsToDefaults()
        {
            sat = 1;
            val = 1;
            ColorSelectorHueSlider.Value = 0;
        }

        // color-related calculations

        private unsafe void UpdateColor()
        {
            byte r, g, b;
            FishUtil.HsvToRgb(ColorSelectorHueSlider.Value, sat, val, out r, out g, out b);
            selectedcolor->R = r;
            selectedcolor->G = g;
            selectedcolor->B = b;
            if (*selectedcolor == BackgroundColor)
            {
                ((SolidColorBrush)BubbleContainer.canvas.Background).Color = BackgroundColor;
            }
            else if (*selectedcolor == BubbleColor || *selectedcolor == TextColor)
            {
                MainPage.getCurrentContainer().UpdateBubbleAppearance();
            }

            FishUtil.HsvToRgb(ColorSelectorHueSlider.Value, 1, 1, out r, out g, out b);
            ((SolidColorBrush)ColorSelectorHueRect.Fill).Color = new Color() { R = r, G = g, B = b, A = 0xff };
        }

        private void ColorSelectorHueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            UpdateColor();
        }

        private void SettingsFlyout_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!pressed) return;
            Point p = e.GetCurrentPoint(ColorSelectorCanvas).Position;
            p = new Point(FishUtil.ClampDouble(p.X, 0, 256), FishUtil.ClampDouble(p.Y, 0, 256));
            Canvas.SetLeft(ColorSelectorEllipse, p.X - ColorSelectorEllipse.Width / 2);
            Canvas.SetTop(ColorSelectorEllipse, p.Y - ColorSelectorEllipse.Height / 2);

            FishUtil.PosToHsv((int)p.X, (int)p.Y, out sat, out val);
            UpdateColor();
        }

        // switching colors

        private unsafe void BackgroundRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            fixed(Color* colorptr = &BackgroundColor)
            {
                selectedcolor = colorptr;
            }
        }

        private unsafe void BubbleRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            fixed (Color* colorptr = &BubbleColor)
            {
                selectedcolor = colorptr;
            }
        }

        private unsafe void TextRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            fixed (Color* colorptr = &TextColor)
            {
                selectedcolor = colorptr;
            }
        }

        // pointer tracking

        private void Rectangle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pressed = true;
            SettingsFlyout_PointerMoved(sender, e);
        }

        private void SettingsFlyout_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPointProperties properties = e.GetCurrentPoint(ColorSelectorCanvas).Properties;
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed || properties.IsMiddleButtonPressed) pressed = true;
        }

        private void Rectangle_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            pressed = false;
        }

        private void SettingsFlyout_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            pressed = false;
        }

        private void SettingsFlyout_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            pressed = false;
        }
    }
}
