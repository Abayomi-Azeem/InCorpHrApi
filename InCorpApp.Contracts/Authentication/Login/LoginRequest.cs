using InCorpApp.Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Authentication.Login
{
    public class LoginRequest: IRequest<ResponseWrapper<LoginResponse>>
    {
        public string Email { get; set; }

        public  string  Password { get; set; }
    }
}
