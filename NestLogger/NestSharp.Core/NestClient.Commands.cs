using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NestSharp.Core
{
    public partial class NestClient
    {
        public async Task<dynamic> GetDevices()
        {
            var transportUri = string.Format("{0}/v2/mobile/user.{1}", TransportApiBaseAddress, UserId);

            var request = new HttpRequestMessage(HttpMethod.Get, transportUri);
            request.Headers.Add("X-nl-client-timestamp", GetDateAsJsonDate());
            request.Headers.Add("X-nl-protocol-version", "1");
            request.Headers.Add("Accept-Language", "en-us");
            request.Headers.Add("X-nl-user-id", UserId);
            request.Headers.Add("Authorization", string.Format("Basic {0}", AccessToken));

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject(responseBody);
        }
    }
}
