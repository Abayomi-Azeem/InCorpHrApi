using InCorpApp.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Applicant.GetAppliedJobs
{
    public class GetAppliedJobsResponse
    {
        public string JobPosterEmail { get; set; }
        public Guid JobId { get; set; }
        public int CurrentStageInJob { get; set; }
        public DateTime DateApplied { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Role { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public SalaryType SalaryStructure { get; set; }
        public string City { get; set; }
        public Guid CurrentStageId { get; set; }
    }
}
