using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Shared.Exceptions
{
    public class BadRequestException:ApplicationException
    {
        public BadRequestException(string? message=null)
            :base(message)
        {
            
        }
    }
}
