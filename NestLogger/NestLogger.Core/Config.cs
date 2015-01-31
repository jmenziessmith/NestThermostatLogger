using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NestLogger.Core
{
    public class Config
    {
        private string GetString(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        public string Username
        {
            get { return GetString("Nest.Username"); }
        }

        public string Password
        {
            get { return GetString("Nest.Password");  }
        }

        public string StatsMixApiKey
        {
            get { return GetString("STATSMIX_URL").Split('/').Last(); }
        }

        public string GraphiteApiKey
        {
            get { return GetString("HOSTEDGRAPHITE_APIKEY"); }
        }

        public int JobMinutes
        {
            get { return GetInt("Job.Minutes",5); }
        }

        public bool SendRandom
        {
            get { return GetBool("SendRandom", false); }
        }

        private bool GetBool(string key, bool defaultValue)
        {
            var result = defaultValue;
            var stringValue = GetString(key);
            if (!string.IsNullOrEmpty(stringValue))
            {
                if (bool.TryParse(stringValue, out result))
                {
                    return result;
                }
            }
            return defaultValue;
        }

        private int GetInt(string key, int defaultValue)
        {
            var result = defaultValue;
            var stringValue = GetString(key);
            if (!string.IsNullOrEmpty(stringValue))
            {
                if (int.TryParse(stringValue, out result))
                {
                    return result;
                }
            }
            return defaultValue;
        }
    }
}
