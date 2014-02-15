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
        public static bool TextAutoSize = true;
        public static double FontSize;
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
            BubbleSizeSlider.IsEnabled = false;
            TextSizeSlider.IsEnabled = false;
        }

        private void UpdateBubbleAppearance()
        {
            BubbleContainer container = MainPage.getCurrentContainer();
            if (container != null) container.UpdateBubbleAppearance();
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

            UpdateBubbleAppearance();
        }

        // bubble size

        private void BubbleSizeToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            BubbleAutoSize = !BubbleSizeToggleSwitch.IsOn;
            BubbleSizeSlider.IsEnabled = BubbleSizeToggleSwitch.IsOn;
            if (BubbleAutoSize) AutoSizeBubbleRadius();
        }

        private void BubbleSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            BubbleRadius = e.NewValue; // BubbleSizeSlider is null for some reason on startup at this point
            UpdateBubbleAppearance();
            if ((TextSizeSlider != null) && TextAutoSize) AutoSizeText();
        }

        public void AutoSizeBubbleRadius()
        {
            if (MainPage.getCurrentContainer() == null || MainPage.getCurrentContainer().getNumBubbles() == 0) return;
            Rect bounds = Window.Current.Bounds;
            double numbubbles = (double)MainPage.getCurrentContainer().getNumBubbles();
            double radius = Math.Sqrt((bounds.Width * bounds.Height) / (Math.PI * numbubbles)) / 2;
            radius = FishUtil.ClampDouble(radius, 20, Math.Sqrt((bounds.Width * bounds.Height) / Math.PI) / 4);
            BubbleSizeSlider.Value = radius;
        }

        // text size

        private void TextSizeToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            TextAutoSize = !TextSizeToggleSwitch.IsOn;
            TextSizeSlider.IsEnabled = TextSizeToggleSwitch.IsOn;
            if (TextAutoSize) AutoSizeText();
        }

        private void TextSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            FontSize = e.NewValue;
            System.Diagnostics.Debug.WriteLine(FontSize);
            UpdateBubbleAppearance();
        }

        public void AutoSizeText()
        {
            TextSizeSlider.Value = BubbleRadius * (32.0 / 100.0);
        }
    }
}
