using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Common
{
    public  class DateTimeProvider
    {
        public DateTime CurrentDateTime() => DateTime.UtcNow.AddHours(1);
    }
}
