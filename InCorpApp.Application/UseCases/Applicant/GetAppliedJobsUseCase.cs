using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Applicant.ApplyJob;
using InCorpApp.Contracts.Applicant.GetAppliedJobs;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Applicant
{
    public class GetAppliedJobsUseCase : IRequestHandler<GetAppliedJobsRequest, ResponseWrapper<List<GetAppliedJobsResponse>>>
    {
        private readonly IRepository _repository;

        public GetAppliedJobsUseCase(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseWrapper<List<GetAppliedJobsResponse>>> Handle(GetAppliedJobsRequest request, CancellationToken cancellationToken)
        {
            var validated = new SignedInUserValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<List<GetAppliedJobsResponse>>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail);
            if (user is null)
            {
                return ResponseBuilder.Build<List<GetAppliedJobsResponse>>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.NOT_FOUND);
            }
            var response = user.ToGetAppliedJobs();
            return ResponseBuilder.Build<List<GetAppliedJobsResponse>>(response);
        }
    }
}
