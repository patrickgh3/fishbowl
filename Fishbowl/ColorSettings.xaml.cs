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
        }

        public void SetControlsToDefaults()
        {
            sat = 1;
            val = 1;
            ColorSelectorHueSlider.Value = 0;
        }

        // color related

        private void UpdateColor()
        {
            byte r, g, b;
            FishUtil.HsvToRgb(ColorSelectorHueSlider.Value, sat, val, out r, out g, out b);
            BackgroundColor.R = r;
            BackgroundColor.G = g;
            BackgroundColor.B = b;
            ((SolidColorBrush)BubbleContainer.canvas.Background).Color = BackgroundColor;

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

        // non-color releated

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
