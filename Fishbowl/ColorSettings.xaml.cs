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

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Fishbowl
{
    public sealed partial class ColorSettings : SettingsFlyout
    {
        private bool pressed;

        public ColorSettings()
        {
            this.InitializeComponent();
        }

        public void SetControlsToDefaults()
        {
            // TODO: this
        }

        private void ColorSelectorHueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            // TODO: this
        }

        private void SettingsFlyout_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!pressed) return;
            Point p = e.GetCurrentPoint(ColorSelectorCanvas).Position;
            Canvas.SetLeft(ColorSelectorEllipse, FishUtil.ClampDouble(p.X, 0, 256) - ColorSelectorEllipse.Width / 2);
            Canvas.SetTop(ColorSelectorEllipse, FishUtil.ClampDouble(p.Y, 0, 256) - ColorSelectorEllipse.Height / 2);
        }

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
