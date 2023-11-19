using Amazon.S3;
using Amazon.S3.Model;
using InCorpApp.Application.Abstractions.AWS;
using InCorpApp.Contracts.Common;
using InCorpApp.Contracts.Shared.UploadFile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Infrastructure.AWS
{
    public sealed class S3Manager : IS3Manager
    {
        private readonly IAmazonS3 _amazons3;
        private readonly string _bucketName;

        public S3Manager(IAmazonS3 amazons3)
        {
            _amazons3 = amazons3;
            _bucketName = AWSNames.S3Name;
        }

        public async Task<bool> UploadFileToS3(string email, IFormFile file, UploadedFileCat filecategory)
        {
            try
            {
                var putObjectRequest = new PutObjectRequest()
                {
                    BucketName = _bucketName,
                    Key = $"{filecategory.ToString()}/{email}",
                    ContentType = file.ContentType,
                    InputStream = file.OpenReadStream(),
                    Metadata =
                {
                    ["x-amz-meta-originalname"] = file.FileName
                }
                };

                var response = await _amazons3.PutObjectAsync(putObjectRequest);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    
        public async Task<GetObjectResponse> GetFile(string email, UploadedFileCat fileCat)
        {
            var getObjectRequest = new GetObjectRequest()
            {
                BucketName = _bucketName,
                Key = $"{fileCat.ToString()}/{email}"
            };
            var file = await _amazons3.GetObjectAsync(getObjectRequest);
            return file;

        }
    }
}
