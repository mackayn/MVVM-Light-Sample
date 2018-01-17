using System.Diagnostics;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using XamFormsTestApp.CustomPages;
using XamFormsTestApp.Data.ViewModel;
using XamFormsTestApp.Helpers;
using XamFormsTestApp.View;
using Xamarin.Forms;
using XamFormsTestApp.Data.Services.Common;

namespace XamFormsTestApp
{
    public partial class App : Application
    {
        private static ViewModelLocator _locator;

        public App()
        {
            InitializeComponent();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Handle app resuming from 1st page
            if (SimpleIoc.Default.IsRegistered<INavService>())
            {
                //MainPage = new CustomNavigationPage(new StartPage());
                MainPage = new CustomNavigationPage(new StartPage());
                return;
            }

            // Setup Nav service
            var nav = new NavigationService();
            nav.Configure(ViewModelLocator.PageKeyOrdDetails, typeof(DetailsPage));
            SimpleIoc.Default.Register<INavService>(() => nav);

            // Setup dialog service
            var dialog = new DialogService();
            SimpleIoc.Default.Register<IDialogService>(() => dialog);

            MainPage = new CustomNavigationPage(new StartPage());
        }

        public static ViewModelLocator Locator
        {
            get { return _locator ?? new ViewModelLocator(); }
        }

        protected override void OnStart()
        {
            base.OnStart();
            Debug.WriteLine("OnStart");
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            Debug.WriteLine("OnSleep");
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Lets start from the beginning again
            var nav = ServiceLocator.Current.GetInstance<INavService>();
            nav.Home();
        }
    }
}
