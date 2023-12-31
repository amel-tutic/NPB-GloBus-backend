﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException(string? message) : base(message)
        {
        }
    }
}
