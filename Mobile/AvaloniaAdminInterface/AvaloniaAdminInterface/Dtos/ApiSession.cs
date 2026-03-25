using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaAdminInterface.Dtos
{
    public class ApiSession
    {
    
            public int ID { get; set; }
            public string UserName { get; set; } = "";
            public string Role { get; set; } = "";
   
            public HttpClient _client { get; set; }
            public CookieContainer Cookies { get; set; }

            public ApiSession(string url)
            {
                Cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler
                {
                    UseCookies = true,
                    CookieContainer = Cookies
                };

                _client = new HttpClient(handler) { BaseAddress = new Uri(url) };
            }
    }
}

