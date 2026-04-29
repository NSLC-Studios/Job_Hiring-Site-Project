using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UnitTester.Controller_Tests
{
    public class AdminControllerTest :IClassFixture<CustomWebAplicationFactory>
    {
        private readonly HttpClient _client;

        public AdminControllerTest(CustomWebAplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
        /*
        [Fact]
        public async Task Promote_ReturnsForbidden_ForNonAdmin()
        {
            
     

            var response = await _client.PutAsync("/api/admin/user/promote?id=2", null);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Promote_ReturnsOk_ForAdmin()
        {
          
           
            var response = await _client.PutAsync("/api/admin/user/promote?id=2", null);

           
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        */
    }
}
