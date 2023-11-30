using Amazon.Runtime.Internal.Util;
using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Applicant.ApplyJob;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Recruiter.GetCreatedJobs;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Recruiter
{
    public class GetCreatedJobsUseCase : IRequestHandler<GetCreatedJobsRequest, ResponseWrapper<List<GetCreatedJobsResponse>>>
    {
        private readonly IRepository _repository;
        private readonly ILogger<GetCreatedJobsUseCase> _logger;

        public GetCreatedJobsUseCase(IRepository repository, ILogger<GetCreatedJobsUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResponseWrapper<List<GetCreatedJobsResponse>>> Handle(GetCreatedJobsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[GetCreatedJobsUseCase] - Request Arrived - {request.SignedInEmail}");
            var validated = new SignedInUserValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<List<GetCreatedJobsResponse>>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail);
            if (user is null)
            {
                return ResponseBuilder.Build<List<GetCreatedJobsResponse>>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.NOT_FOUND);
            }
            var response = user.ToGetCreatedJobs(user.Email, user.CompanyAddress);
            return ResponseBuilder.Build<List<GetCreatedJobsResponse>>(response);
        }
    }
}
