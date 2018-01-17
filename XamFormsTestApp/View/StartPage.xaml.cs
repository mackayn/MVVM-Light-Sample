using Xamarin.Forms.Xaml;
using XamFormsTestApp.Data.Services.Common;
using XamFormsTestApp.Data.ViewModel;

namespace XamFormsTestApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : IPageLifetime
    {

        public StartPage()
        {
            InitializeComponent();
            ((StartViewModel)this.BindingContext).RefreshCommand.Execute(null);
        }

        public void CleanupPage()
        {
            //ListviewOrders.Behaviors.Clear();
        }
    }
}