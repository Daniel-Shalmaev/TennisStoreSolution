using Blazored.LocalStorage;
using TennisStoreClient.Authentication;
using TennisStoreClient.PrivateModels;
using TennisStoreSharedLibrary.DTOs;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public class ClientServices(HttpClient httpClient, AuthenticationService authenticationService, ILocalStorageService localStorageService) :
        IProductService, ICategoryService, IUserAccountService, ICart , IBrandService
    {
        private const string ProductBaseUrl = "api/product";
        private const string CategoryBaseUrl = "api/category";
        private const string BrandBaseUrl = "api/brand";
        private const string AuthenticationBaseUrl = "api/account";

        public Action? CategoryAction { get; set; }
        public Action? ProductAction { get; set; }
        public Action? BrandAction { get ; set ; }
        public List<Category> AllCategories { get; set; } 
        public List<Brand> AllBrands { get; set; } 
        public List<Product> AllProducts { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> ProductsByCategory { get; set; }
        public bool IsVisible { get; set; }
        public Action? CartAction { get; set; }
        public int CartCount { get; set; }
        public bool IsCartLoaderVisible { get; set; }


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
            if (FeaturedProducts == null || !FeaturedProducts.Any())
                throw new InvalidOperationException("No featured products available.");

            var products = FeaturedProducts.ToList();
            return products[new Random().Next(products.Count)];
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

        #region Brands
        public async Task GetAllBrands()
        {
            if (AllBrands is null)
            {
                var response = await httpClient.GetAsync($"{BrandBaseUrl}");
                var (flag, _) = CheckResponse(response);
                if (!flag) return;

                var result = await ReadContent(response);
                AllBrands = (List<Brand>?)General.DeserializedJsonStringList<Brand>(result)!;
                BrandAction?.Invoke();
            }
        }

        public async Task<ServiceResponse> AddBrand(Brand model)
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

        #endregion

        #region Cart Service

        public async Task GetCartCount()
        {
            string cartString = await GetCartFromLocalStorage();
            if (string.IsNullOrEmpty(cartString))
                CartCount = 0;
            else
                CartCount = General.DeserializedJsonStringList<StorageCart>(cartString).Count();
            CartAction?.Invoke();
        }

        public async Task<ServiceResponse> AddToCart(Product model, int updateQuantity = 1)
        {
            string message = string.Empty;
            var MyCart = new List<StorageCart>();
            var getCartFromStorage = await GetCartFromLocalStorage();
            if (!string.IsNullOrEmpty(getCartFromStorage))
            {
                MyCart = (List<StorageCart>)General.DeserializedJsonStringList<StorageCart>(getCartFromStorage);
                var checkIfAddedAlerdy = MyCart.FirstOrDefault(_ => _.ProductId == model.Id);
                if (checkIfAddedAlerdy is null)
                {
                    MyCart.Add(new StorageCart() { ProductId = model.Id, Quantity = model.Quantity });
                    message = "Product added to cart";
                }
                else
                {
                    var updatedProduct = new StorageCart() { Quantity = updateQuantity, ProductId = model.Id };
                    MyCart.Remove(checkIfAddedAlerdy!);
                    MyCart.Add(updatedProduct);
                    message = "Product Updated";
                }
            }
            else
            {
                MyCart.Add(new StorageCart() { ProductId = model.Id, Quantity = 1 });
                message = "Product added to cart";
            }
            await RemoveCartFromLocalStorage();
            await SetCartFromLocalStorage(General.SerilazedObj(MyCart));
            await GetCartCount();
            return new ServiceResponse(true, message);
        }

        public async Task<List<Order>> MyOrders()
        {
            IsCartLoaderVisible = true;
            var cartList = new List<Order>();
            string myCartString = await GetCartFromLocalStorage();
            if (string.IsNullOrEmpty(myCartString)) return null!;

            var myCartList = General.DeserializedJsonStringList<StorageCart>(myCartString);
            await GetAllProducts(false);
            foreach (var cartItem in myCartList)
            {
                var product = AllProducts.FirstOrDefault(_ => _.Id == cartItem.ProductId);
                cartList.Add(new Order()
                {
                    Id = product!.Id,
                    Name = product.Name,
                    Quantity = cartItem.Quantity,
                    Price = product.Price,
                    Image = product.Base64Img
                });
            }
            IsCartLoaderVisible = false;
            await GetCartCount();
            return cartList;

        }

        public async Task<ServiceResponse> DeleteCart(Order cart)
        {
            var myCartList = General.DeserializedJsonStringList<StorageCart>(await GetCartFromLocalStorage());
            if (myCartList is null)
                return new ServiceResponse(false, "Product not found");

            myCartList.Remove(myCartList.FirstOrDefault(_ => _.ProductId == cart.Id)!);
            await RemoveCartFromLocalStorage();
            await SetCartFromLocalStorage(General.SerilazedObj(myCartList));
            await GetCartCount();
            return new ServiceResponse(true, "Product removed successfully");
        }

        private async Task<string> GetCartFromLocalStorage() => await localStorageService.GetItemAsStringAsync("cart");
        private async Task SetCartFromLocalStorage(string cart) => await localStorageService.SetItemAsStringAsync("cart", cart);
        private async Task RemoveCartFromLocalStorage() => await localStorageService.RemoveItemAsync("cart");

       

        #endregion
    }
}

