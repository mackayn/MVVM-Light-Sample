using System.Threading.Tasks;

namespace XamFormsTestApp.Data.Services.Common
{
    /// <summary>
    /// An interface defining how navigation between pages should
    ///             be performed in various frameworks such as Windows,
    ///             Windows Phone, Android, iOS etc.
    /// 
    /// </summary>
    public interface INavService
    {
        /// <summary>
        /// The key corresponding to the currently displayed page.
        /// 
        /// </summary>
        string CurrentPageKey { get; }

        /// <summary>
        /// The key corresponding to the currently displayed modal page.
        /// </summary>
        string CurrentModalPageKey { get; }

        /// <summary>
        /// The key corresponding to the currently displayed modal page.
        /// </summary>
        int ModalStackCount { get; }

        /// <summary>
        /// If possible, instructs the navigation service
        ///             to discard the current page and display the previous page
        ///             on the navigation stack.
        /// 
        /// </summary>
        Task GoBack();

        /// <summary>
        /// Pop to root navigation page
        /// 
        /// </summary>
        Task Home();

        /// <summary>
        /// Push a modal page on to the navigation stack
        /// 
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        ///             that should be displayed.</param>
        Task PushModal(string pageKey);

        /// <summary>
        /// Pop a modal page friom the navigation stack
        /// 
        /// </summary>
        Task PopModal();

        /// <summary>
        /// Instructs the navigation service to display a new page
        ///             corresponding to the given key. Depending on the platforms,
        ///             the navigation service might have to be configured with a
        ///             key/page list.
        /// 
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        ///             that should be displayed.</param>
        Task NavigateTo(string pageKey);

        /// <summary>
        /// Instructs the navigation service to display a new page
        ///             corresponding to the given key, and passes a parameter
        ///             to the new page.
        ///             Depending on the platforms, the navigation service might
        ///             have to be Configure with a key/page list.
        /// 
        /// </summary>
        /// <param name="pageKey">The key corresponding to the page
        ///             that should be displayed.</param><param name="parameter">The parameter that should be passed
        ///             to the new page.</param>
        Task NavigateTo(string pageKey, object parameter);
    }
}


