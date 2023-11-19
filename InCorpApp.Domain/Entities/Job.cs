using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Enums;
using InCorpApp.Contracts.Recruiter.CreateJob;
using InCorpApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.Entities
{
    public sealed class Job
    {
        public Job()
        {

        }
        public Job(CreateJobRequest request)
        {
            Id = Guid.NewGuid();
            Description = request.Description;
            Title = request.Title;
            Tags = request.Tags;
            Role = request.Role;
            MinSalary = request.MinSalary;
            MaxSalary = request.MaxSalary;
            SalaryStructure = request.SalaryStructure;
            ExpirationDate = request.ExpirationDate;
            Country = request.Country;
            City = request.City;
            JobType = request.JobType;
            JobBenefits = request.JobBenefits;
            Requirements = request.Requirements;
            DateCreated = new DateTimeProvider().CurrentDateTime();
            Status = JobStatus.Active.ToString();
            CurrentJobStage = 0;
            UsersApplied = new();
        }

        public Guid Id { get; init; }
        public string Description { get; init; }
        public string Title { get; init; }
        public string Tags { get; init; }
        public string Role { get; init; }
        public decimal MinSalary { get; init; }
        public decimal MaxSalary { get; init; }
        public SalaryType SalaryStructure { get; init; }
        public DateOnly ExpirationDate { get; init; }
        public string Country { get; init; }
        public string City { get; init; }
        public JobType JobType { get; init; }
        public List<string> JobBenefits { get; init; }
        public string Requirements { get; init; }
        public List<Stage> Stages { get; init; } = new List<Stage>();
        public DateTime DateCreated { get; init; }
        public string Status { get; set; }
        public int CurrentJobStage { get; set; }
        public List<string> UsersApplied { get; set; }


        public static Job CreateJob(CreateJobRequest request)
        {
            Job job = new(request);
            foreach (var stage in request.Stages)
            {
                var createdStage = Stage.CreateStage(stage);
                job.Stages.Add(createdStage);
            }
            return job;
        }
    
        public void RecruiterApplyJob(string applicantemail)
        {
            this.UsersApplied.Add(applicantemail);
        }

        public void IncreaseJobSate()
        {
            this.CurrentJobStage += 1;
        }

        public void UpdateJobStatus(JobStatus status)
        {
            this.Status = status.ToString();
        }

        public bool ExhaustedNoOfDaysInStage(int stageNo)
        {
            var stages = this.Stages.OrderBy(x => x.Id).ToList();
            var totalTimeSpanSinceExpiration = new DateTimeProvider().CurrentDateTime() - this.ExpirationDate.ToDateTime(TimeOnly.MinValue);
            var totalDaysSinceExpiration = (double)totalTimeSpanSinceExpiration.Days;
            for (int i = 0; i < stages.Count(); i++)
            {
                totalDaysSinceExpiration -= stages[i].NoOfDaysInStage;
                if (stages[i].StageNumber == stageNo)
                {
                    break;
                }
            }
            return totalDaysSinceExpiration == 0 ? true : false;
        }

        public DateOnly GetStageEndDate(Guid stageId)
        {
            var stages = this.Stages.OrderBy(x => x.Id).ToList();
            Double totalDays= default;
            for (int i = 0; i < stages.Count(); i++)
            {

                totalDays += stages[i].NoOfDaysInStage;
                if (stages[i].Id == stageId)
                {
                    break;
                }
            }
            var noOfDayselasped = Convert.ToInt16(totalDays);
            var stageEndDate = this.ExpirationDate.AddDays(noOfDayselasped);
            return stageEndDate;
        }
    }
}
