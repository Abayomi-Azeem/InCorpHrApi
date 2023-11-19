using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Applicant.SubmitTest
{
    public class SubmitTestRequest: SignedInUserRequest, IRequest<ResponseWrapper<SubmitTestResponse>>
    {
        
        public string JobPosterEmail { get; set; }
        public Guid JobId { get; set; }
        public Guid StageId { get; set; }

        public IEnumerable<KeyValuePair<int, int>> QuestionOptions { get; set; }
    }
}
