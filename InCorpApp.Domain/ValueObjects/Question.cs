using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.ValueObjects
{
    public class Question
    {
        public string QuestionValue { get; set; }
        public int QuestionId { get; set; }
        public List<Option> Options { get; set; }
        public int CorrectOptionId { get; set; }

        public void SetCorrectOptionToZero()
        {
            this.CorrectOptionId = 0;
        }
    }
}
