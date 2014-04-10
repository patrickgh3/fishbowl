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
        static BubbleContainer currentContainer;
        static DragController dragController;
        static CreationController creationController;

        public MainPage()
        {
            this.InitializeComponent();
            BubbleContainer.canvas = BubbleCanvas;
            currentContainer = new BubbleContainer();
            dragController = new DragController();
            creationController = new CreationController(CreationTextBox);
            Window.Current.CoreWindow.PointerReleased += CoreWindow_PointerReleased;
            Window.Current.CoreWindow.SizeChanged +=CoreWindow_SizeChanged;

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

        internal static BubbleContainer getCurrentContainer()
        {
            return currentContainer;
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (creationController.isActive())
            {
                creationController.relinquishBubble();
            }

            if (!creationController.isActive())
            {
                dragController.Pressed(e.GetCurrentPoint(BubbleCanvas).Position, currentContainer);
            }
            /*Point p = e.GetCurrentPoint(BubbleCanvas).Position;
            if (currentContainer.catchBubble(p) != null)
            {
                dragController.Pressed(p, currentContainer);
            }
            else
            {

            }*/
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

        private void CoreWindow_PointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            App.colorsettings.MainPagePointerReleased();
        }

        private void CoreWindow_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            if (Preferences.BubbleAutoSize) Preferences.getInstance().AutoSizeBubbleRadius();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Point p = e.GetPosition(BubbleCanvas);
            if (currentContainer.catchBubble(p) == null && !creationController.isActive())
            {
                creationController.createBubble(p, currentContainer);
            }
        }

        private void CreationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            creationController.UpdateBubbleText();
        }

        private void CreationTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter
                && creationController.isActive()) creationController.relinquishBubble();
        }
    }
}
