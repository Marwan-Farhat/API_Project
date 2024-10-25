using Demo.Core.Application.Abstraction.Models.Auth;
using Demo.Core.Application.Abstraction.Services.Auth;
using Demo.Core.Application.Exceptions;
using Demo.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application.Services.Auth
{
    internal class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthService
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
                Id=user.Id,
                DisplayName=user.DisplayName,
                Email=user.Email!,
                Token="This will be Token"
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
                Token = "This will be Token"
            };
            return response;
        }
    }
}
