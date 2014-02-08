using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.System.Threading;
using Windows.UI.Core;
using System.Diagnostics;

namespace Fishbowl
{
    /// <summary>
    /// Main application page.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        BubbleContainer currentContainer;

        public MainPage()
        {
            this.InitializeComponent();
            BubbleContainer.canvas = BubbleCanvas;
            currentContainer = new BubbleContainer();

            // Create a periodic work schedule - every 1 second update all the bubbles.
            // http://msdn.microsoft.com/en-US/library/windows/apps/jj248676
            TimeSpan period = TimeSpan.FromMilliseconds(1);
            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                // Update the UI thread by using the UI core dispatcher.
                Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {
                        currentContainer.tick();
                    });

            }, period);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void BubbleCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
        }

        private void BubbleCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

        }

        private void AddOkButton_Click(object sender, RoutedEventArgs e)
        {
            currentContainer.addBubble(AddTextBox.Text);
            AddTextBox.Text = "";
            AddFlyout.Hide();
        }

        private void AddTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter) AddOkButton_Click(null, null);
        }
    }
}
