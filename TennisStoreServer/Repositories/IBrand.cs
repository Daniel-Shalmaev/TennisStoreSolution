using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreServer.Repositories
{
    public interface IBrand
    {
        Task<List<Brand>> GetAllBrands();
        Task<Brand?> GetBrandById(int id);
        Task<ServiceResponse> AddBrand(Brand model);
    }
}
