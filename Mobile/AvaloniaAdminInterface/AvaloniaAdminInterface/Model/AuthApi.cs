using JobHiringAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<BaseUserDto> GetUserData()
        {
            return await _session._client.GetFromJsonAsync<BaseUserDto>("api/user/whoami");
        }

        public async Task<bool> CanProcced(string username,string pass)
        {
            //reminder make a get user by name in user 
            //make it return basicuser dto
            if ()
            {
                return false;
                throw new UnauthorizedAccessException("Current user is not an admin");
               
            }
            return true;
        }
    }
}
