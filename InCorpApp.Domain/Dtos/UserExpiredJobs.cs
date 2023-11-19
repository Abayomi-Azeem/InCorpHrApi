using InCorpApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Domain.Dtos
{
    public class UserExpiredJobs
    { 
        public User user { get; set; }
        public Guid JobId { get; set; }
    }
}
