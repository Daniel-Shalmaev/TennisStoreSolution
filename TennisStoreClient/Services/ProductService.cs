using TennisStoreClient.Interfaces;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public class ProductService(HttpClient httpClient) : IProductService
    {
        #region Fields 

        private readonly HttpClient _httpClient = httpClient;

        #endregion

        #region Properties

        public bool IsVisible { get; set; }
        public Action? ProductAction { get; set; }
        public List<Product> AllProducts { get; set; } = [];
        public List<Product> FeaturedProducts { get; set; } = [];
        public List<Product> ProductsByCategory { get; set; } = [];

        #endregion

        #region Public Methods

        public Product GetRandomProduct()
        {
            if (FeaturedProducts == null || !FeaturedProducts.Any())
                throw new InvalidOperationException("No featured products available.");

            var products = FeaturedProducts.ToList();
            return products[new Random().Next(products.Count)];
        }

        public async Task GetAllProducts(bool featuredProducts)
        {
            var response = await _httpClient.GetAsync($"api/product?featured={featuredProducts}");
            if (!response.IsSuccessStatusCode) return;

            var content = await response.Content.ReadAsStringAsync();
            var products = General.DeserializedJsonStringList<Product>(content);
            if (featuredProducts)
                FeaturedProducts = products?.ToList() ?? [];
            else
                AllProducts = products?.ToList() ?? [];

            ProductAction?.Invoke();
        }

        public async Task GetProductsByCategory(int categoryId)
        {
            await GetAllProducts(false);
            ProductsByCategory = AllProducts.Where(p => p.CategoryId == categoryId).ToList();
            ProductAction?.Invoke();
        }

        public async Task<ServiceResponse> AddProduct(Product model)
        {
            var response = await _httpClient.PostAsync("api/product",
                General.GenerateStringContent(General.SerilazedObj(model)));
            var result = CheckResponse(response);

            if (!result.Flag) return result;

            await RefreshProducts();
            return result;
        }

        #endregion

        #region Private Methods

        private async Task RefreshProducts()
        {
            FeaturedProducts.Clear();
            AllProducts.Clear();
            await GetAllProducts(true);
            await GetAllProducts(false);
        }

        private static ServiceResponse CheckResponse(HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode
                ? new ServiceResponse(true, null!)
                : new ServiceResponse(false, "Error occurred. Try again later...");
        }

        #endregion
    }
}
