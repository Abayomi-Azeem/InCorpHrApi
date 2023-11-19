using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Utilities
{
    public sealed class ResponseWrapper<T> where T: class
    {
        public T? Data { get; set; }

        public bool HasError { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string? ActionMessage { get; set; }
    }
}
