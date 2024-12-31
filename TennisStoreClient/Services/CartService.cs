using Blazored.LocalStorage;
using TennisStoreClient.Interfaces;
using TennisStoreClient.PrivateModels;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;
using Order = TennisStoreClient.PrivateModels.Order;

namespace TennisStoreClient.Services
{
    public class CartService(ILocalStorageService localStorageService) : ICart
    {
        #region Fields 

        private readonly ILocalStorageService _localStorageService = localStorageService;

        #endregion

        #region Properties

        public Action? CartAction { get; set; }
        public int CartCount { get; set; }
        public bool IsCartLoaderVisible { get; set; }

        #endregion

        #region Public Methods

        public async Task GetCartCount()
        {
            string cartString = await GetCartFromLocalStorage();

            CartCount = string.IsNullOrEmpty(cartString)
                ? 0
                : General.DeserializedJsonStringList<StorageCart>(cartString).Sum(item => item.Quantity);

            CartAction?.Invoke();
        }

        public async Task<ServiceResponse> AddToCart(Product model, int updateQuantity = 1)
        {
            string message;
            var cartItems = await GetCartItems();
            var existingItem = cartItems.FirstOrDefault(item => item.ProductId == model.Id);

            if (existingItem == null)
            {
                cartItems.Add(new StorageCart { ProductId = model.Id, Quantity = updateQuantity });
                message = "Product added to cart";
            }
            else
            {
                existingItem.Quantity = updateQuantity;
                message = "Product updated";
            }

            await SaveCartItems(cartItems);
            await GetCartCount();

            return new ServiceResponse(true, message);
        }

        public async Task<List<Order>> MyOrders()
        {
            IsCartLoaderVisible = true;

            var cartItems = await GetCartItems();
            var orders = cartItems.Select(cartItem => new Order
            {
                Id = cartItem.ProductId,
                Quantity = cartItem.Quantity
            }).ToList();

            IsCartLoaderVisible = false;
            await GetCartCount();

            return orders;
        }

        public async Task<ServiceResponse> DeleteCart(Order cart)
        {
            var cartItems = await GetCartItems();
            var itemToRemove = cartItems.FirstOrDefault(item => item.ProductId == cart.Id);

            if (itemToRemove == null)
                return new ServiceResponse(false, "Product not found");

            cartItems.Remove(itemToRemove);
            await SaveCartItems(cartItems);
            await GetCartCount();

            return new ServiceResponse(true, "Product removed successfully");
        }

        #endregion

        #region Private Methods

        private async Task<List<StorageCart>> GetCartItems()
        {
            string cartString = await GetCartFromLocalStorage();
            return string.IsNullOrEmpty(cartString)
                ? new List<StorageCart>()
                : General.DeserializedJsonStringList<StorageCart>(cartString).ToList();
        }

        private async Task SaveCartItems(List<StorageCart> cartItems)
        {
            string serializedCart = General.SerilazedObj(cartItems);
            await _localStorageService.SetItemAsStringAsync("cart", serializedCart);
        }

        private async Task<string> GetCartFromLocalStorage() =>
            await _localStorageService.GetItemAsStringAsync("cart");

        #endregion
    }
}
