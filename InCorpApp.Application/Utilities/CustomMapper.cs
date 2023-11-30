using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using InCorpApp.Contracts.Admin.GetAllUsers;
using InCorpApp.Contracts.Admin.GetUnverifiedRecruiters;
using InCorpApp.Contracts.Admin.GetUser;
using InCorpApp.Contracts.Applicant.GetActiveJobs;
using InCorpApp.Contracts.Applicant.GetAppliedJobs;
using InCorpApp.Contracts.Applicant.GetTestQuestions;
using InCorpApp.Contracts.Authentication.Login;
using InCorpApp.Contracts.Enums;
using InCorpApp.Contracts.Recruiter.CreateJob;
using InCorpApp.Contracts.Recruiter.GetCreatedJobs;
using InCorpApp.Domain.Entities;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stage = InCorpApp.Domain.Entities.Stage;

namespace InCorpApp.Application.Utilities
{
    public static class CustomMapper
    {
        public static LoginResponse ToLoginResponse(this User user, string accessToken, string refreshToken)
        {
            var response = new LoginResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString(),
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
            if (user.Role == Domain.Enums.Roles.Applicant)
            {
                user.JobsApplied.Select(x => x.StageAnswers == new List<Domain.ValueObjects.ApplicantAnswer>());
                response.Jobs = JsonConvert.SerializeObject(user.JobsApplied);
            }
            else
            {
                response.Jobs = JsonConvert.SerializeObject(user.JobsCreated);

            }
            return response;
        }
        
        public static GetUnverifiedRecruitersResponse ToUnverifiedRecruiters(this User user)
        {
            var response = new GetUnverifiedRecruitersResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString(),
                CompanyAddress = user.CompanyAddress,
                CompanyRegNumber = user.CompanyRegNumber,
                CreatedDate = user.CreatedDate,
                IsVerified = user.IsVerified
            };
            return response;
        }

        public static GetUserResponse ToGetUserResponse(this User user)
        {
            var response = new GetUserResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString(),
                CompanyAddress = user.CompanyAddress,
                CompanyRegNumber = user.CompanyRegNumber,
                CreatedDate = user.CreatedDate,
                IsVerified = user.IsVerified,                
            };
            if (user.Role == Domain.Enums.Roles.Applicant)
            {
                response.Jobs = JsonConvert.SerializeObject(user.JobsApplied);
            }
            else
            {
                response.Jobs = JsonConvert.SerializeObject(user.JobsCreated);

            }
            return response;
        }

        public static GetTestQuestionsResponse ToGetTestQuestionsResponse(this Stage stage)
        {
            var response = new GetTestQuestionsResponse()
            {
                Id = stage.Id,
                NoOfDaysInStage = stage.NoOfDaysInStage,
                StageNumber = stage.StageNumber,
                StageInfo = stage.StageInfo,
                StageType = stage.StageType
            };
            return response;
        }

        public static GetActiveJobsResponse ToGetActiveJobs(this Job job, string jobPosterEmail, string? companyName)
        {
                var response = new GetActiveJobsResponse()
                {
                    Id = job.Id,
                    Description = job.Description,
                    Title = job.Title,
                    Tags = job.Tags,
                    Role = job.Role,
                    MinSalary = job.MinSalary,
                    MaxSalary = job.MaxSalary,
                    SalaryStructure = job.SalaryStructure.ToString(),
                    ExpirationDate = job.ExpirationDate,
                    Country = job.Country,
                    JobType = job.JobType.ToString(),
                    JobBenefits = job.JobBenefits,
                    Requirements = job.Requirements,
                    Status = job.Status,
                    JobPosterEmail = jobPosterEmail,
                    CompanyName = companyName,
                    DateCreated = job.DateCreated,
                    City = job.City
                };
            return response;
        }
    
        public static List<GetAllUsersResponse> ToGetAllUsers(this IEnumerable<User> users)
        {
            List<GetAllUsersResponse> response = new();
            foreach (var user in users)
            {
                GetAllUsersResponse result = new()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    CompanyAddress = user.CompanyAddress,
                    CompanyRegNumber = user.CompanyRegNumber,
                    CreatedDate = user.CreatedDate,
                    IsVerified = user.IsVerified
                };

                response.Add(result);
            }
            return response;
        }

        public static List<GetCreatedJobsResponse> ToGetCreatedJobs (this User user, string jobPosterEmail, string companyName)
        {
            var response = new List<GetCreatedJobsResponse>();
            foreach (var job in user.JobsCreated)
            {
                var createdjob = new GetCreatedJobsResponse
                {
                    Id = job.Id,
                    Description = job.Description,
                    Title = job.Title,
                    Tags = job.Tags,
                    Role = job.Role,
                    MinSalary = job.MinSalary,
                    MaxSalary = job.MaxSalary,
                    SalaryStructure = job.SalaryStructure.ToString(),
                    ExpirationDate = job.ExpirationDate,
                    Country = job.Country,
                    JobType = job.JobType.ToString(),
                    JobBenefits = job.JobBenefits,
                    Requirements = job.Requirements,
                    Status = job.Status,
                    JobPosterEmail = jobPosterEmail,
                    CompanyName = companyName,
                    DateCreated = job.DateCreated,
                    City = job.City,
                    NoOfAppliedUsers = job.UsersApplied.Count,
                    Stages = job.Stages.Select(x => new Contracts.Recruiter.CreateJob.Stage  { 
                        StageType = x.StageType,
                        NoOfDaysInStage = x.NoOfDaysInStage,
                        StageNumber = x.StageNumber,
                        StageProperties = x.StageInfo
                    }).ToList()
                };
                response.Add(createdjob);
            }
            return response;
        }

        public static List<GetAppliedJobsResponse> ToGetAppliedJobs(this User user)
        {
            var response = new List<GetAppliedJobsResponse>();
            foreach (var job in user.JobsApplied)
            {
                var createdjob = new GetAppliedJobsResponse
                {
                   JobPosterEmail = job.JobPosterEmail,
                   JobId = job.JobId,
                   CurrentStageInJob = job.CurrentStageInJob,
                   DateApplied = job.DateApplied,
                   Description = job.Description,
                   Title = job.Title,
                   Role = job.Role,
                   MinSalary = job.MinSalary,
                   MaxSalary = job.MaxSalary,
                   SalaryStructure = job.SalaryStructure,
                   City = job.City,
                   CurrentStageId = job.CurrentStageId
                };
                response.Add(createdjob);
            }
            return response;
        }
    }
}
