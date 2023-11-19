using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Admin.GetUnverifiedRecruiters;
using InCorpApp.Contracts.Admin.GetUser;
using InCorpApp.Contracts.Authentication.Login;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Admin
{
    public class GetUserUseCase : IRequestHandler<GetUserRequest, ResponseWrapper<IEnumerable<GetUserResponse>>>
    {
        private readonly IRepository _repository;

        public GetUserUseCase(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseWrapper<IEnumerable<GetUserResponse>>> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            var validated = new GetUserValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<IEnumerable<GetUserResponse>>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }

            var users = await _repository.GetUsers(request.SearchParam, request.Value);

            return ResponseBuilder.Build<IEnumerable<GetUserResponse>>(users);

        }
    }
}
