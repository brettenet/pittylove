using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml.Navigation;

namespace _02PittyLove.WinRT2.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class EditFlyout : FlyoutView
    {
        public EditFlyout()
            : base(StandardFlyoutSize.Narrow)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
