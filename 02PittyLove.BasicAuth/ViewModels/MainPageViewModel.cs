using System.Collections.ObjectModel;
using System.ServiceModel.Security;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Popups;
using _02PittyLove.WinRT2.Services;

namespace _02PittyLove.WinRT2.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        #region Fields

        private PitbullViewModel _selectedItem;
        private bool _isBottomBarOpen;
        private readonly IFlyoutService _flyoutService;
        private readonly IPittyLoveService _pittyLoveService;

        #endregion

        #region Properties

        public DelegateCommand EditCommand
        {
            get; 
            private set;
        }

        public ObservableCollection<PitbullViewModel> Items
        {
            get; 
            set;
        }

        public PitbullViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                IsBottomBarOpen = value != null;
            }
        }

        public bool IsBottomBarOpen
        {
            get { return _isBottomBarOpen; }
            set
            {
                _isBottomBarOpen = value;
                OnPropertyChanged("IsBottomBarOpen");
            }
        }

        #endregion

        #region Constructor

        public MainPageViewModel(IFlyoutService flyoutService, IPittyLoveService pittyLoveService)
        {
            _flyoutService = flyoutService;
            _pittyLoveService = pittyLoveService;
            IsBottomBarOpen = false;
            Items = new ObservableCollection<PitbullViewModel>();
            EditCommand = new DelegateCommand(EditCommandExecuted, CanEditCommandExecute);

            try
            {
                var pitbulls = _pittyLoveService.GetDogs();
                foreach (var pitbull in pitbulls)
                {
                    Items.Add(new PitbullViewModel(pitbull));
                }
            }
            catch (SecurityAccessDeniedException)
            {
                var messageDialog = new MessageDialog("No man, this is not for you! Now turn around slowly and go back from where you came!");
                messageDialog.Commands.Add(new UICommand("OK"));
                messageDialog.ShowAsync();
            }
        }

        private bool CanEditCommandExecute()
        {
            return SelectedItem != null;
        }

        private void EditCommandExecuted()
        {
            _flyoutService.ShowFlyout("Edit", SelectedItem, () => {});
        }

        #endregion
    }
}
