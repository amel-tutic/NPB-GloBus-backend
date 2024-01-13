using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Data.Models
{
    public class Line
    {
     public int Id { get; set; }
        public List<string> Stations { get; set; }
        public List<int>  Distance { get; set; }

    
    }
}
