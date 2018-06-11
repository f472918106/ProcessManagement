using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Models
{
    public class Part
    {
        public int type;
        public int size;

        public Part prev_Part, next_Part;
    }
}
