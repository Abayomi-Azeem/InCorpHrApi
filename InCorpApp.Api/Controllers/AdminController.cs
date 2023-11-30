using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal;
using InCorpApp.Contracts.Admin.GetAllUsers;
using InCorpApp.Contracts.Admin.GetUnverifiedRecruiters;
using InCorpApp.Contracts.Admin.GetUser;
using InCorpApp.Contracts.Admin.RemoveUser;
using InCorpApp.Contracts.Admin.VerifyUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InCorpApp.Api.Controllers
{
    /// <summary>
    /// User Management and Recruiter Management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : ControllerBase
    {
        private readonly ISender _sender;

        public AdminController(ISender sender)
        {
            _sender = sender;
        }

        //delete user
        /// <summary>
        /// Delete Existing User - Recruiter or Applicant
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete-user/{email}")]
        public async Task<IActionResult> RemoveUser(string email)
        {
            var request = new RemoveUserRequest() { Email = email };
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }


        /// <summary>
        /// Returns a List of all Unverified Recruiters in the App
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-unverified-recruiters")]
        public async Task<IActionResult> GetUnverifiedRecruiters()
        {
            var response = await _sender.Send(new GetUnverifiedRecruitersRequest());
            return StatusCode((int)response.HttpStatusCode, response);
        }

        
        /// <summary>
        /// Returns a List of Users Based on a Search Parameter
        /// </summary>
        /// <param name="searchParam"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search-user")]
        public async Task<IActionResult> GetUser(SearchUserBy searchParam, string value)
        {
            var request = new GetUserRequest() { SearchParam = searchParam, Value = value };
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }


        /// <summary>
        /// Verify a Recruiter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("verify-recruiter")]
        public async Task<IActionResult> VerifyRecruiter(VerifyUserRequest request)
        {
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet]
        [Route("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var request = new GetAllUsersRequest();
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
