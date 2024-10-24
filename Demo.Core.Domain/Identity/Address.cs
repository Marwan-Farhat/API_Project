using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Identity
{
    public class Address
    {
        public int Id { get; set; }

        // We added FirstName & LastName because if user have more than one address in this app so we will set FirstName & LastName for the person who will receive the order
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }


        public required string UserId { get; set; }
        public virtual required ApplicationUser User { get; set; }
    }
}
