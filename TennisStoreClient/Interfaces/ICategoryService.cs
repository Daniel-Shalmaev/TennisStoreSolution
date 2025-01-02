using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Interfaces
{
    public interface ICategoryService
    {
        Task GetAllCategories();
        Action? CategoryAction { get; set; }
        int GetSubCategoryCount(int categoryId);
        public List<Category> AllCategories { get; set; }
        Task<ServiceResponse> AddCategory(Category model);
    }
}
