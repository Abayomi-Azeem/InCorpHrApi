using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Shared.UploadFile
{
    public class UploadFileRequest: SignedInUserRequest, IRequest<ResponseWrapper<UploadFileResponse>>
    {
        public UploadedFileCat FileType { get; set; }
        public IFormFile UploadedFile {get; set;}
        
    }

    public enum UploadedFileCat
    {
        CV,
        ProfileImage,
        Logo,
        BannerImage,
        TestQuestions
    }
}
