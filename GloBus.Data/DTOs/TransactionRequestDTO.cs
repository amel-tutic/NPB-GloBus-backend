using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Data.DTOs
{
    public class TransactionRequestDTO
    {
        public int UserId { get; set; }
        public float Credit { get; set; }
    }
}
