using InCorpApp.Contracts.Shared.UploadFile;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InCorpApp.Api.Controllers
{
    /// <summary>
    /// Contains Endpoints not Particular to any Role
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SharedController : ControllerBase
    {
        private readonly ISender _sender;

        public SharedController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Upload a file(CV, Logo, ProfilePicture) to the Server
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("upload-file")]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
        {
            var signedInUserEmail = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            request.SignedInEmail = signedInUserEmail;
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }

    }
}
