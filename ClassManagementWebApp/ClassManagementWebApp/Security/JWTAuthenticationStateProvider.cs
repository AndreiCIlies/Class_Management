using ClassManagementWebApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClassManagementWebApp.Security
{
    public class JWTAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AccessTokenService _accessTokenService;

        public JWTAuthenticationStateProvider(AccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _accessTokenService.GetToken();
                if (string.IsNullOrWhiteSpace(token))
                {
                    return await MarkAsUnauthorzied();
                }

                var readJWT = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
                var principal = new ClaimsPrincipal(identity);

                return await Task.FromResult(new AuthenticationState(principal));
            }
            catch (Exception ex)
            {
                return await MarkAsUnauthorzied();
            }
        }

        private async Task<AuthenticationState> MarkAsUnauthorzied()
        {
            try
            {
                var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                NotifyAuthenticationStateChanged(Task.FromResult(state));

                return state;
            }
            catch (Exception ex)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}
