using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestSharp.Core.Models
{
    public interface IDevice
    {
        void ParseJson(dynamic jsonData);
    }
}
