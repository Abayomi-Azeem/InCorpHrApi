using InCorpApp.Application.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Applicant.GetTestQuestions
{
    public class GetTestQuestionsRequest: IRequest<ResponseWrapper<GetTestQuestionsResponse>>
    {
        public string JobPosterEmail { get; set; }

        public Guid JobId { get; set; }

        public Guid StageId { get; set; }
    }
}
