using TennisStoreClient.Interfaces;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public class CategoryService(HttpClient httpClient) : ICategoryService
    {
        #region Fields

        private readonly HttpClient _httpClient = httpClient;

        #endregion

        #region  Properties

        public Action? CategoryAction { get; set; }
        public List<Category> AllCategories { get; set; } = [];

        #endregion

        #region API Calls

        public async Task GetAllCategories()
        {
            var response = await _httpClient.GetAsync("api/category");
            if (!response.IsSuccessStatusCode) return;

            var content = await response.Content.ReadAsStringAsync();
            AllCategories = General.DeserializedJsonStringList<Category>(content).ToList() ?? [];
            CategoryAction?.Invoke();
        }

        public async Task<ServiceResponse> AddCategory(Category model)
        {
            var response = await _httpClient.PostAsync("api/category",
                General.GenerateStringContent(General.SerilazedObj(model)));
            var result = CheckResponse(response);
            if (!result.Flag) return result;

            await RefreshCategories();
            return result;
        }

        #endregion

        #region Category Management

        private async Task RefreshCategories()
        {
            AllCategories.Clear();
            await GetAllCategories();
        }

        #endregion

        #region Helper Methods

        private static ServiceResponse CheckResponse(HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode
                ? new ServiceResponse(true, null!)
                : new ServiceResponse(false, "Error occurred. Try again later...");
        }

        #endregion
    }
}
