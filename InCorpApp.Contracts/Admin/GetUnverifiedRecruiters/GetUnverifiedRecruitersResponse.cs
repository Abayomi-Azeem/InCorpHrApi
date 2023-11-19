using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Admin.GetUnverifiedRecruiters
{
    public class GetUnverifiedRecruitersResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? CompanyRegNumber { get; set; }
        public string? CompanyAddress { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedDate { get; init; }
        public bool IsVerified { get; set; }
    }
}
