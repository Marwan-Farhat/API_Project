using Demo.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Entities.Products
{
    public class Products : BaseEntity<int>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public  string? PictureUrl { get; set; }
        public  decimal Price { get; set; }

        public int? BrandId { get; set; }   // Foreign Key for ProductBrand    
        public virtual ProductBrand? Brand { get; set; } // Navigational Property with ProductBrand Table  M:1   +  It's virtual to enable Lazy Loading Mode
        public int? CategoryId { get; set; }   // Foreign Key for ProductCategory    
        public virtual ProductCategory? Category { get; set; } // Navigational Property with ProductCategory Table  M:1   +  It's virtual to enable Lazy Loading Mode
    }
} 
