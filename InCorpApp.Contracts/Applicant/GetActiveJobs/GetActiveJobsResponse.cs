using InCorpApp.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Applicant.GetActiveJobs
{
    public class GetActiveJobsResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Role { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public string SalaryStructure { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string JobType { get; set; }
        public List<string> JobBenefits { get; set; }
        public string Requirements { get; set; }
        public string Status { get; set; }
    }
}
