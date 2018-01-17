using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms.Xaml;
using XamFormsTestApp.Data.Model;
using XamFormsTestApp.Data.Services.Common;
using XamFormsTestApp.Data.ViewModel;

namespace XamFormsTestApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : IPageLifetime
    {
        public DetailsPage(Order ord)
        {
            InitializeComponent();
            
            ((DetailsViewModel)BindingContext).RefreshCommand.Execute(ord);
        }

        public void CleanupPage()
        {
            SimpleIoc.Default.Unregister<DetailsViewModel>();
        }
    }
}
