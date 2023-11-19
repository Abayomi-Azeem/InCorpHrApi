using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Applicant.CreateProfile;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Recruiter.CreateRecruiterProfile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Recruiter
{
    public class CreateRecruiterProfileUseCase : IRequestHandler<CreateRecruiterProfileRequest, ResponseWrapper<CreateRecruiterProfileResponse>>
    {
        private readonly IRepository _repository;

        public CreateRecruiterProfileUseCase(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseWrapper<CreateRecruiterProfileResponse>> Handle(CreateRecruiterProfileRequest request, CancellationToken cancellationToken)
        {
            var validated = new SignedInUserValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<CreateRecruiterProfileResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail);
            if (user is null)
            {
                return ResponseBuilder.Build<CreateRecruiterProfileResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.INVALID_EMAIL_PASS);
            }
            user.CreateRecruiterProfile(request);
            var isUpdated = await _repository.UpdateAsync(user);

            if (isUpdated)
            {
                return ResponseBuilder.Build<CreateRecruiterProfileResponse>();
            }
            return ResponseBuilder.Build<CreateRecruiterProfileResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.FAILURE);

        }
    }
}
