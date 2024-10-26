using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;
namespace TennisStoreServer.Repositories
{
    public interface ICategory
    {
        Task<ServiceResponse> AddCategory(Category model);
        Task<List<Category>> GetAllCategories();
    }
}
