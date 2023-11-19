using InCorpApp.Contracts.Applicant.ApplyJob;
using InCorpApp.Contracts.Applicant.CreateProfile;
using InCorpApp.Contracts.Authentication.Register;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Recruiter.CreateJob;
using InCorpApp.Contracts.Recruiter.CreateRecruiterProfile;
using InCorpApp.Domain.Enums;
using InCorpApp.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.Entities
{
    public sealed class User
    {
        public User()
        {

        }
        private User(RegisterRequest userRequest, string hashedPassword, string salt)
        {
            Id = Guid.NewGuid();
            FirstName = userRequest.FirstName;
            LastName = userRequest.LastName;
            Email = userRequest.Email;
            Password = hashedPassword;
            PasswordSalt = salt;
            Role = Roles.Applicant;
            JobsApplied = new();
            CreatedDate = new DateTimeProvider().CurrentDateTime();
            IsVerified = true;
        }

        private User(RegisterRecuiterRequest userRequest, string hashedPassword, string salt)
        {
            Id = Guid.NewGuid();
            Email = userRequest.CompanyEmail;
            Password = hashedPassword;
            PasswordSalt = salt;
            Role = Roles.Recruiter;
            JobsCreated = new List<Job>();
            CreatedDate = new DateTimeProvider().CurrentDateTime();
            CompanyAddress = userRequest.CompanyName;
            CompanyRegNumber = userRequest.CompanyRegistrationNumber;
            IsVerified = false;
        }


        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string? CompanyRegNumber { get; init; }
        public string? CompanyAddress { get; init; }
        public string Password { get; init; }
        public string PasswordSalt { get; init; }
        public Roles Role { get; init; }
        public List<Job>? JobsCreated { get; init; }
        public List<AppliedJob>? JobsApplied { get; set; }
        public string? CVLink { get; init; }
        public DateTime CreatedDate { get; init; }
        public bool IsVerified { get; set; }
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
        public string? AboutUs { get; set; }
        public string? OrganizationType { get; set; }
        public string? IndustryType { get; set; }
        public int? TeamSize { get; set; }
        public int? YearsOfEstablishment { get; set; }
        public string? CompanyVision { get; set; }


        public static User CreateApplicant(RegisterRequest userRequest, string hashedPassword, string salt)
        {
            return new(userRequest, hashedPassword, salt);
        }

        public static User CreateRecruiter(RegisterRecuiterRequest userRequest, string hashedPassword, string salt)
        {
            return new(userRequest, hashedPassword, salt);
        }

        public void VerifyUser()
        {
            this.IsVerified = true;
        }

        public void CreateProfile(CreateProfileRequest request)
        {
            this.HighestEducation = request.HighestEducation is not null? request.HighestEducation:this.HighestEducation;
            this.PersonalWebsite = request.PersonalWebsite is not null? request.PersonalWebsite: this.PersonalWebsite;
            this.Nationality = request.Nationality is not null? request.Nationality: this.Nationality;
            this.DateOfBirth = request.DateOfBirth is not null? request.DateOfBirth: this.DateOfBirth;
            this.Gender = request.Gender is not null? request.Gender: this.Gender;
            this.MaritalSatus = request.MaritalSatus is not null? request.MaritalSatus: this.MaritalSatus;
            this.Biography = request.Biography is not null? request.Biography: this.Biography;
            this.FaceBookLink = request.FaceBookLink is not null? request.FaceBookLink: this.FaceBookLink;
            this.TwitterLink = request.TwitterLink is not null? request.TwitterLink: this.TwitterLink;
            this.LinkedInLink = request.LinkedInLink is not null ? request.LinkedInLink : this.LinkedInLink;
            this.InstagramLink = request.InstagramLink is not null? request.InstagramLink: this.InstagramLink;
        }

        public void CreateRecruiterProfile(CreateRecruiterProfileRequest request)
        {
            this.AboutUs = request.AboutUs is not null? request.AboutUs: this.AboutUs;
            this.OrganizationType = request.OrganizationType is not null? request.OrganizationType:this.OrganizationType;
            this.IndustryType = request.IndustryType is not null? request.IndustryType:this.IndustryType;
            this.TeamSize = request.TeamSize is not null? request.TeamSize: this.TeamSize;
            this.YearsOfEstablishment = request.YearsOfEstablishment is not null? request.YearsOfEstablishment: this.YearsOfEstablishment;
            this.CompanyVision = request.CompanyVision is not null? request.CompanyVision: this.CompanyVision;
        }

        public void CreateJob(CreateJobRequest request)
        {
            var job = Job.CreateJob(request);
            this.JobsCreated!.Add(job);
        }

        public void ApplicantApplyJob(AppliedJob job)
        {
            this.JobsApplied!.Add(job);
        }
    
        public void RecruiterApplyJob(Guid jobId, string applicantemail)
        {
            var job = this.JobsCreated!.Where(x => x.Id == jobId).First();
            job.RecruiterApplyJob(applicantemail);
        }    
    }
}
