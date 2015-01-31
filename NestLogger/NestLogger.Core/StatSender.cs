using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Plasma.HostedGraphite.Services;

namespace NestLogger.Core
{
    public class StatSender
    {
        public void Send(DeviceReadings deviceReadings)
        {
            var config = new Config();

            StatsMix.Client smClient = new StatsMix.Client(config.StatsMixApiKey);
            var s1 = smClient.track("humidity", deviceReadings.CurrentHumidity);
            var s2 = smClient.track("temperature", deviceReadings.CurrentTemp);
            var s3 = smClient.track("target_temperature", deviceReadings.TargetTemp);

            if (config.SendRandom) {
                Random rnd = new Random();
                smClient.track("random", rnd.Next(1, 100));
            }

            Console.WriteLine("Tracked on StatsMix");

            var hgClient = new HostedGraphiteService();
            hgClient.ActivateForUse(config.GraphiteApiKey);
            hgClient.Send("humidity", Convert.ToSingle(deviceReadings.CurrentHumidity));
            hgClient.Send("temperature", Convert.ToSingle(deviceReadings.CurrentTemp));
            hgClient.Send("target_temperature", Convert.ToSingle(deviceReadings.TargetTemp));

            if (config.SendRandom)
            {
                Random rnd = new Random();
                hgClient.Send("random", rnd.Next(1, 100));
            }

            Console.WriteLine("Tracked on Graphite");

        }
    }
}
