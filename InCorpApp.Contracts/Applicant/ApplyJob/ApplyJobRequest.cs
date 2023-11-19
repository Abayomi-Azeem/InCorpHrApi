using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Applicant.ApplyJob
{
    public class ApplyJobRequest: SignedInUserRequest, IRequest<ResponseWrapper<ApplyJobResponse>>
    {
        public string JobPosterEmail { get; set; }

        public Guid JobId { get; set; }
    }
}
