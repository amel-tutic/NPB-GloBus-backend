using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Data.Models
{
    public class Penalty
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int InspectorId { get; set; }
        public DateTime DateOfPenalty { get; set; }
        public float Price { get; set; }
    }
}
