using AutoMapper;
using Demo.Core.Application.Abstraction.Services.Auth;
using Demo.Core.Application.Extensions;
using Demo.Core.Domain.Entities.Identity;
using Demo.Shared.Exceptions;
using Demo.Shared.Models.Auth;
using Demo.Shared.Models.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Core.Application.Services.Auth
{
    public class AuthService(IMapper mapper,IOptions<JwtSettings> jwtSettings,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthService
    {
        private readonly JwtSettings _jwtSettings= jwtSettings.Value;


        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            // To validate if the user has an account or not by Email if hasn't an UnAuthorizedException will be thrown
            var user = await userManager.FindByEmailAsync(loginDto.Email);  

            if (user is null) throw new UnAuthorizedException("Invalid Login");


            // To validate if the user Allowed or his account LockedOut or the password wrong if the Account confirmed and not Locked and Succeeded is still false then the password is wrong
            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

            if (result.IsNotAllowed) throw new UnAuthorizedException("Account not confirmed yet");
            if (result.IsLockedOut) throw new UnAuthorizedException("Account is Locked");
            //if (result.RequiresTwoFactor) throw new UnAuthorizedException("Requires Two-Factor Authentication");
            if (!result.Succeeded) throw new UnAuthorizedException("Invalid Login");


            // If he pass all this validation then he has an account with the email he entered and the password is true
            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user)
            };
            return response;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            /// This already done with identity Extensions Configurations (RequireUniqueEmail)
            /// if (EmailExists(registerDto.Email).Result) // Result to skip Async as rest of code depend on this condition
            ///     throw new BadRequestException("This email is already in use");

            var user = new ApplicationUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email!,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) throw new ValidationException() { Errors = result.Errors.Select(E => E.Description) };

            // If this validation is Succeeded then his account will be created
            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user)
            };
            return response;
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            // To Get User Claims from Database if there is User Claims
            var userClaims = await userManager.GetClaimsAsync(user);


            // To Get User roles from Database to store it as a claims and send it as a claims in token, if Frontend need to know user roles he will find it in the Token
            var rolesAsClaims = new List<Claim>();
            var roles =await userManager.GetRolesAsync(user);
            foreach (var role in roles) 
                rolesAsClaims.Add(new Claim(ClaimTypes.Role,role.ToString()));


            // Create Claims this not all claims i will send
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.GivenName,user.DisplayName)
            }
               .Union(userClaims)
               .Union(rolesAsClaims);


            // Build Secret Security Key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            // Build Signing Credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);


            // Finaly Build Token with claims and Signing Credentials
            var tokenObj = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                claims: claims,
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenObj);
        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(email!);

            return new UserDto()
            {
                Id = user!.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = await GenerateTokenAsync(user)
            };
        }

        public async Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal)
        {
            var user = await userManager.FindUserWithAddress(claimsPrincipal!);

            var address = mapper.Map<AddressDto>(user!.Address);

            return address;
        }

        public async Task<AddressDto> UpdateUserAddress(ClaimsPrincipal claimsPrincipal, AddressDto addressDto)
        {
            // Map updated Address from AddressDto to Address
            var updatedAddress = mapper.Map<Address>(addressDto);

            // Find User With Address
            var user = await userManager.FindUserWithAddress(claimsPrincipal!);

            // Check if the user already has an address he will set the new address id with the exists address id So that he don't make another new address with new record
            if (user?.Address is not null)
                updatedAddress.Id = user.Address.Id;

            user!.Address = updatedAddress;

            // Update Address
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.Select(error => error.Description).Aggregate((X, Y) => $"{X}, {Y}"));

            return addressDto;
        }

        public async Task<bool> EmailExists(string email)
        {   
            return await userManager.FindByEmailAsync(email!) is not null;
        }
    }
}
