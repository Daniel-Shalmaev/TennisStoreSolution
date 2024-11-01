using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public interface IProductService
    {
        Action? ProductAction { get; set; }
        Task<ServiceResponse> AddProduct(Product model);
        Task GetAllProducts(bool featuredProducts);
        Task GetProductsByCategory (int categoryId);
        List<Product> AllProducts { get; set; }
        List<Product> FeaturedProducts { get; set; }
        List<Product> ProductsByCategory { get; set; }
        Product GetRandomProduct();
        bool IsVisible { get; set; }
    }
}
