using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Applicant.ApplyJob;
using InCorpApp.Contracts.Applicant.CreateProfile;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Recruiter.CreateJob;
using InCorpApp.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Applicant
{
    public class ApplyJobUseCase : IRequestHandler<ApplyJobRequest, ResponseWrapper<ApplyJobResponse>>
    {
        private readonly IRepository _repository;

        public ApplyJobUseCase(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseWrapper<ApplyJobResponse>> Handle(ApplyJobRequest request, CancellationToken cancellationToken)
        {
            var validated = new SignedInUserValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<ApplyJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail);
            if (user is null)
            {
                return ResponseBuilder.Build<ApplyJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.NOT_FOUND);
            }
            var jobPoster = await _repository.GetById(request.JobPosterEmail);
            if (jobPoster is null || jobPoster.Role != Domain.Enums.Roles.Recruiter)
            {
                return ResponseBuilder.Build<ApplyJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.JOB_NOTFOUND);
            }
            var job = jobPoster.JobsCreated!.Where(x => x.Id == request.JobId).FirstOrDefault();
            if (job is null)
            {
                return ResponseBuilder.Build<ApplyJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.JOB_NOTFOUND);
            }
            if (job.ExpirationDate < DateOnly.FromDateTime(new DateTimeProvider().CurrentDateTime().Date))
            {
                return ResponseBuilder.Build<ApplyJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.JOB_EXPIRED);
            }
            var appliedJob = AppliedJob.Create(request, job);
            user.ApplicantApplyJob(appliedJob);
            jobPoster.RecruiterApplyJob(request.JobId, user.Email);
            var isUpdated = await _repository.UpdateAsync(user);
            var isrecruiterUpdated = await _repository.UpdateAsync(jobPoster);

            if (isUpdated && isrecruiterUpdated)
            {
                return ResponseBuilder.Build<ApplyJobResponse>();
            }
            return ResponseBuilder.Build<ApplyJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.FAILURE);

        }
    }
}
