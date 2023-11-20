using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Applicant.ApplyJob;
using InCorpApp.Contracts.Applicant.GetActiveJobs;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Applicant
{
    public class GetActiveJobsUseCase : IRequestHandler<GetActiveJobsRequest, ResponseWrapper<List<GetActiveJobsResponse>>>
    {
        private readonly IRepository _repository;

        public GetActiveJobsUseCase(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseWrapper<List<GetActiveJobsResponse>>> Handle(GetActiveJobsRequest request, CancellationToken cancellationToken)
        {
            var validated = new SignedInUserValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<List<GetActiveJobsResponse>>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail);
            if (user is null)
            {
                return ResponseBuilder.Build<List<GetActiveJobsResponse>>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.NOT_FOUND);
            }

            var jobs = await _repository.GetAllUnExpiredJobs();
            var response = jobs.ToList().ToGetActiveJobs();
            return ResponseBuilder.Build<List<GetActiveJobsResponse>>(response);
        }
    }
}
