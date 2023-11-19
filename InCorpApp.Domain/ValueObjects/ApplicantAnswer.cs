using InCorpApp.Contracts.Applicant.SubmitTest;
using InCorpApp.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InCorpApp.Domain.ValueObjects
{
    public class ApplicantAnswer
    {
        public ApplicantAnswer()
        {

        }

        public ApplicantAnswer(SubmitTestRequest answer)
        {
            StageId = answer.StageId;
            QuestionOptions = answer.QuestionOptions;
            DateSubmitted = new DateTimeProvider().CurrentDateTime();
            Score = 0;
        }

        public Guid StageId { get; init; }
        
        public IEnumerable<KeyValuePair<int, int>> QuestionOptions { get; init; }
        public DateTime DateSubmitted { get; init; }
        public int Score { get; set; }

        public static ApplicantAnswer Create(SubmitTestRequest answer)
        {
            return new(answer);
        }

        public void UpdateScore(int score)
        {
            this.Score = score;
        }
    }
}
