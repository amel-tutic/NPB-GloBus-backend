using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Data.DTOs
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Line { get; set; }
        public string Start { get; set; }
        public string Destination { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TicketType { get; set; }
        public bool isApproved { get; set; }
    }
}
