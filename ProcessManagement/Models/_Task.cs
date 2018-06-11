using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Models
{
    public class _Task
    {
        public int id;
        public int priority;
        public int state;

        public int createtime;
        public int exittime;
        public int turnaroundtime;

        public int partnumber;
        public int exitpartnumber;
        public Part partqueue=new Part();
        public Part exitpartqueue=new Part();

        public int cputime;
        public int iotime;
        public int readystart, readyend, readytime;
        public int ioreadystart, ioreadyend, ioreadytime;

        public int schedulenumber;
        public int ionumber;
        public int timeoutnumber;

        public double cpuratio;
        public double ioratio;
        public double readyratio;
        public double ioreadyratio;

        public _Task prev_Task, next_Task;
    }
}
