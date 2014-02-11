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
        DragController dragController;

        public MainPage()
        {
            this.InitializeComponent();
            BubbleContainer.canvas = BubbleCanvas;
            currentContainer = new BubbleContainer();
            dragController = new DragController();

            // Create a periodic work schedule - every 1 millisecond update all the bubbles.
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

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            dragController.Pressed(e.GetCurrentPoint(BubbleCanvas).Position, currentContainer);
        }

        private void Grid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            dragController.Moved(e.GetCurrentPoint(BubbleCanvas).Position);
        }

        private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            dragController.Released();
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            dragController.Released();
        }
    }
}
