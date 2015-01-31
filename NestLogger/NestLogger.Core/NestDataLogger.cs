using System;
using System.Linq;
using NestSharp.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 

namespace NestLogger.Core
{
    public class NestDataLogger
    {
        public void Execute()
        {
            try
            {
                var config = new Config();

                var deviceReadings = GetReadings(config.Username, config.Password);

                Console.WriteLine("{0} {1} {2}", 
                    deviceReadings.CurrentHumidity, 
                    deviceReadings.CurrentTemp,
                    deviceReadings.TargetTemp);

                var sender = new StatSender();
                sender.Send(deviceReadings);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {0} \n {1}", ex.Message, ex.StackTrace);
            }

        }

        private DeviceReadings GetReadings(string username, string password)
        {
            var nestClient = new NestClient(username, password);
            nestClient.Authenticate().Wait();

            var devicesTask = nestClient.GetDevices();
            devicesTask.Wait();
            dynamic result = devicesTask.Result;

            var devices = result.device;
            Type devicesType = devices.GetType();
            var devicesFirstProp = devicesType.GetProperties().First(x => x.Name == "First");
            dynamic firstDevice = devicesFirstProp.GetValue(devices).Value;

            var shareds = result.shared;
            Type sharedType = shareds.GetType();
            var sharedFirstProp = sharedType.GetProperties().First(x => x.Name == "First");
            dynamic firstShared = sharedFirstProp.GetValue(shareds).Value;

            double currentHumidity = firstDevice.current_humidity;
            double currentTemp = firstShared.current_temperature;
            double targetTemp = firstShared.target_temperature;


            var readings = new DeviceReadings()
                         {
                             CurrentHumidity = currentHumidity,
                             CurrentTemp = currentTemp,
                             TargetTemp = targetTemp
                         };

            return readings;
        }
    }
}
