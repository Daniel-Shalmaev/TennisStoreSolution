﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using TennisStoreClient.Services;
using TennisStoreSharedLibrary.DTOs;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreClient.Authentication
{
    public class AuthenticationService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;

        public AuthenticationService(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        public async Task<UserSession> GetUserDetails()
        {
            var token = await GetTokenFromLocalStorage();
            if (string.IsNullOrEmpty(token))
                return null!;

            var httpClient = await AddHeaderToHttpClient();
            var userDetails = General.DeserializedJsonString<TokenProp>(token);
            if (userDetails is null || string.IsNullOrEmpty(userDetails.Token))
                return null!;

            var response = await GetUserDetailsFromApi();
            if (response.IsSuccessStatusCode)
                return await GetUserSession(response);

            if (httpClient.DefaultRequestHeaders.Contains("Authorization") &&
                response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var encodedToken = Encoding.UTF8.GetBytes(userDetails.RefreshToken!);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                var model = new PostRefreshTokenDTO() { RefreshToken = validToken };

                var result = await httpClient.PostAsync("api/account/refresh-token",
                    General.GenerateStringContent(General.SerilazedObj(model)));
                if (result.IsSuccessStatusCode && result.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = await result.Content.ReadAsStringAsync();
                    var loginResponse = General.DeserializedJsonString<LoginResponse>(apiResponse);

                    await SetTokenToLocalStorage(General.SerilazedObj(new TokenProp()
                    { Token = loginResponse.Token, RefreshToken = loginResponse.RefreshToken }));

                    var callApiAgain = await GetUserDetailsFromApi();
                    if (callApiAgain.IsSuccessStatusCode)
                        return await GetUserSession(callApiAgain);
                }
            }
            return null!;
        }

        public static async Task<UserSession> GetUserSession(HttpResponseMessage response)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return General.DeserializedJsonString<UserSession>(apiResponse);
        }

        // Public Methods
        public async Task SetTokenToLocalStorage(string token) =>
            await _localStorageService.SetItemAsStringAsync("access_token", token);

        public async Task<HttpClient> AddHeaderToHttpClient()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer",
            General.DeserializedJsonString<TokenProp>(await GetTokenFromLocalStorage()).Token);
            return _httpClient;
        }

        public async Task RemoveTokenFromLocalStorage() =>
            await _localStorageService.RemoveItemAsync("access_token");

        public ClaimsPrincipal SetClaimPrincipal(UserSession model)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new (ClaimTypes.Name , model.Name!),
                    new (ClaimTypes.Role , model.Role!),
                    new (ClaimTypes.Email , model.Email!)
                }, "AccessTokenAuth"));
        }

        // Private Methods
        private async Task<string> GetTokenFromLocalStorage() =>
            await _localStorageService.GetItemAsStringAsync("access_token");

        private async Task<HttpResponseMessage> GetUserDetailsFromApi()
        {
            var httpClient = await AddHeaderToHttpClient();
            return await httpClient.GetAsync("api/account/user-info");
        }
    }
}
