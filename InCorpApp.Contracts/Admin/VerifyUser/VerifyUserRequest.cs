using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Admin.RemoveUser;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Admin.VerifyUser
{
    public class VerifyUserRequest: EmailRequest, IRequest<ResponseWrapper<VerifyUserResponse>>
    {

    }
}
