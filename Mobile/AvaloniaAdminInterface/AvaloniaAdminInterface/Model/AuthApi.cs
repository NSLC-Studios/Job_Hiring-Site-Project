using JobHiringAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.Model
{
    public class AuthApi
    {
        private readonly ApiSession _session;

        public AuthApi(ApiSession session)
        {
            _session = session;
        }

        public async Task<BaseUserDto> GetUserData(string user2,string pass)
        {
            // await _session._client.PostAsync<BaseUserDto>("api/user/")
            var response = await _session._client.PostAsJsonAsync<BaseUserDto>($"api/user/login?username={user2}&password={pass}", null);
            response.EnsureSuccessStatusCode();



            // Read and deserialize the response body
            var user = await response.Content.ReadFromJsonAsync<BaseUserDto>();

            return user!;
        }

        public async Task CanProcced(string username,string pass)
        {
            //reminder make a get user by name in user 
            //make it return basicuser dto
           
        }
    }
}
