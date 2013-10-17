using System;
using System.ServiceModel.Security;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Popups;
using _02PittyLove.WinRT2.Model;
using _02PittyLove.WinRT2.Services;

namespace _02PittyLove.WinRT2.ViewModels
{
    public class EditFlyoutViewModel : BindableBase, IFlyoutViewModel
    {
        #region Fields
        
        private Action _successAction;
        private PitbullViewModel _model;
        private Action _closeFlyout;
        private Action _goBack;
        private string _originalName;
        private string _originalDescription;
        private readonly IPittyLoveService _pittyLoveService;
        public string Name { get; set; }
        public string Description { get; set; }
        
        #endregion

        #region Properties

        public PitbullViewModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public Action CloseFlyout
        {
            get { return _closeFlyout; }
            set { SetProperty(ref _closeFlyout, value); }
        }

        public Action GoBack
        {
            get { return _goBack; }
            set { SetProperty(ref _goBack, value); }
        }

        public void Open(object parameter, Action successAction)
        {
            _successAction = successAction;
            var model = parameter as PitbullViewModel;
            if (model != null)
            {
                _originalDescription = model.Description;
                _originalName = model.Name;
                Model = model;
            }
        }

        #endregion

        #region Constructor

        public EditFlyoutViewModel(IPittyLoveService pittyLoveService)
        {
            _pittyLoveService = pittyLoveService;
            SaveCommand = new DelegateCommand(SaveCommandExecuted, CanSaveExecute);
            CancelCommand = new DelegateCommand(CancelCommandExecuted, CanCancelExecute);
        }

        #endregion

        #region Methods

        private void RevertChanges()
        {
            Model.Name = _originalName;
            Model.Description = _originalDescription;
        }

        private bool CanSaveExecute()
        {
            return Model != null;
        }

        private void SaveCommandExecuted()
        {
            //Map view model fields to model
            var pitbull = new Pitbull {Description = Model.Description, Name = Model.Name, Id = Model.UniqueId, ImageUrl = Model.ImageUrl};

            try
            {
                var savedItem = _pittyLoveService.Save(pitbull);
            }
            catch (SecurityAccessDeniedException)
            {
                RevertChanges();
                var messageDialog = new MessageDialog("No man, this is not for you! Now turn around slowly and go back from where you came!");
                messageDialog.Commands.Add(new UICommand("OK"));
                messageDialog.ShowAsync();
            }
           
            CloseFlyout();
            _successAction();
        }

        private bool CanCancelExecute()
        {
            return Model != null;
        }

        private void CancelCommandExecuted()
        {
            RevertChanges();
            CloseFlyout();
        }

        #endregion
    }
}
