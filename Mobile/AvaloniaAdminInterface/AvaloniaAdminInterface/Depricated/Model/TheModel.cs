using JobHiringAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvaloniaAdminInterface.Model
{
    public class TheModel
    {
        private readonly ApiSession _session;

        public TheModel(ApiSession session)
        {
            _session = session;
        }

        private void EnsureAdmin()
        {
            if (!string.Equals(_session.Role, "Admin"))
                throw new UnauthorizedAccessException("Current user is not an admin");
        }

        // --------------Log in/out-------------
        // GET api/user/login
        public async Task<UserLoginDto?> Log_in(string name, string pass)
        {
            var response = await _session._client.PostAsync(
                $"api/user/login?username={name}&password={pass}", null);

            response.EnsureSuccessStatusCode();

            var dto = await response.Content.ReadFromJsonAsync<UserLoginDto>();

            if (dto != null)
            {
                _session.Role = dto.Role;      
                _session.ID = dto.UserID;      
                _session.UserName = dto.UserName;
            }

            return dto;
        }

        // GET api/user/logout

        public async Task Log_Out()
        {
            var response = await _session._client.PostAsync("/api/user/logout", null);
            response.EnsureSuccessStatusCode();
        }

        //--------------Gets------------------

        // GET api/admin/users
        public async Task<List<BaseUserDto>> GetUsers()
        {
            EnsureAdmin();
            return await _session._client.GetFromJsonAsync<List<BaseUserDto>>(
                "api/admin/users");
        }//will need to limit to 10 / tab or maybe not idk at this point

        // GET api/admin/username?id=123
        public async Task<BaseUsernameDto?> GetUserName(int id)
        {
            EnsureAdmin();
            return await _session._client.GetFromJsonAsync<BaseUsernameDto>(
                $"api/admin/username?id={id}");
        }





        // GET api/admin/companies?id=123
        public async Task<List<AdminCompanyDto>> GetCompanies(int id)
        {
            EnsureAdmin();
            return await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>(
                $"api/admin/companies?id={id}");
        }

        // GET api/admin/jobs?id=123
        public async Task<List<AdminCompanyDto>> GetJobs(int id)
        {
            EnsureAdmin();
            return await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>(
                $"api/admin/jobs?id={id}");
        }

        // GET api/admin/requests?id=123
        public async Task<List<AdminCompanyDto>> GetRequestsByJobId(int id)
        {
            EnsureAdmin();
            return await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>(
                $"api/admin/requests?id={id}");
        }

        // GET api/admin/requests/user?id=123
        public async Task<List<AdminCompanyDto>> GetRequestsByUserId(int id)
        {
            EnsureAdmin();
            return await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>(
                $"api/admin/requests/user?id={id}");
        }

        // GET api/admin/requests/company?id=123
        public async Task<List<AdminCompanyDto>> GetRequestsByCompanyId(int id)
        {
            EnsureAdmin();
            return await _session._client.GetFromJsonAsync<List<AdminCompanyDto>>(
                $"api/admin/requests/company?id={id}");
        }
    
        //-------------Puts-----------------


        // PUT api/admin/request/status
        public async Task UpdateRequestStatus(AdminUpdateRequestStatusDto dto)
        {
            EnsureAdmin();
            var response = await _session._client.PutAsJsonAsync(
                "api/admin/request/status", dto);

            response.EnsureSuccessStatusCode();
        }
        //-----------Promote to admin demote to user-----
        //(kinda security concern but hey employes can be fired and prosicuted hackers npot as easily)

        // PUT api/admin/user/promote?id=123
        public async Task PromoteUser(int id)
        {
            EnsureAdmin();
            var response = await _session._client.PutAsync(
                $"api/admin/user/promote?id={id}", null);

            response.EnsureSuccessStatusCode();
        }


        // PUT api/admin/user/demote?id=321
        public async Task DemoteUser(int id)
        {
            EnsureAdmin();
            var response = await _session._client.PutAsync(
                $"api/admin/user/demote?id={id}", null);

            response.EnsureSuccessStatusCode();
        }

        // enforce wellbehawed slavers i mean the terms and conditions for companyes

        // PUT api/admin/request/underreview?id=42
        public async Task PutUnderReview(int id)
        {
            EnsureAdmin();
            var response = await _session._client.PutAsync(
                $"api/admin/request/underreview?id={id}", null);

            response.EnsureSuccessStatusCode();
        }

        //---------reset password for user -----------

        // PUT api/admin/reset?id=123
        public async Task<string> ResetPassword(int id)
        {
            EnsureAdmin();
            var response = await _session._client.PutAsync(
                $"api/admin/reset?id={id}", null);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        //-----------Deletes -----------

        // DELETE api/admin/delete/user?id=47 
        //agent 47 bit the dust :)
        public async Task DeleteUser(int id)
        {
            EnsureAdmin();
            var response = await _session._client.DeleteAsync(
                $"api/admin/delete/user?id={id}");

            response.EnsureSuccessStatusCode();
        }

        // DELETE api/admin/delete/company?id=123
        public async Task DeleteCompany(int id)
        {
            EnsureAdmin();
            var response = await _session._client.DeleteAsync(
                $"api/admin/delete/company?id={id}");

            response.EnsureSuccessStatusCode();
        }
        // DELETE api/admin/delete/job?id=1
        public async Task DeleteJob(int id)
        {
            EnsureAdmin();
            var response = await _session._client.DeleteAsync(
                $"api/admin/delete/job?id={id}");

            response.EnsureSuccessStatusCode();
        }
    }
    //2 am insanity
    //4am torture
    //3am 3rd day in a row of not sleeping enough
}
