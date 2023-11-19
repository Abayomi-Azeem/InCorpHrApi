using InCorpApp.Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Admin.GetUser
{
    public class GetUserRequest: IRequest<ResponseWrapper<IEnumerable<GetUserResponse>>>
    {
        public SearchUserBy SearchParam { get; set; }

        public string Value { get; set; }
    }

    public enum SearchUserBy
    {
        Email,
        CompanyRegNumber,
        FirstName,
        LastName,
        Role
    }
}
