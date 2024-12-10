using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreServer.Repositories
{
    public interface IBrand
    {
        Task<ServiceResponse> AddBrand(Brand model);
        Task<List<Brand>> GetAllBrands();
    }
}
