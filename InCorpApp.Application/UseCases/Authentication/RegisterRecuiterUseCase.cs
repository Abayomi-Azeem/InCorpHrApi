using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Authentication.Register;
using InCorpApp.Contracts.Common;
using InCorpApp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Authentication
{
    public class RegisterRecuiterUseCase : IRequestHandler<RegisterRecuiterRequest, ResponseWrapper<RegisterResponse>>
    {
        private readonly IRepository _repository;
        private readonly ILogger<RegisterRecuiterUseCase> _logger;

        public RegisterRecuiterUseCase(IRepository repository, ILogger<RegisterRecuiterUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResponseWrapper<RegisterResponse>> Handle(RegisterRecuiterRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[RegisterRecuiterUseCase] - Request Arrived - {0}", request.CompanyEmail);
            var validated = new RegisterRecuiterValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<RegisterResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.CompanyEmail);
            if (user is not null)
            {
                return ResponseBuilder.Build<RegisterResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.USER_EXISTS);
            }
            var passwordSalt = PasswordHasher.GeneratePasswordSalt(request.CompanyEmail);
            var hashedPassword = PasswordHasher.HashPassword(request.Password, passwordSalt);

            User createdUser = User.CreateRecruiter(request, hashedPassword, passwordSalt);
            var userCreated = await _repository.InsertAsync(createdUser);

            if (userCreated)
            {
                return ResponseBuilder.Build<RegisterResponse>();
            }
            return ResponseBuilder.Build<RegisterResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.USER_CREATION_FAILED);

        }
    }
}
