using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Exceptions
{

    
    public class UserExistsException : Exception
    {
        public UserExistsException(string? message) : base(message)
        {
        }
    }
}
