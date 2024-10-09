using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Application.Abstraction.Models
{
    public class BrandDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
