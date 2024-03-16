using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloBus.Data.DTOs
{
    public class EditLineDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Stations { get; set; }
        public List<int> Distance { get; set; }
    }
}
