﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Infrastructure.Repositories
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
