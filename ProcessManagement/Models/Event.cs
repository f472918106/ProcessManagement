using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Models
{
    public class Event
    {
        public int time;
        public int type;
        public _Task task;

        public Event prev_Event, next_Event;
    }
}
