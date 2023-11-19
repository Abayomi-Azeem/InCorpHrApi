using InCorpApp.Application.Abstractions.Notification;
using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Admin.VerifyUser;
using InCorpApp.Contracts.Authentication.Register;
using InCorpApp.Contracts.Common;
using InCorpApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Admin
{
    public class VerifyUserUseCase : IRequestHandler<VerifyUserRequest, ResponseWrapper<VerifyUserResponse>>
    {
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;

        public VerifyUserUseCase(IRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task<ResponseWrapper<VerifyUserResponse>> Handle(VerifyUserRequest request, CancellationToken cancellationToken)
        {
            var validated = new EmailValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<VerifyUserResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.Email);
            if (user is null)
            {
                return ResponseBuilder.Build<VerifyUserResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.NotFound, actionMessage: ResponseMessages.NOT_FOUND);
            }
            user.VerifyUser();

            var isUpdated = await _repository.UpdateAsync(user);

            if (isUpdated)
            {
                _emailService.SendVerificationEmail(user.FirstName, user.Email);
                return ResponseBuilder.Build<VerifyUserResponse>();
            }
            return ResponseBuilder.Build<VerifyUserResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.FAILURE);
        }
    }
}
