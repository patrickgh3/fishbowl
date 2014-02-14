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

        public Preferences()
        {
            this.InitializeComponent();
        }

        public void SetControlsToDefaults()
        {
            PushStrengthSlider.Value = 0.45;
            FontFamilyComboBox.SelectedIndex = 0;
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

        }

        private void BubbleSizeToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {

        }
    }
}
