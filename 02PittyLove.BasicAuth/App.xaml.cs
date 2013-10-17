using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using _02PittyLove.WinRT2.Services;
using _02PittyLove.WinRT2.ViewModels;
using _02PittyLove.WinRT2.Views;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace _02PittyLove.WinRT2
{
    sealed partial class App : MvvmAppBase
    {
        private UnityContainer _container;

        public IUnityContainer Container
        {
            get { return _container; }
        }

        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Required override. Generally you do your initial navigation to launch page, or 
        /// to the page appropriate based on a search, sharing, or secondary tile launch of the app
        /// </summary>
        /// <param name="args">The launch arguments passed to the application</param>
        protected override void OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Main", null);
        }

        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        /// <summary>
        /// This is the place you initialize your services and set default factory or default resolver for the view model locator
        /// </summary>
        /// <param name="args">The same launch arguments passed when the app starts.</param>
        protected override void OnInitialize(IActivatedEventArgs args)
        {
            _container = new UnityContainer();

            _container.RegisterInstance<INavigationService>(NavigationService);
            _container.RegisterInstance<IPittyLoveService>(new PittyLoveService());
            _container.RegisterInstance<ISessionStateService>(SessionStateService);
            _container.RegisterInstance<IFlyoutService>(FlyoutService);
            ViewModelLocator.Register(typeof(MainPage).ToString(), () => _container.Resolve<MainPageViewModel>());
        }
    }
}
