using JobHiringAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.Model
{
    public class Model
    {
        //private readonly HttpClient _client;
        private readonly ApiSession _session;

        public Model(ApiSession session,HttpClient httpClient)
        {
            _session = session;
             //_client = httpClient;
        }

        private void EnsureAdmin()
        {
            if (!string.Equals(_session.Role, "Admin"))
            {
                throw new UnauthorizedAccessException("Current user is not an admin");
            }
        }

        public async Task<List<BaseUserDto>> GetUsers()
        {
            EnsureAdmin();
            var response = await _session._client.GetAsync("api/AdminController/users");

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new UnauthorizedAccessException();
            }
            return response.Users.Tolist(); 
        }

        public async Task ResetPassword(int id , int secure )
        {
            EnsureAdmin();
            var response = await _session._client.PostAsJsonAsync("api/AdminController/requests/user", id);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) 
            {
                throw new UnauthorizedAccessException();
            }
            response.Password = secure;

        }

    }
}
