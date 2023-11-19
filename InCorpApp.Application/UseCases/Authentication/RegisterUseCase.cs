using InCorpApp.Application.Abstractions.AWS;
using InCorpApp.Application.Abstractions.Notification;
using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Authentication.Login;
using InCorpApp.Contracts.Authentication.Register;
using InCorpApp.Contracts.Common;
using InCorpApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Authentication
{
    public class RegisterUseCase : IRequestHandler<RegisterRequest, ResponseWrapper<RegisterResponse>>
    {
        private readonly IRepository _repository;
        private readonly IS3Manager _s3Manager;
        private readonly ILogger<RegisterUseCase> _logger;
        private readonly IEmailService _emailService;

        public RegisterUseCase(IRepository repository, IS3Manager s3Manager, ILogger<RegisterUseCase> logger, IEmailService emailService)
        {
            _repository = repository;
            _s3Manager = s3Manager;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<ResponseWrapper<RegisterResponse>> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[RegisterUseCase] - Request Arrived - {0}", request.Email);
            var validated = new RegisterValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<RegisterResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.Email);
            if (user is not null)
            {
                return ResponseBuilder.Build<RegisterResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.USER_EXISTS);
            }

            var passwordSalt = PasswordHasher.GeneratePasswordSalt(request.Email);
            var hashedPassword = PasswordHasher.HashPassword(request.Password, passwordSalt);

            User createdUser = User.CreateApplicant(request, hashedPassword,passwordSalt);
            var userCreated = await _repository.InsertAsync(createdUser);

            _emailService.SendNewMemberEmail(request.FirstName, request.Email);
            if (userCreated)
            {
                return ResponseBuilder.Build<RegisterResponse>();
            }
            return ResponseBuilder.Build<RegisterResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.USER_CREATION_FAILED);
        }  
               
    }
}
