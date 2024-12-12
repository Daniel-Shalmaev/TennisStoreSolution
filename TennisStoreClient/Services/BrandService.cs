using TennisStoreClient.Interfaces;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public class BrandService(HttpClient httpClient) : IBrandService
    {
        #region Fields

        private readonly HttpClient _httpClient = httpClient;

        #endregion

        #region Properties

        public Action? BrandAction { get; set; }
        public List<Brand> AllBrands { get; set; } = new();

        #endregion

        #region Public Methods

        public async Task GetAllBrands()
        {
            if (AllBrands.Any())
                return;

            var response = await _httpClient.GetAsync("api/brand");
            if (!response.IsSuccessStatusCode)
                return;

            var content = await response.Content.ReadAsStringAsync();
            AllBrands = General.DeserializedJsonStringList<Brand>(content).ToList() ?? [];
            BrandAction?.Invoke();
        }

        public async Task<ServiceResponse> AddBrand(Brand model)
        {
            var response = await _httpClient.PostAsync("api/brand",
                General.GenerateStringContent(General.SerilazedObj(model)));

            if (!response.IsSuccessStatusCode)
                return new ServiceResponse(false, "Failed to add brand");

            await RefreshBrands();
            return new ServiceResponse(true, "Brand added successfully");
        }

        #endregion

        #region Private Methods

        private async Task RefreshBrands()
        {
            AllBrands.Clear();
            await GetAllBrands();
        }

        #endregion
    }
}
