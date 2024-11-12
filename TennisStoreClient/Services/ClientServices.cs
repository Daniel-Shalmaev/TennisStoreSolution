using TennisStoreClient.Authentication;
using TennisStoreClient.PrivateModels;
using TennisStoreSharedLibrary.DTOs;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public class ClientServices(HttpClient httpClient, AuthenticationService authenticationService) :
        IProductService, ICategoryService, IUserAccountService , ICart
    {
        private const string ProductBaseUrl = "api/product";
        private const string CategoryBaseUrl = "api/category";
        private const string AuthenticationBaseUrl = "api/account";

        public Action? CategoryAction { get; set; }
        public Action? ProductAction { get; set; }
        public List<Category> AllCategories { get; set; }
        public List<Product> AllProducts { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> ProductsByCategory { get; set; }
        public bool IsVisible { get; set; }
        public Action? CartAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int CartCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsCartLoaderVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        #region  Products

        public async Task<ServiceResponse> AddProduct(Product model)
        {
            var privateHttpClient = await authenticationService.AddHeaderToHttpClient();
            var response = await privateHttpClient.PostAsync(ProductBaseUrl,
                General.GenerateStringContent(General.SerilazedObj(model)));
            var result = CheckResponse(response);

            if (!result.Flag) return result;

            var apiResponse = await ReadContent(response);
            var data = General.DeserializedJsonString<ServiceResponse>(apiResponse);
            if (!data.Flag) return data;
            await ClearAndGetAllProducts();
            return data;
        }
        private async Task ClearAndGetAllProducts()
        {
            bool featuredProduct = true;
            bool allProduct = false;
            AllProducts = null!;
            FeaturedProducts = null!;
            await GetAllProducts(featuredProduct);
            await GetAllProducts(allProduct);
        }
        public async Task GetAllProducts(bool featuredProducts)
        {
            if (featuredProducts && FeaturedProducts is null)
            {
                IsVisible = true;
                FeaturedProducts = await GetProducts(featuredProducts);
                IsVisible = false;
                ProductAction?.Invoke();
                return;
            }
            else
            {
                if (!featuredProducts && AllProducts is null)
                {
                    IsVisible = true;
                    AllProducts = await GetProducts(featuredProducts);
                    IsVisible = false;
                    ProductAction?.Invoke();
                    return;
                }
            }
        }

        private async Task<List<Product>> GetProducts(bool featured)
        {
            var response = await httpClient.GetAsync($"{ProductBaseUrl}?featured={featured}");
            var (flag, _) = CheckResponse(response);
            if (!flag) return null!;

            var result = await ReadContent(response);
            return (List<Product>?)General.DeserializedJsonStringList<Product>(result)!;
        }

        public async Task GetProductsByCategory(int categoryId)
        {
            bool featured = false;
            await GetAllProducts(featured);
            ProductsByCategory = AllProducts.Where(_ => _.CategoryId == categoryId).ToList();
            ProductAction?.Invoke();
        }

        public Product GetRandomProduct()
        {
            if (FeaturedProducts is null) return null!;
            Random RandomNumbers = new();
            int minimumNumber = FeaturedProducts.Min(_ => _.Id);
            int maximumNumber = FeaturedProducts.Max(_ => _.Id) + 1;
            var result = RandomNumbers.Next(minimumNumber, maximumNumber);
            return FeaturedProducts.FirstOrDefault(_ => _.Id == result)!;
        }

        #endregion

        #region Categories

        public async Task<ServiceResponse> AddCategory(Category model)
        {
            await authenticationService.GetUserDetails();
            var privateHttpClient = await authenticationService.AddHeaderToHttpClient();
            var response = await privateHttpClient.PostAsync(CategoryBaseUrl,
                General.GenerateStringContent(General.SerilazedObj(model)));

            var result = CheckResponse(response);
            if (!result.Flag) return result;

            var apiResponse = await ReadContent(response);

            var data = General.DeserializedJsonString<ServiceResponse>(apiResponse);
            if (!data.Flag) return data;
            await ClearAndGetAllCategories();
            return data;
        }
        public async Task GetAllCategories()
        {
            if (AllCategories is null)
            {
                var response = await httpClient.GetAsync($"{CategoryBaseUrl}");
                var (flag, _) = CheckResponse(response);
                if (!flag) return;

                var result = await ReadContent(response);
                AllCategories = (List<Category>?)General.DeserializedJsonStringList<Category>(result)!;
                CategoryAction?.Invoke();
            }
        }
        private async Task ClearAndGetAllCategories()
        {
            AllCategories = null!;
            await GetAllCategories();
        }

        #endregion

        #region  General Methods

        private static ServiceResponse CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return new ServiceResponse(false, "Error occured. Try again later...");
            else
                return new ServiceResponse(true, null!);
        }

        private static async Task<string> ReadContent(HttpResponseMessage response) =>
            await response.Content.ReadAsStringAsync();

        #endregion

        #region Account / Authentication

        public async Task<LoginResponse> Login(LoginDTO model)
        {
            var response = await httpClient.PostAsync($"{AuthenticationBaseUrl}/login",
                General.GenerateStringContent(General.SerilazedObj(model)));

            if (!response.IsSuccessStatusCode)
                return new LoginResponse(false, "Error occured", null!, null!);

            var apiResponse = await ReadContent(response);
            return General.DeserializedJsonString<LoginResponse>(apiResponse);
        }

        public async Task<ServiceResponse> Register(UserDTO model)
        {
            var response = await httpClient.PostAsync($"{AuthenticationBaseUrl}/register",
                General.GenerateStringContent(General.SerilazedObj(model)));
            var result = CheckResponse(response);
            if (!result.Flag)
                return result;

            var apiResponse = await ReadContent(response);
            return General.DeserializedJsonString<ServiceResponse>(apiResponse);
        }

        public Task GetCartCount()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> AddToCart(Product model, int updateQuantity = 1)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> MyOrders()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> DeleteCart(Order cart)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

