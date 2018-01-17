using GalaSoft.MvvmLight;

namespace XamFormsTestApp.Data.Model
{
    public class Order: ObservableObject
    {
        public string OrdId { get; set; }
        private string _orderName;
        private bool _fav;
        public string OrdName
        {
            get { return _orderName; }
            set { Set(() => OrdName, ref _orderName, value); }
        }
        public bool Favorite
        {
            get { return _fav; }
            set { Set(() => Favorite, ref _fav, value); }
        }
    }
}
