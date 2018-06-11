using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManagement.Models;

namespace ProcessManagement.Controllers
{
    public class _EventHandler
    {
        public Event CreateEvent(int time,int type)
        {
            Event e=new Event();
            if (e == null)
            {
                return null;
            }
            e.time = time;
            e.type = type;
            e.task = null;
            e.prev_Event = e.next_Event = e;
            return e;
        }
        public void DestroyEvent(Event _event)
        {
            IDisposable disposable = _event as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        public void InitEventQueue(Event queue)
        {
            queue.prev_Event = queue.next_Event = queue;
        }
        public void ClearEventQueue(Event queue)
        {
            Event e;
            while (IsEmptyEventQueue(queue) == Program.FALSE)
            {
                e = FirstEventInQueue(queue);
                RemoveEventFromQueue(e);
                DestroyEvent(e);
            }
            InitEventQueue(queue);
        }
        public int IsEmptyEventQueue(Event queue)
        {
            if (queue == queue.next_Event)
            {
                return Program.TRUE;
            }
            else
            {
                return Program.FALSE;
            }
        }
        public int SizeofEventQueue(Event queue)
        {
            int n;
            Event e;
            for (n = 0, e = queue.next_Event; e != queue; ++n, e = e.next_Event) ;
            return n;
        }
        public void PrintEvent(Event e)
        {
            Console.Write("\n {0}, ", e.time);
            switch (e.type)
            {
                case Program.TASKCREATEEVENT:
                    Console.Write("create ");
                    break;
                case Program.TASKSCHEDULEEVENT:
                    Console.Write("sch    ");
                    break;
                case Program.TASKTIMEOUTEVENT:
                    Console.Write("timeout");
                    break;
                case Program.TASKIOREQUESTEVENT:
                    Console.Write("ioreque");
                    break;
                case Program.TASKIOSCHEDULEEVENT:
                    Console.Write("iosch  ");
                    break;
                case Program.TASKIOCOMPLETEEVENT:
                    Console.Write("iocompl");
                    break;
                case Program.TASKRUNEXITEVENT:
                    Console.Write("runexit");
                    break;
                case Program.TASKIOEXITEVENT:
                    Console.Write("ioexit ");
                    break;
                default:
                    Console.Write("Unknown");
                    break;
            }
            Console.Write(", id={0}", e.task == null ? -1 : e.task.id);
        }
        public void PrintEventQueue(Event queue)
        {
            Event e;
            for (e = queue.next_Event; e != queue; e = e.next_Event)
            {
                PrintEvent(e);
            }
        }
        public Event FirstEventInQueue(Event queue)
        {
            if (IsEmptyEventQueue(queue) == Program.TRUE)
            {
                return null;
            }
            else
            {
                return queue.next_Event;
            }
        }
        public void RemoveEventFromQueue(Event _event)
        {
            _event.prev_Event.next_Event = _event.next_Event;
            _event.next_Event.prev_Event = _event.prev_Event;
            _event.prev_Event = _event.next_Event = _event;
        }
        public void AddEventToQueue(Event _event,Event queue)
        {
            Event e;
            if (IsEmptyEventQueue(queue) == Program.TRUE)
            {
                queue.next_Event = queue.prev_Event = _event;
                _event.prev_Event = _event.next_Event = queue;
                return;
            }
            for (e = queue.next_Event; e != queue && e.time <= _event.time; e = e.next_Event) ;
            _event.next_Event = e;
            _event.prev_Event = e.prev_Event;
            e.prev_Event.next_Event = _event;
            e.prev_Event = _event;
            return;
        }
        public void FrontAddEventToQueue(Event _event,Event queue)
        {
            Event e;
            if (IsEmptyEventQueue(queue) == Program.TRUE)
            {
                queue.next_Event = queue.prev_Event = _event;
                _event.prev_Event = _event.next_Event = queue;
                return;
            }
            for (e = queue.next_Event; e != queue && e.time < _event.time; e = e.next_Event) ;
            _event.next_Event = e;
            _event.prev_Event = e.prev_Event;
            e.prev_Event.next_Event = _event;
            e.prev_Event = _event;
            return;
        }
    }
}
