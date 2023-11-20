using Amazon.Auth.AccessControlPolicy;
using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Admin.GetUnverifiedRecruiters;
using InCorpApp.Contracts.Applicant.ApplyJob;
using InCorpApp.Contracts.Applicant.CreateProfile;
using InCorpApp.Contracts.Applicant.GetActiveJobs;
using InCorpApp.Contracts.Applicant.GetTestQuestions;
using InCorpApp.Contracts.Applicant.SubmitTest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace InCorpApp.Api.Controllers
{
    /// <summary>
    /// Applicant Management
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApplicantPolicy")]
    public class ApplicantController : ControllerBase
    {
        //Create Profile
        private readonly ISender _sender;

        public ApplicantController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Create Applicant Profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-profile")]
        public async Task<IActionResult> CreateProfile(CreateProfileRequest request)
        {
            var claims = User.Claims;
            var signedInUserEmail = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            request.SignedInEmail = signedInUserEmail;
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }


        //applyjob
        /// <summary>
        /// Apply for A Job
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-job")]
        public async Task<IActionResult> ApplyJob(ApplyJobRequest request)
        {
            var claims = User.Claims;
            var signedInUserEmail = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            request.SignedInEmail = signedInUserEmail;
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }


        /// <summary>
        /// Get All ACtive Jobs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("active-jobs")]
        [ProducesResponseType(typeof(ResponseWrapper<List<GetActiveJobsResponse>>), 200)]
        public async Task<IActionResult> GetActiveJobs()
        {
            var request = new GetActiveJobsRequest();
            var signedInUserEmail = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            request.SignedInEmail = signedInUserEmail;
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }


        /// <summary>
        /// Get Test Questions
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-test-questions")]
        [ProducesResponseType(typeof(ResponseWrapper<GetTestQuestionsResponse>),200)]
        public async Task<IActionResult> GetQuestions(GetTestQuestionsRequest request)
        {
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }


        /// <summary>
        /// Submit Answers to Test
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("submit-test")]
        public async Task<IActionResult> SubmitTest(SubmitTestRequest request)
        {
            var claims = User.Claims;
            var signedInUserEmail = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            request.SignedInEmail = signedInUserEmail;
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }
        //take test jobPosterEmail, JobId
        //get appliedjobs
    }
}
