using Demo.Core.Domain.Contracts.Persistence;
using Demo.Core.Domain.Contracts.Persistence.DbInitializers;
using Demo.Core.Domain.Identity;
using Demo.Infrastructure.Persistence.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence.Identity
{
    internal class StoreIdentityDbInitializer(StoreIdentityDbContext _dbContext,UserManager<ApplicationUser> _userManager) : DbInitializer(_dbContext), IStoreIdentityDbInitializer
    {
        // We Inherited InitializeAsync from DbInitializer
        public override async Task SeedAsync()
        {
            if (!_userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Ahmed Nasr",
                    UserName = "ahmed.nasr",
                    Email = "ahmed.nasr@gmail.com",
                    PhoneNumber = "01122334455"
                };
                await _userManager.CreateAsync(user, "P@ssw0rd"); 
            }
        }
    }
}
