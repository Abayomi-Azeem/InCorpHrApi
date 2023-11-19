using FluentValidation;
using InCorpApp.Contracts.Admin.GetUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Validators
{
    public class GetUserValidator: AbstractValidator<GetUserRequest>
    {
        public GetUserValidator()
        {
            RuleFor(x => x.SearchParam).NotNull().IsInEnum();
            RuleFor(x => x.Value).NotNull().NotEmpty();
        }
    }
}
