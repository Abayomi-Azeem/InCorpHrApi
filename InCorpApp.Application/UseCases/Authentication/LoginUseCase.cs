using InCorpApp.Application.Abstractions.Authentication;
using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Authentication.Login;
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
    public class LoginUseCase : IRequestHandler<LoginRequest, ResponseWrapper<LoginResponse>>
    {
        private readonly IRepository _repository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ILogger<LoginUseCase> _logger;

        public LoginUseCase(IRepository repository, ITokenGenerator tokenGenerator, ILogger<LoginUseCase> logger)
        {
            _repository = repository;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        public async Task<ResponseWrapper<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[LoginUseCase] - Request Arrived - {0}", request.Email);
            var validated = new LoginValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<LoginResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.Email);
            if (user is null)
            {                
                return ResponseBuilder.Build<LoginResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.INVALID_EMAIL_PASS );
            }
            var passwordHash = PasswordHasher.HashPassword(request.Password, user.PasswordSalt);

            if (passwordHash != user.Password)
            {
                return ResponseBuilder.Build<LoginResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.INVALID_EMAIL_PASS);
            }
            if (!user.IsVerified)
            {
                return ResponseBuilder.Build<LoginResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.UNVERIFIED_PROFILE);
            }
            var accessToken = _tokenGenerator.GenerateAccessToken(user);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            var loginResponse = user.ToLoginResponse(accessToken, refreshToken);
            return ResponseBuilder.Build<LoginResponse>(loginResponse);
        }
    }
}
