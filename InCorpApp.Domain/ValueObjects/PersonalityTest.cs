using InCorpApp.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.ValueObjects
{
    public class PersonalityTest
    {
        public TimeSpan TestDuration { get; set; }
        
        public IEnumerable<PersonalityTypes> PreferredPersonalityTypes { get; set; }

    }
}
