using Demo.APIs.Controllers.Base;
using Demo.Core.Application.Abstraction.Services;
using Demo.Shared.Models.Auth;
using Demo.Shared.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.APIs.Controllers.Controllers.Account
{
    public class AccountController(IServiceManager serviceManager):BaseApiController
    {
        [HttpPost("login")]  // POST: /api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var result = await serviceManager.AuthService.LoginAsync(model);
            return Ok(result);
        }


        [HttpPost("register")]// POST: /api/account/register
        public async Task<ActionResult<UserDto>> Register (RegisterDto model)
        {
            var result = await serviceManager.AuthService.RegisterAsync(model);
            return Ok(result);
        }

        [Authorize]
        [HttpGet] // GET: /api/account
        public async Task<ActionResult<UserDto>>GetCurrentUser()
        {
            var result = await serviceManager.AuthService.GetCurrentUser(User);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("address")] // GET: /api/account/address
        public async Task<ActionResult<AddressDto>>GetUserAddress()
        {
            var result = await serviceManager.AuthService.GetUserAddress(User);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("address")] // Put: /api/account/address
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var result = await serviceManager.AuthService.UpdateUserAddress(User, address);
            return Ok(result);
        }

        [HttpGet("emailexists")] // GET: /api/account/emailexists?email=ali@gmail.com
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return Ok(await serviceManager.AuthService.EmailExists(email!));
        }
    }
}
