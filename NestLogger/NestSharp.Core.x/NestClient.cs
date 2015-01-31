using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NestSharp.Core
{
    public partial class NestClient
    {
        public string ApiUrl { get; set; }
        public string TransportApiBaseAddress { get; set; }
        public string WeatherUrl { get; set; }

        private string _username { get; set; }
        private string _password { get; set; }
        public string AccessToken { get; private set; }
        public DateTime AccessTokenExpiration { get; private set; }
        public string UserId { get; private set; }

        private HttpClient httpClient;

        public NestClient(string username, string password)
        {
            _username = username;
            _password = password;

            httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Nest/1.1.0.10 CFNetwork/548.0.4");
            httpClient.BaseAddress = new Uri("https://home.nest.com");
        }
    }
}
