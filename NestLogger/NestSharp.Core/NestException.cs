using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NestSharp.Core
{
    public class NestException : Exception
    {
        public HttpResponseMessage Response { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public NestException()
        {
        }

        public NestException(HttpStatusCode status)
        {
            StatusCode = status;
        }

        public NestException(HttpResponseMessage response) 
            : this(response.StatusCode)
        {
            Response = response;
        }

        public NestException(Exception ex)
            : base("Nest error occurred", ex)
        {
        }
    }
}
