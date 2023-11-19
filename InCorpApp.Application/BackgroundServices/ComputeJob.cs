using InCorpApp.Application.Abstractions.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InCorpApp.Contracts.Enums;
using InCorpApp.Domain.Entities;
using Newtonsoft.Json;
using InCorpApp.Domain.ValueObjects;
using static System.Formats.Asn1.AsnWriter;
using InCorpApp.Application.Abstractions.Notification;
using Microsoft.Extensions.Logging;
using InCorpApp.Contracts.Common;
using InCorpApp.Application.Abstractions.AWS;

namespace InCorpApp.Application.BackgroundServices
{
    public class ComputeJob : IJob
    {
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;
        private readonly ILogger<ComputeJob> _logger;
        private readonly IS3Manager _s3Manager;
        public ComputeJob(IRepository repository, IEmailService emailService, ILogger<ComputeJob> logger, IS3Manager s3Manager)
        {
            _repository = repository;
            _emailService = emailService;
            _logger = logger;
            _s3Manager = s3Manager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //get all recruiter that have jobs that have expired
            //
            _logger.LogInformation($"[ComputeJob] - Starting Job at - {new DateTimeProvider().CurrentDateTime()}");
            var recruiters = await _repository.GetRecruitersWithExpiredJobs();
            foreach (var recruiter in recruiters)
            {
                var expiredJob = recruiter.user.JobsCreated!.Where(x => x.Id == recruiter.JobId).First();

                if (expiredJob.CurrentJobStage == 0)
                {
                    expiredJob.UpdateJobStatus(JobStatus.OngoingRecruitment);
                    expiredJob.CurrentJobStage += 1;
                }
                if (expiredJob.Status != JobStatus.OngoingRecruitment.ToString())
                {
                    continue;
                }

                var expiredJobStage = expiredJob.Stages.Where(x => x.StageNumber == expiredJob.CurrentJobStage).First();
                if (expiredJob.ExhaustedNoOfDaysInStage(expiredJobStage.StageNumber))
                {
                    expiredJob.IncreaseJobSate();
                }
                int currentJobStage = expiredJob.CurrentJobStage;
                if (currentJobStage > expiredJob.Stages.Count())
                {
                    expiredJob.UpdateJobStatus(JobStatus.Done);
                    await _repository.UpdateAsync(recruiter.user);
                    continue;
                }
                foreach (var applicantemail in expiredJob.UsersApplied)
                {
                    var applicant = await _repository.GetById(applicantemail);
                    if (applicant is null)
                    {
                        continue;
                    }
                    
                    
                    Stage currentStage = expiredJob.Stages.Where(x => x.StageNumber == currentJobStage).First();
                    var computedJob = currentStage.StageType switch
                    {
                        StageType.CV => ComputeCVTest(applicant,expiredJob, currentStage),
                        StageType.PersonalityTest => ComputePersonalityTest(applicant,expiredJob, currentStage),
                        StageType.TechnicalTest => ComputeTechnicalTest(applicant, expiredJob,currentStage),
                        StageType.Interview => throw new NotImplementedException(),
                        _ => throw new NotImplementedException(),
                    };

                    await _repository.UpdateAsync(applicant);
                    

                }
            }
        }

        public User ComputeCVTest(User applicant, Job job, Stage currentStage)
        {
            var applicantJob = applicant.JobsApplied.Where(x => x.JobId == job.Id && x.CurrentStageInJob == job.CurrentJobStage).FirstOrDefault();
            if (applicantJob is null)
            {
                //user did not apply for job
                return applicant;
            }
            //var userCv = _s3Manager.GetFile(applicant.Email, Contracts.Shared.UploadFile.UploadedFileCat.CV);
            //var tags = JsonConvert.DeserializeObject<CVScan>(currentStage.StageInfo);
            throw new NotImplementedException();



        }

        public User ComputePersonalityTest(User applicant, Job job, Stage currentStage)
        {
            var applicantJob = applicant.JobsApplied.Where(x => x.JobId == job.Id && x.CurrentStageInJob==job.CurrentJobStage).FirstOrDefault();
            if (applicantJob is null)
            {
                //user did not apply for job
                return applicant;
            }
            var questions = JsonConvert.DeserializeObject<PersonalityTest>(currentStage.StageInfo);
            var applicantAns = applicantJob.StageAnswers.Where(x => x.StageId == currentStage.Id).First();
            if (questions is null )
            {
                //invalid stage
            }
            var preferredPersonality = new StringBuilder();
            int score = 0;
            foreach (KeyValuePair<int, int> ans in applicantAns.QuestionOptions)
            {
                preferredPersonality.Append(ans.Value);
            }
            foreach (var item in questions!.PreferredPersonalityTypes)
            {
                if (item.ToString() == preferredPersonality.ToString())
                {
                    score += 1;
                }
            }
            applicantAns.UpdateScore(score);
            if (score > 0 )
            {
                applicantJob.UpdateStage();
                var nextLoginDate = job.GetStageEndDate(currentStage.Id).ToShortDateString();
                _emailService.SendSuccessEmail(applicant.FirstName, applicant.Email, currentStage.StageType.ToString(), nextLoginDate);
                //move stage and send success email
            }
            //send failure email
            _emailService.SendFailureMail(applicant.FirstName, applicant.Email, currentStage.StageType.ToString());
            return applicant;
        }
        public User ComputeTechnicalTest(User applicant, Job job, Stage currentStage)
        {
            var applicantJob = applicant.JobsApplied.Where(x => x.JobId == job.Id && x.CurrentStageInJob == job.CurrentJobStage).FirstOrDefault();
            if (applicantJob is null)
            {
                //user did not apply for job
                return applicant;
            }
            var questions = JsonConvert.DeserializeObject<TechnicalTest>(currentStage.StageInfo);
            var applicantAns = applicantJob.StageAnswers.Where(x => x.StageId == currentStage.Id).First();
            if (questions is null)
            {
                //invalid stage
            }
            int score = 0;
            foreach (KeyValuePair<int,int> ans in applicantAns.QuestionOptions)
            {
                var question = questions.Questions.Where(x => x.QuestionId == ans.Key).First();
                if (question.CorrectOptionId == ans.Value)
                {
                    score += 1;
                }
            }
            applicantAns.UpdateScore(score);
            if (score >= questions.PassMark)
            {
                applicantJob.UpdateStage();
                var nextLoginDate = job.GetStageEndDate(currentStage.Id).ToShortDateString();
                _emailService.SendSuccessEmail(applicant.FirstName, applicant.Email, currentStage.StageType.ToString(), nextLoginDate);
            }
            //send failure email
            _emailService.SendFailureMail(applicant.FirstName, applicant.Email, currentStage.StageType.ToString());
            return applicant;
        }
        
    }
}
