using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Data.Models
{
    public class TransactionRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float Credit { get; set; }
    }
}
