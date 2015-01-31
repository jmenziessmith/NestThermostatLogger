using System;
using System.Collections;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;

namespace StatsMix
{
    public class Client
    {
        public string ApiKey { get; private set; }
        private RestClient restClient;
        private const string BaseUrl = "http://api.statsmix.com/api/v2";

        public Client(string apiKey)
        {
            ApiKey = apiKey;
            restClient = new RestClient(BaseUrl);
            restClient.AddDefaultHeader("X-StatsMix-Token", apiKey);
            
        }

        public string track(string metricName, double value, Dictionary<string, string> properties, Dictionary<string, string> meta)
        {
            string json = JsonConvert.SerializeObject(meta);
            properties["value"] = value.ToString();
            properties["meta"] = json;
            properties["name"] = metricName;
            string resp = request("track", properties, Method.POST);
            return resp;
        }

        public string track(string metricName, Dictionary<string, string> properties, Dictionary<string, string> meta)
        {
            string json = JsonConvert.SerializeObject(meta);
            properties["meta"] = json;
            properties["name"] = metricName;
            string resp = request("track", properties, Method.POST);
            return resp;
        }

        public string track(string metricName, Dictionary<string, string> properties)
        {
            properties["name"] = metricName;
            string resp = request("track", properties, Method.POST);
            return resp;
        }

        public string track(string metricName, double value = 1.0)
        {
            var properties = new Dictionary<string, string>();
            properties["name"] = metricName;
            properties["value"] = value.ToString();
            return request("track", properties, Method.POST);
        }

        public string track(string metricName, double value, Dictionary<string, string> properties, Method method)
        {
            properties["name"] = metricName;
            properties["value"] = value.ToString();
            return request("track", properties, Method.POST);
        }

        private string request(string resource, Dictionary<string, string> properties, Method method)
        {
            var req = new RestRequest(resource, method);
            foreach (string key in properties.Keys)
            {
                req.AddParameter(key, properties[key]);
            }
            IRestResponse resp = restClient.Execute(req);
            return resp.Content;
        }
    }
}
