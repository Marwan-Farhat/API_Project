using Demo.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Entities.Products
{
    public class ProductCategory:BaseEntity<int>
    {
        public required string Name { get; set; }

    }
}
