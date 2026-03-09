using JobHiringAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
             
        }



        private void EnsureAdmin()
        {
            if (!string.Equals(_session.Role, "Admin"))
            {
                throw new UnauthorizedAccessException("Current user is not an admin");
            }
        }
        public async Task<BaseUserDto> NameById(int id)
        {
            EnsureAdmin();
            var response = await _session._client.GetFromJsonAsync<List<BaseUserDto>>("api/AdminController/username"); 
            return response.Where(x=> x.ID == id).FirstOrDefault();
        }
        public async Task<List<BaseUserDto>> GetUsers()
        {
            EnsureAdmin();
            var response = await _session._client.GetFromJsonAsync<List<BaseUserDto>>("api/AdminController/users");


            return response;
        }
        
        public async Task<List<AdminCompanyDto>> GetCompanies()
        {
            EnsureAdmin();
            var response = await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>("api/AdminController/companies");

            return response;
        }
        
        public async Task<List<AdminCompanyDto>> GetJobs()
        {
            EnsureAdmin();
            var response = await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>("api/AdminController/jobs");

            return response;
        }
        
        public async Task<List<AdminCompanyDto>> GetRequestsByJobId(int id)
        {
            EnsureAdmin();
            var response = await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>($"api/AdminController/requests?{id}");

            return response;
        }

        public async Task<List<AdminCompanyDto>> GetRequestsByUserId(int id)
        {
            EnsureAdmin();
            var response = await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>($"api/AdminController/requests/user?{id}");

            return response;
        }

        public async Task<List<AdminCompanyDto>> GetRequestsByCompanyId(int id)
        {
            EnsureAdmin();
            var response = await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>($"api/AdminController/requests/requests/company?{id}");

            return response;
        }

        public async Task ResetPassword(int id , int secure )
        {
            EnsureAdmin();
            var response = await _session._client.PostAsJsonAsync("api/AdminController/requests/user", id);

            if (response.StatusCode != System.Net.HttpStatusCode.OK) 
            {
                throw new UnauthorizedAccessException();
            }
            

        }
        
    }
}
