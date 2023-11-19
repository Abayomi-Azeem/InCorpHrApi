using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Utilities
{
    public static class ResponseBuilder
    {
        public static ResponseWrapper<T> Build<T>(
                T? data = null,
                HttpStatusCode  statusCode= HttpStatusCode.OK,
                bool hasError = false,
                string actionMessage = "Success"
            ) where T: class
        
        {
            return new ResponseWrapper<T>
            {
                Data = data,
                HttpStatusCode = statusCode,
                HasError = hasError,
                ActionMessage = actionMessage
            };
        }
    }
}
