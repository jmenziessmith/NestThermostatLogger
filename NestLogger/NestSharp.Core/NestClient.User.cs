using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NestSharp.Core.Models;
using Newtonsoft.Json;

namespace NestSharp.Core
{
    public partial class NestClient
    {
        public async Task Authenticate()
        {
            var loginRequestData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", _username),
                new KeyValuePair<string, string>("password", _password)
            });

            var response = await httpClient.PostAsync("/user/login", loginRequestData);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            dynamic jsonData = JsonConvert.DeserializeObject(responseBody);

            if (jsonData == null)
                throw new NestException(response);

            if (jsonData.urls == null)
                throw new NestException(response);

            AccessToken = jsonData.access_token;
            AccessTokenExpiration = jsonData.expires_in;
            UserId = jsonData.userid;
            var urls = jsonData.urls;
            ApiUrl = urls.rubyapi_url;
            TransportApiBaseAddress = urls.transport_url;
            WeatherUrl = urls.weather_url;
        }
    }
}
