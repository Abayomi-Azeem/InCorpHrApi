using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Recruiter.GetCreatedJobs
{
    public class GetCreatedJobsRequest:SignedInUserRequest, IRequest<ResponseWrapper<List<GetCreatedJobsResponse>>>
    {
        
    }
}
