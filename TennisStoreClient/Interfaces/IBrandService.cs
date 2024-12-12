using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Interfaces
{
    public interface IBrandService
    {
        Action? BrandAction { get; set; }
        public List<Brand> AllBrands { get; set; }
        Task GetAllBrands();
        Task<ServiceResponse> AddBrand(Brand model);
    }
}
