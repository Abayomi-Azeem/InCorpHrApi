using Amazon.Auth.AccessControlPolicy;
using Amazon.Runtime.Internal;
using DocumentFormat.OpenXml.Office2016.Excel;
using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Applicant.CreateProfile;
using InCorpApp.Contracts.Authentication.Register;
using InCorpApp.Contracts.Recruiter.CreateJob;
using InCorpApp.Contracts.Recruiter.CreateRecruiterProfile;
using InCorpApp.Contracts.Recruiter.GetCreatedJobs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace InCorpApp.Api.Controllers
{
    /// <summary>
    /// Containts Endpoints Related to Actions performed by Recruiters
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RecruiterPolicy")]
    public class RecruiterController : ControllerBase
    {
        private readonly ISender _sender;

        public RecruiterController(ISender sender)
        {
            _sender = sender;
        }

        //CreateJob

        /// <summary>
        /// Create Recruiter Profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-profile")]
        public async Task<IActionResult> CreateProfile(CreateRecruiterProfileRequest request)
        {
            var claims = User.Claims;
            var signedInUserEmail = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            request.SignedInEmail = signedInUserEmail;
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }


        /// <summary>
        /// Create a Job, Complete with all Stages
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create-Job")]
        public async Task<IActionResult> CreateJob(CreateJobRequest request)
        {
            var signedInUserEmail = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            request.SignedInEmail = signedInUserEmail;
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        /// <summary>
        /// Get All Jobs Created by User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-created-jobs")]
        [ProducesResponseType(typeof(ResponseWrapper<List<GetCreatedJobsResponse>>), 200)]
        public async Task<IActionResult> GetJobs()
        {
            var signedInUserEmail = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            var request = new GetCreatedJobsRequest { SignedInEmail = signedInUserEmail };
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
