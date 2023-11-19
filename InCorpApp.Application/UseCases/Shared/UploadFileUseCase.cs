using InCorpApp.Application.Abstractions.AWS;
using InCorpApp.Application.Abstractions.Persistence;
using InCorpApp.Application.Utilities;
using InCorpApp.Application.Validators;
using InCorpApp.Contracts.Applicant.CreateProfile;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Shared.UploadFile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.UseCases.Shared
{
    public class UploadFileUseCase : IRequestHandler<UploadFileRequest, ResponseWrapper<UploadFileResponse>>
    {
        private readonly IRepository _repository;
        private readonly IS3Manager _s3Manager;
        public UploadFileUseCase(IRepository repository, IS3Manager s3Manager)
        {
            _repository = repository;
            _s3Manager = s3Manager;
        }

        public async Task<ResponseWrapper<UploadFileResponse>> Handle(UploadFileRequest request, CancellationToken cancellationToken)
        {
            var validated = new SignedInUserValidator().Validate(request);
            if (!validated.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in validated.Errors)
                {
                    validationErrors.Add(error.ErrorMessage);
                }
                return ResponseBuilder.Build<UploadFileResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: validationErrors.First());
            }
            var user = await _repository.GetById(request.SignedInEmail);
            if (user is null)
            {
                return ResponseBuilder.Build<UploadFileResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.INVALID_EMAIL_PASS);
            }
            var isUploaded = await _s3Manager.UploadFileToS3(request.SignedInEmail, request.UploadedFile, request.FileType);
            if (isUploaded)
            {
                return ResponseBuilder.Build<UploadFileResponse>();
            }
            return ResponseBuilder.Build<UploadFileResponse>(hasError: true, statusCode: System.Net.HttpStatusCode.BadRequest, actionMessage: ResponseMessages.FAILURE);

        }
    }
    
}
