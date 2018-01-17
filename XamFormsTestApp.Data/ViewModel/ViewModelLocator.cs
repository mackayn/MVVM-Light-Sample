using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using XamFormsTestApp.Data.Services;

namespace XamFormsTestApp.Data.ViewModel
{
    public class ViewModelLocator
    {
        public const string PageKeyOrdDetails = "DetailsPage";

        static ViewModelLocator()
        {

            if (ViewModelBase.IsInDesignModeStatic)
            {
                //NOTE: No design time currently in Xamarin forms
            }
            else
            {
                SimpleIoc.Default.Register<IOrderService, OrderService>();
            }

            SimpleIoc.Default.Register<StartViewModel>();

        }

        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
       
        public StartViewModel Start
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StartViewModel>();
            }
        }

        public DetailsViewModel Details
        {
            get
            {
                if (!SimpleIoc.Default.IsRegistered<DetailsViewModel>())
                    SimpleIoc.Default.Register<DetailsViewModel>();
                return ServiceLocator.Current.GetInstance<DetailsViewModel>();
            }
        }
    }
}
