using FluentValidation;
using InCorpApp.Contracts.Recruiter.CreateJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Validators
{
    public class CreateJobValidator: AbstractValidator<CreateJobRequest>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.Tags).NotNull().NotEmpty();
            RuleFor(x => x.Role).NotNull().NotEmpty();
            RuleFor(x => x.SalaryStructure).IsInEnum().NotNull();
            RuleFor(x => x.ExpirationDate).NotNull().NotEmpty();
            RuleFor(x => x.Country).NotNull().NotEmpty();
            RuleFor(x => x.City).NotEmpty().NotNull();
            RuleFor(x => x.JobType).IsInEnum().NotNull();
            RuleFor(x => x.JobBenefits).NotNull().NotEmpty();
            RuleFor(x => x.Requirements).NotEmpty().NotNull();
            RuleFor(x => x.SignedInEmail).NotNull().NotEmpty();
        }
    }
}
