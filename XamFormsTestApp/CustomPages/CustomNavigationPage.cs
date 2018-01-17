using Xamarin.Forms;
using XamFormsTestApp.Data.Services.Common;

namespace XamFormsTestApp.CustomPages
{
    public class CustomNavigationPage : NavigationPage
    {

        public CustomNavigationPage(Page content)
            : base(content)
        {
            Init();
        }

        private void Init()
        {
            //this.Pushed += (object sender, NavigationEventArgs e) =>
            //{
            //    //Handle pushing a new screen. 

            //};

            Popped += (sender, e) =>
            {
                var navpage = e.Page as IPageLifetime;
                if (navpage != null)
                {
                    // Unregister vm of page, message listener etc
                    navpage.CleanupPage();
                }
                e.Page.BindingContext = null;
            };
        }
    }
}
