using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Models
{
    public class Device
    {
        public int state;
        public int policy;
        public _Task ioingtask;

        public int busytime;
        public int freetime;

        public int ionumber;
        public double utilization;
    }
}
