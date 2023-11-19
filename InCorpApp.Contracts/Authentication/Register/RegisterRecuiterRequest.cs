using InCorpApp.Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Authentication.Register
{
    public class RegisterRecuiterRequest: IRequest<ResponseWrapper<RegisterResponse>>
    {        
        public string CompanyRegistrationNumber { get; set; }

        public string CompanyEmail { get; set; }

        public string CompanyName { get; set; }

        public string Password { get; set; }
        
        
    }
}
