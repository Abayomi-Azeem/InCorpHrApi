using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Applicant.CreateProfile
{
    public class CreateProfileRequest: SignedInUserRequest, IRequest<ResponseWrapper<CreateProfileResponse>>
    {        
        public string? HighestEducation { get; set; }
        public string? PersonalWebsite { get; set; }
        public string? Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? MaritalSatus { get; set; }
        public string? Biography { get; set; }
        public string? FaceBookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? LinkedInLink { get; set; }
        public string? InstagramLink { get; set; }        

    }
}
