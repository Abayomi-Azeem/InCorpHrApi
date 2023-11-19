using FluentValidation;
using InCorpApp.Contracts.Authentication.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Validators
{
    public class RegisterRecuiterValidator: AbstractValidator<RegisterRecuiterRequest>
    {
        public RegisterRecuiterValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().NotNull();
            RuleFor(x => x.CompanyEmail).NotEmpty().NotNull();
            RuleFor(x => x.CompanyRegistrationNumber).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
