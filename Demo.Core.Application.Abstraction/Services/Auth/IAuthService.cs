using Demo.Shared.Models.Auth;
using Demo.Shared.Models.Common;
using System.Security.Claims;

namespace Demo.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginDto model);
        Task<UserDto> RegisterAsync(RegisterDto model);
        Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal);
        Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal);
        Task<AddressDto> UpdateUserAddress(ClaimsPrincipal claimsPrincipal, AddressDto addressDto);
        Task<bool> EmailExists(string email);
    }
}
