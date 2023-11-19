using InCorpApp.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Applicant.GetTestQuestions
{
    public class GetTestQuestionsResponse
    {
        public StageType StageType { get; set; }
        public Guid Id { get; set; }
        public Double NoOfDaysInStage { get; set; }
        public string StageInfo { get; set; }
        public int StageNumber { get; set; }
    }
}
