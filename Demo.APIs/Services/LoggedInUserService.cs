using Demo.Core.Application.Abstraction;
using System.Security.Claims;

namespace Demo.APIs.Services
{
    public class LoggedInUserService : ILoggedInUserService
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public string? UserId { get;  }

        public LoggedInUserService(IHttpContextAccessor? httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            // HttpContext Contains all Context Request Info We Specify User Info and use FindFirstValue method to get user id that named in the claim as PrimarySid
            UserId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.PrimarySid);  
        }
    }
}
