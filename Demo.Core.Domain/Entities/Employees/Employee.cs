using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Entities.Employees
{
    public class Employee:BaseAuditableEntity<int>
    {
        public required string Name { get; set; }
        public int? Age { get; set; }
        public decimal Salary { get; set; }
        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
    }
}
