using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Admin.GetUnverifiedRecruiters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Admin
{
    public class GetUnverifiedRecruitersUseCase : IRequestHandler<GetUnverifiedRecruitersRequest, ResponseWrapper<IEnumerable<GetUnverifiedRecruitersResponse>>>
    {
        private readonly IRepository _repository;

        public GetUnverifiedRecruitersUseCase(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseWrapper<IEnumerable<GetUnverifiedRecruitersResponse>>> Handle(GetUnverifiedRecruitersRequest request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetUnverifiedRecruiters();

            return ResponseBuilder.Build<IEnumerable<GetUnverifiedRecruitersResponse>>(users);
        }
    }
}
