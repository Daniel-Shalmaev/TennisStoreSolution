using TennisStoreClient.PrivateModels;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;
using Order = TennisStoreClient.PrivateModels.Order;

namespace TennisStoreClient.Interfaces
{
    public interface ICart
    {
        Task GetCartCount();
        Task<List<Order>> MyOrders();
        public int CartCount { get; set; }
        bool IsCartLoaderVisible { get; set; }
        public Action? CartAction { get; set; }
        Task<ServiceResponse> DeleteCart(Order cart);
        Task<ServiceResponse> AddToCart(Product model, int updateQuantity = 1);
    }
}
