using InCorpApp.Contracts.Applicant.GetActiveJobs;
using InCorpApp.Contracts.Enums;
using InCorpApp.Contracts.Recruiter.CreateJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Recruiter.GetCreatedJobs
{
    public class GetCreatedJobsResponse: GetActiveJobsResponse
    {
        public List<Stage> Stages { get; set; }
        public int NoOfAppliedUsers { get; set; }
    }
    
}
