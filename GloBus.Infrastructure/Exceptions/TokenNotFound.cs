using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Exceptions
{
    internal class TokenNotFound : Exception
    {
        public TokenNotFound(string? message) : base(message){

        }
    }
}
