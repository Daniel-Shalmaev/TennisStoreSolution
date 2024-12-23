using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Interfaces
{
    public interface IProductService
    {
        Product GetRandomProduct();
        bool IsVisible { get; set; }
        Action? ProductAction { get; set; }
        List<Product> AllProducts { get; set; }
        Task GetAllProducts(bool featuredProducts);
        Task GetProductsByCategory(int categoryId);
        List<Product> FeaturedProducts { get; set; }
        Task<Product?> GetProductById(int productId);
        List<Product> ProductsByCategory { get; set; }
        Task<ServiceResponse> AddProduct(Product model);
    }
}
