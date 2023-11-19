using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Recruiter.CreateRecruiterProfile
{
    public class CreateRecruiterProfileRequest: SignedInUserRequest, IRequest<ResponseWrapper<CreateRecruiterProfileResponse>>
    {
        public string? AboutUs { get; set; }
        public string? OrganizationType { get; set; }
        public string? IndustryType { get; set; }
        public int? TeamSize { get; set; }
        public int? YearsOfEstablishment { get; set; }
        public string? CompanyVision { get; set; }
        public string? FaceBookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? LinkedInLink { get; set; }
        public string? InstagramLink { get; set; }

    }
}
