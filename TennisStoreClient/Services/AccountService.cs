using TennisStoreClient.Interfaces;
using TennisStoreSharedLibrary.DTOs;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Services
{
    public class AccountService(HttpClient httpClient) : IUserAccountService
    {
        #region Fields 

        private readonly HttpClient _httpClient = httpClient;
        private const string AuthenticationBaseUrl = "api/account";

        #endregion

        #region Authentication Methods

        public async Task<LoginResponse> Login(LoginDTO model)
        {
            var response = await _httpClient.PostAsync($"{AuthenticationBaseUrl}/login",
                General.GenerateStringContent(General.SerilazedObj(model)));

            if (!response.IsSuccessStatusCode)
                return new LoginResponse(false, "Error occurred", null!, null!);

            var apiResponse = await response.Content.ReadAsStringAsync();
            return General.DeserializedJsonString<LoginResponse>(apiResponse);
        }

        public async Task<ServiceResponse> Register(UserDTO model)
        {
            var response = await _httpClient.PostAsync($"{AuthenticationBaseUrl}/register",
                General.GenerateStringContent(General.SerilazedObj(model)));

            var result = CheckResponse(response);
            if (!result.Flag)
                return result;

            var apiResponse = await response.Content.ReadAsStringAsync();
            return General.DeserializedJsonString<ServiceResponse>(apiResponse);
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
