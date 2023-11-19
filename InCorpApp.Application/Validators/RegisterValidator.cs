using FluentValidation;
using InCorpApp.Contracts.Authentication.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Validators
{
    public class RegisterValidator: AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.FirstName).NotEmpty().NotNull();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}
