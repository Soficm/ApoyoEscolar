using Apoyo.Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Apoyo.Client.Auth
{
    public class JWTAuthenticationProvider: AuthenticationStateProvider, ILoginServices
    {
        private readonly IJSRuntime js;
        private readonly HttpClient httpClient;
        private static readonly string TOKENKEY = "TOKENKEY";
        private AuthenticationState anonimo => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        public JWTAuthenticationProvider(IJSRuntime js, HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.js = js;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.GetFromLocalStorage(TOKENKEY);
            if (string.IsNullOrEmpty(token))
            {
                return anonimo;
            }
            return ConstruirAuthentificationState(token);
        }
        private AuthenticationState ConstruirAuthentificationState(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }
        public async Task Login(string token)
        {
            await js.SetInLocalStorage(TOKENKEY, token);
            var authState = ConstruirAuthentificationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }
        public async Task Logout()
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
            await js.RemoveItem(TOKENKEY);
            NotifyAuthenticationStateChanged(Task.FromResult(anonimo));
        }
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}

