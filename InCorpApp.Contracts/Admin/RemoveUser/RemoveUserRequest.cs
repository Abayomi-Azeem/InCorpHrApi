using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Admin.RemoveUser
{
    public class RemoveUserRequest: EmailRequest, IRequest<ResponseWrapper<RemoveUserResponse>>
    {

    }
}
