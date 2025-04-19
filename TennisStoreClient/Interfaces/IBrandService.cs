using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Interfaces
{
    public interface IBrandService
    {
        Task GetAllBrands();
        Action? BrandAction { get; set; }
        Task<Brand?> GetBrandById(int brandId);
        public List<Brand> AllBrands { get; set; }
        Task<ServiceResponse> AddBrand(Brand model);
        Task<List<Product>> GetProductsByBrandAndSubcategory(int brandId, int subcategoryId);

    }
}
