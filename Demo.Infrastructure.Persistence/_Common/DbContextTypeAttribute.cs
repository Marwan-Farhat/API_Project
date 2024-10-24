using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure.Persistence._Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbContextTypeAttribute:Attribute
    {
        public Type DbContextType { get;}

        public DbContextTypeAttribute(Type dbContextType)
        {
            DbContextType = dbContextType;
        }
    }
}
