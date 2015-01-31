using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestSharp.Core.Models
{
    public class Thermostat : Device, IDevice
    {
        public void ParseJson(dynamic jsonData)
        {
            throw new NotImplementedException();
        }
    }
}
