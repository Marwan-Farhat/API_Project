using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Demo.Core.Domain.Common
{
    public abstract class BaseEntity<TKey> where TKey : IEquatable<TKey>  // We Used Generic key So that we do not Enforce every entity inherent from this entity to have specific Key Type
    {
        public TKey Id { get; set; }        // required is a keyword to enforce new to not initialize this property but
                                                     // when we make an object from class we must initialize this property in object initializer 
    }
}
