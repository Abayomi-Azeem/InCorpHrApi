using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Authentication.Login;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Recruiter.CreateJob;
using InCorpApp.Contracts.Recruiter.CreateRecruiterProfile;
using InCorpApp.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Recruiter
{
    internal class CreateJobUseCase : IRequestHandler<CreateJobRequest, ResponseWrapper<CreateJobResponse>>
    {
        private readonly ILogger<CreateJobUseCase> _logger;
        private readonly IRepository _repository;

        public CreateJobUseCase(ILogger<CreateJobUseCase> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<ResponseWrapper<CreateJobResponse>> Handle(CreateJobRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[CreateJobUseCase] - Request Arrived - {0}", JsonConvert.SerializeObject(request));
            var validated = new CreateJobValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<CreateJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail!);
            if (user is null)
            {
                return ResponseBuilder.Build<CreateJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.INVALID_EMAIL_PASS);
            }
            foreach (var stage in request.Stages)
            {
                var isValid = ValidateStage(stage);
                if (!isValid)
                {
                    return ResponseBuilder.Build<CreateJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: $"Invalid Stage Properties - {stage.StageNumber}");
                }
            }
            user.CreateJob(request);
            var isUpdated = await _repository.UpdateAsync(user);

            if (isUpdated)
            {
                return ResponseBuilder.Build<CreateJobResponse>();
            }
            return ResponseBuilder.Build<CreateJobResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.FAILURE);

        }

        private bool ValidateStage(Stage stage)
        {
            var response = false;
            switch (stage.StageType)
            {
                case Contracts.Enums.StageType.CV:
                    try
                    {
                        var serialized = JsonConvert.DeserializeObject<CVScan>(stage.StageProperties);
                        response = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"[ValidateStage] - {stage.StageNumber} - {stage.StageProperties} - {ex.Message}");
                        return false;                        
                    }
                    break;
                case Contracts.Enums.StageType.PersonalityTest:
                    try
                    {
                        var serialized = JsonConvert.DeserializeObject<PersonalityTest>(stage.StageProperties);
                        response = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"[ValidateStage] - {stage.StageNumber} - {stage.StageProperties} - {ex.Message}");
                        return false;
                    }
                    break;
                case Contracts.Enums.StageType.TechnicalTest:
                    try
                    {
                        var serialized = JsonConvert.DeserializeObject<TechnicalTest>(stage.StageProperties);
                        response = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"[ValidateStage] - {stage.StageNumber} - {stage.StageProperties} - {ex.Message}");
                        return false;
                    }
                    break;
                case Contracts.Enums.StageType.Interview:
                    return true;
                default:
                    break;
            }
            return response;
        }
    }
}
