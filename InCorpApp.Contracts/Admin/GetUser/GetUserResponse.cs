using InCorpApp.Contracts.Admin.GetUnverifiedRecruiters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Admin.GetUser
{
    public class GetUserResponse: GetUnverifiedRecruitersResponse
    {
        public string Jobs { get; set; }
        public Dictionary<long, long>? JobStages { get; set; }
    }
}
