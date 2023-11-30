using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Applicant.ApplyJob;
using InCorpApp.Contracts.Applicant.GetTestQuestions;
using InCorpApp.Contracts.Applicant.SubmitTest;
using InCorpApp.Contracts.Common;
using InCorpApp.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Applicant
{
    public class SubmitTestUseCase : IRequestHandler<SubmitTestRequest, ResponseWrapper<SubmitTestResponse>>
    {
        private readonly IRepository _repository;

        public SubmitTestUseCase(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseWrapper<SubmitTestResponse>> Handle(SubmitTestRequest request, CancellationToken cancellationToken)
        {
            var validated = new SignedInUserValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<SubmitTestResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail);
            if (user is null)
            {
                return ResponseBuilder.Build<SubmitTestResponse>(null, System.Net.HttpStatusCode.NotFound, true, ResponseMessages.JOB_NOTFOUND);
            }
            var poster = await _repository.GetById(request.JobPosterEmail);
            if (poster is null || poster.JobsCreated.Count > 0)
            {
                return ResponseBuilder.Build<SubmitTestResponse>(null, System.Net.HttpStatusCode.NotFound, true, ResponseMessages.JOB_NOTFOUND);
            }
            var job = poster.JobsCreated!.Where(x => x.Id == request.JobId).FirstOrDefault();
            if (job is null)
            {
                return ResponseBuilder.Build<SubmitTestResponse>(null, System.Net.HttpStatusCode.NotFound, true, ResponseMessages.JOB_NOTFOUND);
            }
            var stage = job.Stages.Where(x => x.Id == request.StageId).FirstOrDefault();
            if (stage is null)
            {
                return ResponseBuilder.Build<SubmitTestResponse>(null, System.Net.HttpStatusCode.NotFound, true, ResponseMessages.JOB_NOTFOUND);
            }
            var appliedJob = user.JobsApplied.Where(x => x.JobId == request.JobId).First();
            var previousSubmission = appliedJob.StageAnswers.Where(x => x.StageId == request.StageId).FirstOrDefault();
            if (previousSubmission is not null)
            {
                return ResponseBuilder.Build<SubmitTestResponse>(null, System.Net.HttpStatusCode.NotFound, true, ResponseMessages.JOB_NOTFOUND);
            }
            var answer = ApplicantAnswer.Create(request);
            appliedJob.SubmitTest(answer);
            await _repository.UpdateAsync(user);
            return ResponseBuilder.Build<SubmitTestResponse>();
            
        }
    }
}
