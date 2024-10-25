using Demo.Core.Application.Abstraction.Models.Auth;
using Demo.Core.Application.Abstraction.Services.Auth;
using Demo.Core.Application.Exceptions;
using Demo.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Core.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthService
    {
        public async Task<UserDto> LoginAsync(LoginDto model)
        {
            // To validate if the user has an account or not by Email if hasn't an UnAuthorizedException will be thrown
            var user = await userManager.FindByEmailAsync(model.Email);  

            if (user is null) throw new UnAuthorizedException("Invalid Login");


            // To validate if the user Allowed or his account LockedOut or the password wrong if the Account confirmed and not Locked and Succeeded is still false then the password is wrong
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);

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

        public async Task<UserDto> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email!,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user,model.Password);
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
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-256-bit-secretttttttttttttttttttttttt"));
            // Build Signing Credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);


            // Finaly Build Token with claims and Signing Credentials
            var tokenObj = new JwtSecurityToken(
                issuer: "TalabatIdentity",
                audience: "TalabatUsers",
                expires: DateTime.UtcNow.AddMinutes(10),
                claims: claims,
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenObj);
        }
    }
}
