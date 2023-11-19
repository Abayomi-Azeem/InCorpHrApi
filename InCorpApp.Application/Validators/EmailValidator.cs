using FluentValidation;
using InCorpApp.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Validators
{
    public class EmailValidator: AbstractValidator<EmailRequest>
    {
        public EmailValidator()
        {
            RuleFor(x => x.Email).NotEmpty().NotNull();
        }
    }
}
