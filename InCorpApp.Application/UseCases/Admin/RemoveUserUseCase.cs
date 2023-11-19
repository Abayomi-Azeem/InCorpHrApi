using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Admin.RemoveUser;
using InCorpApp.Contracts.Authentication.Login;
using InCorpApp.Contracts.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Admin
{
    public class RemoveUserUseCase : IRequestHandler<RemoveUserRequest, ResponseWrapper<RemoveUserResponse>>
    {
        private readonly IRepository _repository;
        private readonly ILogger<RemoveUserUseCase> _logger;

        public RemoveUserUseCase(IRepository repository, ILogger<RemoveUserUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResponseWrapper<RemoveUserResponse>> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[RemoveUserUseCase] - Request Arrived - {0}", request.Email);
            var validated = new EmailValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<RemoveUserResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.Email);
            if (user is null)
            {
                return ResponseBuilder.Build<RemoveUserResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.INVALID_EMAIL_PASS);
            }
            var isRemoved = await _repository.RemoveAsync(request.Email);

            if (isRemoved)
            {
                return ResponseBuilder.Build<RemoveUserResponse>();
            }
            return ResponseBuilder.Build<RemoveUserResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.FAILURE);
        }
    }
}
