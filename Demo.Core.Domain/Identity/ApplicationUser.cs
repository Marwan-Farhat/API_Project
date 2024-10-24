using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Identity
{
    // ApplicationUser Inherit IdentityUser properties and here we customize and add more properties
    public class ApplicationUser:IdentityUser<string>
    {
        public required string DisplayName { get; set; }
        public virtual Address? Address { get; set; }      // virtual : for navigational property
    }
}
