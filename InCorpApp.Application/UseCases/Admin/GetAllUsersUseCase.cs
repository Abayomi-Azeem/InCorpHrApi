using Amazon.Runtime.Internal.Util;
using Amazon.S3.Model;
using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Admin.GetAllUsers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Admin
{
    public class GetAllUsersUseCase : IRequestHandler<GetAllUsersRequest, ResponseWrapper<List<GetAllUsersResponse>>>
    {
        private readonly IRepository _repository;
        private readonly ILogger<GetAllUsersUseCase> _logger;

        public GetAllUsersUseCase(IRepository repository, ILogger<GetAllUsersUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResponseWrapper<List<GetAllUsersResponse>>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GetAllUsersUseCase] - Request arrived");
            var users = await _repository.GetAllUsers();
            var response = users.ToGetAllUsers();
            return ResponseBuilder.Build<List<GetAllUsersResponse>>(response);
        }
    }
}
