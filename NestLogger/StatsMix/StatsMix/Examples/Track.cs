using System;
using System.Collections;
using System.Collections.Generic;
using StatsMix;

namespace StatsMix.Example
{
    class Track
    {
        const string StatsMixApiKey = "Your Api Key";
        static void Main()
        {
    
            //Create a new StatsMix Client
            StatsMix.Client smClient = new StatsMix.Client(StatsMixApiKey);

            //Basic Tracking.  Adds a new stat with default value of 1 to the metric.
            smClient.track("metric_name");

            //Track with a value other than one
            smClient.track("metric_name", 5);

            //Track with additional properties
            var properties = new Dictionary<string, string>();
            properties.Add("value", "5.1"); //If you do not include value, it will default to 1
            properties.Add("ref_id", "Test01");
            properties.Add("generated_at", DateTime.Now.ToString());
            smClient.track("metric_name", properties);

            //Track with meta data
            var meta = new Dictionary<string, string>();
            meta.Add("food", "icecream");
            meta.Add("calories", "500");
            smClient.track("metric_name", properties, meta);

            //Checking the response for failed requests
            string response = smClient.track("metric_name");
            if (response.Contains("error")) 
            {
                //handle
            }
        }
    }
}
