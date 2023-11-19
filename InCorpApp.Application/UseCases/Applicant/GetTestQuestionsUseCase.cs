using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Applicant.GetTestQuestions;
using InCorpApp.Contracts.Common;
using InCorpApp.Domain.Dtos;
using InCorpApp.Domain.ValueObjects;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Applicant
{
    public class GetTestQuestionsUseCase : IRequestHandler<GetTestQuestionsRequest, ResponseWrapper<GetTestQuestionsResponse>>
    {
        private readonly IRepository _repository;

        public GetTestQuestionsUseCase(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseWrapper<GetTestQuestionsResponse>> Handle(GetTestQuestionsRequest request, CancellationToken cancellationToken)
        {
            //get user, job and particular stage

            var poster = await _repository.GetById(request.JobPosterEmail);
            if (poster is null || poster.JobsCreated.Count < 0)
            {
                return ResponseBuilder.Build<GetTestQuestionsResponse>(null, System.Net.HttpStatusCode.NotFound, true, ResponseMessages.JOB_NOTFOUND);
            }
            var job = poster.JobsCreated!.Where(x => x.Id == request.JobId).FirstOrDefault();
            if (job is null)
            {
                return ResponseBuilder.Build<GetTestQuestionsResponse>(null, System.Net.HttpStatusCode.NotFound, true, ResponseMessages.JOB_NOTFOUND);
            }
            var stage = job.Stages.Where(x => x.Id == request.StageId).FirstOrDefault();
            if (stage is null)
            {
                return ResponseBuilder.Build<GetTestQuestionsResponse>(null, System.Net.HttpStatusCode.NotFound, true, ResponseMessages.JOB_NOTFOUND);
            }

            switch (stage.StageType)
            {
                case Contracts.Enums.StageType.CV:
                    stage.StageInfo = string.Empty;
                    break;
                case Contracts.Enums.StageType.PersonalityTest:
                    var personalityQuestions = AllPersonalityQuestion.PersonalityQuestions();
                    stage.StageInfo = JsonConvert.SerializeObject(personalityQuestions);
                    break;
                case Contracts.Enums.StageType.TechnicalTest:
                    TechnicalTest technicalTest = JsonConvert.DeserializeObject<TechnicalTest>(stage.StageInfo);
                    foreach (var question in technicalTest.Questions)
                    {
                        question.SetCorrectOptionToZero();
                    }
                    stage.StageInfo = JsonConvert.SerializeObject(technicalTest);
                    break;
                case Contracts.Enums.StageType.Interview:
                    stage.StageInfo = string.Empty;
                    break;
                default:
                    stage.StageInfo = string.Empty;
                    break;
            }

            var response = stage.ToGetTestQuestionsResponse();
            return ResponseBuilder.Build<GetTestQuestionsResponse>(response);

            //map stageInformationand returnstage Info
        }
    }
}
