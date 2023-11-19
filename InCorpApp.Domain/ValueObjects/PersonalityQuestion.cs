using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.ValueObjects
{
    public class PersonalityQuestion
    {
        public string QuestionValue { get; set; }
        public int QuestionId { get; set; }
        public IEnumerable<PersonalityTestOptions> Options { get; set; }

        
    }
}
