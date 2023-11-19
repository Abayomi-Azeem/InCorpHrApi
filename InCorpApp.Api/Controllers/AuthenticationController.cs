using InCorpApp.Application.Utilities;
using InCorpApp.Contracts.Authentication.Login;
using InCorpApp.Contracts.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InCorpApp.Api.Controllers
{
    /// <summary>
    /// Contains Endpoint to Authenticate a User
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private ISender _sender;

        public AuthenticationController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Login Endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(ResponseWrapper<LoginResponse>),200)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
          var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        /// <summary>
        /// Register as an Applicant
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-applicant")]
        [ProducesResponseType(typeof(ResponseWrapper<RegisterResponse>),200)]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }


        /// <summary>
        /// Register as a Recruiter
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-recruiter")]
        [ProducesResponseType(typeof(ResponseWrapper<RegisterResponse>), 200)]
        public async Task<IActionResult> RegisterAsRecruiter([FromBody] RegisterRecuiterRequest request)
        {
            var response = await _sender.Send(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }


    }
}
