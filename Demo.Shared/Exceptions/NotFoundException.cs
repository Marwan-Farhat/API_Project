using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Shared.Exceptions
{
    public class NotFoundException:ApplicationException
    {
        public NotFoundException(string name, object key)  // name: the entity name that throwed the exception, key: PK of this entity (id)
        : base($"{name} with {key} is not found")
        {
            
        }
    }
}
