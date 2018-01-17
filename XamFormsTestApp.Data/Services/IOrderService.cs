using System.Collections.ObjectModel;
using XamFormsTestApp.Data.Model;
using System.Threading.Tasks;

namespace XamFormsTestApp.Data.Services
{
    public interface IOrderService
    {
        Task<ObservableCollection<Order>> GetOrders();
        Task<OrderDetails> GetOrderDetails(string vesid);
    }
}
