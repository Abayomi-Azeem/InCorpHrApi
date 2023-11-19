using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.ValueObjects
{
    public class CVScan
    {
        public string ExperienceLevel { get; set; }
        public string YearOfExperience { get; set; }
        public string Tools { get; set; }
        public List<string> Keywords { get; set; }
    }
}
