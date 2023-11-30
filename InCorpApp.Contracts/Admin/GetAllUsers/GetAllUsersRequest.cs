using InCorpApp.Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Admin.GetAllUsers
{
    public class GetAllUsersRequest: IRequest<ResponseWrapper<List<GetAllUsersResponse>>>
    {

    }
}
