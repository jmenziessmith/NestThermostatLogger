using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestSharp.Core
{
    public partial class NestClient
    {
        private string GetDateAsJsonDate()
        {
            DateTime epoch = new DateTime(1970, 1, 1);
            DateTime ut = DateTime.Now.ToUniversalTime();
            TimeSpan ts = new TimeSpan(ut.Ticks - epoch.Ticks);
            return Math.Round(ts.TotalMilliseconds).ToString();
        }
    }
}
