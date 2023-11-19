using FluentValidation;
using InCorpApp.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Validators
{
    public class SignedInUserValidator: AbstractValidator<SignedInUserRequest>
    {
        public SignedInUserValidator()
        {
            RuleFor(x => x.SignedInEmail).NotEmpty().NotNull();
        }
    }
}
