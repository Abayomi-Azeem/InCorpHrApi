using FluentValidation;
using InCorpApp.Contracts.Applicant.ApplyJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Validators
{
    public class ApplyJobValidator: AbstractValidator<ApplyJobRequest>
    {
        public ApplyJobValidator()
        {
            RuleFor(x => x.SignedInEmail).NotNull().NotEmpty();
            RuleFor(x => x.JobId).NotNull().NotEmpty();
            RuleFor(x => x.SignedInEmail).NotNull().NotEmpty();
        }
    }
}
