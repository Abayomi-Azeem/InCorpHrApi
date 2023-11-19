using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Recruiter.CreateJob
{
    public class CreateJobRequest:SignedInUserRequest, IRequest<ResponseWrapper<CreateJobResponse>>
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Role { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public SalaryType SalaryStructure { get; set; }

        public DateOnly ExpirationDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public JobType JobType { get; set; }
        public List<string> JobBenefits { get; set; }
        public string Requirements { get; set; }
        public List<Stage> Stages { get; set; }
    }

    public class Stage
    {
        public StageType StageType { get; set; }
        public double NoOfDaysInStage { get; set; }
        public string StageProperties { get; set; }
        public int StageNumber { get; set; }
    }
}
