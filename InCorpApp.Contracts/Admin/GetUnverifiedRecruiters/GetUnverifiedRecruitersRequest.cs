using InCorpApp.Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Admin.GetUnverifiedRecruiters
{
    public class GetUnverifiedRecruitersRequest : IRequest<ResponseWrapper<IEnumerable<GetUnverifiedRecruitersResponse>>>
    {
    }
}
