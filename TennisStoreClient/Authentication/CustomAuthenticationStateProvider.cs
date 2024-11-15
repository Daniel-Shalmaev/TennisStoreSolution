using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TennisStoreClient.Services;

namespace TennisStoreClient.Authentication
{
    public class CustomAuthenticationStateProvider(AuthenticationService authenticationService) : AuthenticationStateProvider
    {
        private ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var getUserSession = await authenticationService.GetUserDetails();

                if (getUserSession is null || string.IsNullOrEmpty(getUserSession.Email))
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var claimsPrincipal = authenticationService.SetClaimPrincipal(getUserSession);
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch { return await Task.FromResult(new AuthenticationState(anonymous)); }
        }

        public async Task UpdateAuthenticationState(TokenProp tokenProp)
        {
            ClaimsPrincipal claimsPrincipal = anonymous;

            if (tokenProp?.Token is not null)
            {
                await authenticationService.SetTokenToLocalStorage(General.SerilazedObj(tokenProp));
                var getUserSession = await authenticationService.GetUserDetails();

                if (getUserSession?.Email is not null)
                    claimsPrincipal = authenticationService.SetClaimPrincipal(getUserSession);
            }
            else
            {
                await authenticationService.RemoveTokenFromLocalStorage();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
