using InCorpApp.Application.Abstractions.Notification;
using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Admin.RemoveUser;
using InCorpApp.Contracts.Admin.VerifyUser;
using InCorpApp.Contracts.Applicant.CreateProfile;
using InCorpApp.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Applicant
{
    public class CreateProfileUseCase : IRequestHandler<CreateProfileRequest, ResponseWrapper<CreateProfileResponse>>
    {
        private readonly IRepository _repository;

        public CreateProfileUseCase(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseWrapper<CreateProfileResponse>> Handle(CreateProfileRequest request, CancellationToken cancellationToken)
        {
            
            var validated = new SignedInUserValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<CreateProfileResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail);
            if (user is null)
            {
                return ResponseBuilder.Build<CreateProfileResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.INVALID_EMAIL_PASS);
            }
            user.CreateProfile(request);
            var isUpdated = await _repository.UpdateAsync(user);

            if (isUpdated)
            {
                return ResponseBuilder.Build<CreateProfileResponse>();
            }
            return ResponseBuilder.Build<CreateProfileResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.FAILURE);

        }
    }
}
