using InCorpApp.Contracts.Applicant.ApplyJob;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Enums;
using InCorpApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.ValueObjects
{
    public class AppliedJob
    {
        public AppliedJob(ApplyJobRequest request, Job job)
        {
            JobPosterEmail = request.JobPosterEmail;
            JobId = request.JobId;
            CurrentStageInJob = 0;
            DateApplied = new DateTimeProvider().CurrentDateTime();
            Description = job.Description;
            Title = job.Title;
            Role = job.Role;
            MinSalary = job.MinSalary;
            MaxSalary = job.MaxSalary;
            SalaryStructure = job.SalaryStructure;
            StageAnswers = new List<ApplicantAnswer>();
            City = job.City;
        }

        public AppliedJob()
        {

        }

        public string JobPosterEmail { get; init; }
        public Guid JobId { get; init; }
        public int CurrentStageInJob { get; set; }
        public DateTime DateApplied { get; init; }
        public string Description { get; init; }
        public string Title { get; init; }
        public string Role { get; init; }
        public decimal MinSalary { get; init; }
        public decimal MaxSalary { get; init; }
        public SalaryType SalaryStructure { get; init; }
        public List<ApplicantAnswer> StageAnswers { get; set; } 
        public string City { get; set; }

        public static AppliedJob Create(ApplyJobRequest request ,Job job)
        {
            return new(request, job);
        }

        public void UpdateStage()
        {
            this.CurrentStageInJob += 1;
        }

        public void SubmitTest(ApplicantAnswer answer)
        {
            this.StageAnswers.Add(answer);
        }
    }
}
