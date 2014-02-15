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
    public sealed partial class Preferences : SettingsFlyout
    {
        public static FontFamily FontFamily = new FontFamily("Segoe UI");
        public static bool BubbleAutoSize = true;
        public static double BubbleRadius;
        private static Preferences instance;

        public static Preferences getInstance()
        {
            return instance;
        }

        public Preferences()
        {
            this.InitializeComponent();
            instance = this;
        }

        public void SetControlsToDefaults()
        {
            PushStrengthSlider.Value = 0.45;
            FontFamilyComboBox.SelectedIndex = 0;
            BubbleSizeSlider.Value = 100;
            BubbleSizeSlider.IsEnabled = false;
        }

        // push strength

        private void PushStrengthSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            double slidervalue = (1 - PushStrengthSlider.Value) * -5;
            double pushstrength = Math.Pow(10, slidervalue);
            if (PushStrengthSlider.Value == 0) pushstrength = 0;
            Bubble.pushStrength = pushstrength;
        }

        // font family

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectedIndex == 0) FontFamily = new FontFamily("Segoe UI");
            else if (FontFamilyComboBox.SelectedIndex == 1) FontFamily = new FontFamily("Times New Roman");
            else if (FontFamilyComboBox.SelectedIndex == 2) FontFamily = new FontFamily("Consolas");

            MainPage.getCurrentContainer().UpdateBubbleAppearance();
        }

        // bubble size

        private void BubbleSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            BubbleRadius = e.NewValue; // BubbleSizeSlider is null for some reason on startup at this point
            UpdateBubbles();
        }

        private void BubbleSizeToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            BubbleAutoSize = !BubbleSizeToggleSwitch.IsOn;
            BubbleSizeSlider.IsEnabled = BubbleSizeToggleSwitch.IsOn;
            if (BubbleAutoSize) AutoSizeBubbleRadius();
        }

        public void AutoSizeBubbleRadius()
        {
            if (MainPage.getCurrentContainer() == null || MainPage.getCurrentContainer().getNumBubbles() == 0) return;
            Rect bounds = Window.Current.Bounds;
            double numbubbles = (double)MainPage.getCurrentContainer().getNumBubbles();
            BubbleSizeSlider.Value = Math.Sqrt((bounds.Width * bounds.Height) / (Math.PI * numbubbles)) / 2.5;
        }

        private void UpdateBubbles()
        {
            if (MainPage.getCurrentContainer() != null) MainPage.getCurrentContainer().UpdateBubbleAppearance();
        }
    }
}
