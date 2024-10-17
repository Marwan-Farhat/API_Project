using Demo.APIs.Controllers.Base;
using Demo.APIs.Controllers.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.APIs.Controllers.Controllers.Buggy
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("notfound")]  // Get: /api/buggy/notfound
        public IActionResult GetNotFoundRequest() 
        {
            // throw new NotFoundException();
             return NotFound(new ApiResponse(404));  // 404
        }


        [HttpGet("unauthorized")] // Get: /api/buggy/unauthorized
        public IActionResult GetAuthorizedrError()
        {
            return Unauthorized(new ApiResponse(401));  // 401
        }


        [HttpGet("badrequest")]  // Get: /api/buggy/badrequest 
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));  // 400
        }


        [HttpGet("badrequest/{id}")]  // Get: /api/buggy/badrequest/five
        public IActionResult GetValidationError(int id)
        {
            return Ok();
            /// This is an Example of Validation Error this endpoint should receive an integer and EndUser send a string
            /// Ok it's a buggy response as if there is a Validation Error will not go throw the endpoint
            /// as there is a Factory (InvalidModelStateFactory) will generate the response if there is an Validation Error
            /// this factory we have as we use [ApiController] Attribute that have multiple behaviours one of them InvalidModelStateFactory
        }


        [HttpGet("servererror")] // Get: /api/buggy/servererror
        public IActionResult GetServerError()
        {
            throw new Exception();  // 500
        }


        [HttpGet("forbidden")] // Get: /api/buggy/forbidden
        public IActionResult GetForbiddenRequest()
        {
            return Forbid();  // 401
        }


        [Authorize]
        [HttpGet("authorized")] // Get: /api/buggy/authorized
        public IActionResult GetAuthorizedRequest()
        {
            return Ok();  
        }
    }
}
