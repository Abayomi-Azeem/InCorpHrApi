using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.ValueObjects
{
    public class TechnicalTest
    {
        public TimeSpan TestDuration { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public int PassMark { get; set; }
    }
}
