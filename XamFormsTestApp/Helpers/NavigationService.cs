using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XamFormsTestApp.Data.Services.Common;
using Xamarin.Forms;

namespace XamFormsTestApp.Helpers
{
    public class NavigationService : INavService
    {
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();

        /// <summary>
        ///     Get the current page key
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                {
                    lock (_pagesByKey)
                    {
                        if (((NavigationPage) Application.Current.MainPage).CurrentPage == null)
                        {
                            return null;
                        }

                        var pageType = ((NavigationPage) Application.Current.MainPage).CurrentPage.GetType();

                        return _pagesByKey.ContainsValue(pageType)
                            ? _pagesByKey.First(p => p.Value == pageType).Key
                            : null;
                    }
                }
            }
        }

        /// <summary>
        ///     Get the current modal page key
        /// </summary>
        public string CurrentModalPageKey
        {
            get
            {
                {
                    lock (_pagesByKey)
                    {
                        // 
                        if (Application.Current.MainPage.Navigation.ModalStack.Count == 1)
                        {
                            return null;
                        }

                        var pageType = Application.Current.MainPage.Navigation.ModalStack.Last().GetType();

                        return _pagesByKey.ContainsValue(pageType)
                            ? _pagesByKey.First(p => p.Value == pageType).Key
                            : null;
                    }
                }
            }
        }

        /// <summary>
        ///     Count of modal pages on the stack. 1 = 0 pages (only navigation parent)
        /// </summary>
        public int ModalStackCount
        {
            get { return Application.Current.MainPage.Navigation.ModalStack.Count; }
        }

        /// <summary>
        ///     Navigate back
        /// </summary>
        public async Task GoBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        ///     Go to home page
        /// </summary>
        public async Task Home()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        /// <summary>
        ///     Push a modal page on to the navigation stack
        /// </summary>
        /// <param name="pageKey">Modal page to navigate to</param>
        public async Task PushModal(string pageKey)
        {
            if (_pagesByKey.ContainsKey(pageKey))
            {

                var type = _pagesByKey[pageKey];
                ConstructorInfo constructor = null;
                object[] parameters = null;

               
                constructor = type.GetTypeInfo()
                    .DeclaredConstructors
                    .FirstOrDefault(c => !c.GetParameters().Any());

                parameters = new object[]
                {
                };

                var page = constructor.Invoke(parameters) as Page;
                await Application.Current.MainPage.Navigation.PushModalAsync(page);
            }
            else
            {
                throw new ArgumentException(
                    string.Format(
                        "No such page: {0}. Did you forget to call NavigationService.Configure?",
                        pageKey),
                    "pageKey");
            }
        }

        /// <summary>
        ///     Closes the current modal page. Forces VM cleanup if IPageLifetime is implemented
        /// </summary>
        public async Task PopModal()
        {
            var modalPage = Application.Current.MainPage.Navigation.ModalStack.LastOrDefault();

            if (!Equals(modalPage,null))
            {

                var cleanup = modalPage as IPageLifetime;
                if (cleanup != null)
                {
                    // Unregister vm of page, message listener etc
                    cleanup.CleanupPage();
                }

                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        /// <summary>
        ///     Navigate to a page with no parameter
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        public async Task NavigateTo(string pagekey)
        {
            await NavigateTo(pagekey, null);
        }

        /// <summary>
        ///     Navigate to a page with parameter
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        /// <param name="parameter">Navigation parameter</param>
        public async Task NavigateTo(string pagekey, object parameter)
        {

            if (_pagesByKey.ContainsKey(pagekey))
            {

                var type = _pagesByKey[pagekey];
                ConstructorInfo constructor = null;
                object[] parameters = null;

                if (parameter == null)
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(c => !c.GetParameters().Any());

                    parameters = new object[]
                    {
                    };
                }
                else
                {
                    constructor = type.GetTypeInfo()
                        .DeclaredConstructors
                        .FirstOrDefault(
                            c =>
                            {
                                var p = c.GetParameters();
                                return p.Count() == 1
                                        && p[0].ParameterType == parameter.GetType();
                            });

                    parameters = new[]
                    {
                        parameter
                    };
                }

                if (constructor == null)
                {
                    throw new InvalidOperationException(
                        "No suitable constructor found for page " + pagekey);
                }

                var page = constructor.Invoke(parameters) as Page;
                await Application.Current.MainPage.Navigation.PushAsync(page);
            }
            else
            {
                throw new ArgumentException(
                    string.Format(
                        "No such page: {0}. Did you forget to call NavigationService.Configure?",
                        pagekey),
                    "pagekey");
            }
        }

        /// <summary>
        ///     Configure navigation page
        /// </summary>
        /// <param name="pagekey">Page to navigate to</param>
        /// <param name="pageType">Type of page</param>
        public void Configure(string pagekey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pagekey))
                {
                    _pagesByKey[pagekey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pagekey, pageType);
                }
            }
        }
    }
}