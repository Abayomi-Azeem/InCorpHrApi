using InCorpApp.Contracts.Enums;
using InCorpApp.Contracts.Recruiter.CreateJob;
using InCorpApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestStage = InCorpApp.Contracts.Recruiter.CreateJob.Stage;

namespace InCorpApp.Domain.Entities
{
    public sealed class Stage
    {
        public Stage()
        {

        }
        public Stage(RequestStage request)
        {
            StageType = request.StageType;
            Id = Guid.NewGuid();
            NoOfDaysInStage = request.NoOfDaysInStage;
            StageInfo = request.StageProperties;
            StageNumber = request.StageNumber;
            StageStatus = JobStageStatus.Pending;
        }

        public StageType StageType { get; init; }
        public Guid Id { get; init; }
        public double NoOfDaysInStage { get; set; }
        public string StageInfo { get; set; }
        public int StageNumber { get; set; }
        public JobStageStatus StageStatus { get; set; }

        public static Stage CreateStage(RequestStage request)
        {
            return new(request);
        }

        public void UpdateStageNumber(int number)
        {
            this.StageNumber = number;
        }

        public void UpdateJobStageStatus(JobStageStatus status)
        {
            this.StageStatus = status;
        }
    }

}
