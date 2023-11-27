using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Applicant.ApplyJob;
using InCorpApp.Contracts.Applicant.GetActiveJobs;
using InCorpApp.Contracts.Common;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Applicant
{
    public class GetActiveJobsUseCase : IRequestHandler<GetActiveJobsRequest, ResponseWrapper<List<GetActiveJobsResponse>>>
    {
        private readonly IRepository _repository;
        private readonly IMemoryCache _cache;
        private static DateTime _CacheKey = new DateTimeProvider().CurrentDateTime();

        public GetActiveJobsUseCase(IRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<ResponseWrapper<List<GetActiveJobsResponse>>> Handle(GetActiveJobsRequest request, CancellationToken cancellationToken)
        {
            var validated = new SignedInUserValidator().Validate(request);
            IEnumerable<GetActiveJobsResponse>jobs;
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<List<GetActiveJobsResponse>>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail);
            if (user is null)
            {
                return ResponseBuilder.Build<List<GetActiveJobsResponse>>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.NOT_FOUND);
            }

            if (!_cache.TryGetValue(_CacheKey, out jobs))
            {
                jobs = await _repository.GetAllUnExpiredJobs();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(10),
                    AbsoluteExpiration = DateTime.UtcNow.AddHours(1),
                    Priority = CacheItemPriority.Normal
                };
                _CacheKey = new DateTimeProvider().CurrentDateTime();
                _cache.Set(_CacheKey, jobs, cacheEntryOptions);
            }                        
            return ResponseBuilder.Build<List<GetActiveJobsResponse>>(jobs.ToList());
        }
    }
}
