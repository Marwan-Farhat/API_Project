using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Shared.Models
{
    public class StripeSettings
    {
        public required string SecretKey { get; set; }
        public required string WebhookSecret { get; set; }

    }
}
