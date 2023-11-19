using InCorpApp.Application.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Authentication.Register
{
    public class RegisterRequest: IRequest<ResponseWrapper<RegisterResponse>>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        //public IFormFile CV { get; set; }

    }
}
