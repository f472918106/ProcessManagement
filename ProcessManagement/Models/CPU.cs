using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Models
{
    public class CPU
    {
        public int state;
        public int policy;
        public _Task runningtask;

        public int busytime;
        public int freetime;

        public int schedulenumber;
        public double utilization;
    }
}
