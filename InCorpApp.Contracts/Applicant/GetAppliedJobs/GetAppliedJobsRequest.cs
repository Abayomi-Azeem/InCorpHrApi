using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Applicant.GetAppliedJobs
{
    public class GetAppliedJobsRequest:SignedInUserRequest, IRequest<ResponseWrapper<List<GetAppliedJobsResponse>>>
    {

    }
}
