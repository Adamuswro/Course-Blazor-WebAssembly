using BlazorBattles.Client.Shared;
using BlazorBattles.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBattles.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient http;

        public AuthService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await http.PostAsJsonAsync("api/auth/register", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
