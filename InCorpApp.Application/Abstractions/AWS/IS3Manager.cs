using Amazon.S3.Model;
using InCorpApp.Contracts.Shared.UploadFile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Application.Abstractions.AWS
{
    public interface IS3Manager
    {
        public Task<bool> UploadFileToS3(string email, IFormFile file, UploadedFileCat fileCat);
        Task<GetObjectResponse> GetFile(string email, UploadedFileCat fileCat);
    }
}
