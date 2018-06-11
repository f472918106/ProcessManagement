using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManagement.Models;

namespace ProcessManagement.Controllers
{
    public class HandleHandler
    {
        public void TaskCreateEventHandle(Event _event)
        {
            _Task task;
            Event e;
            if ((task = Program.taskhandler.CreateTask(Program.taskid++, Program.systemtime, Program.NEXTPARTNUMBER)) == null)
            {
                return;
            }
            _event.task = task;
            Program.taskhandler.AddTaskToQueue(task, Program.readytaskqueue);

            if (Program.cpu.state == Program.FREE)
            {
                if((e=Program.eventhandler.CreateEvent(Program.systemtime, Program.TASKSCHEDULEEVENT)) != null)
                {
                    Program.eventhandler.FrontAddEventToQueue(e, Program.eventqueue);
                }
                return;
            }
        }
        public void TaskScheduleEventHandle(Event _event)
        {
            Event e;
            _Task task;
            Part part;

            if ((Program.cpu.state == Program.BUSY) || (Program.taskhandler.IsEmptyTaskQueue(Program.readytaskqueue) == Program.TRUE))
            {
                return;
            }

            if (Program.cpu.policy == Program.HPF || Program.cpu.policy == Program.HPFRR)
            {
                task = Program.taskhandler.HpTaskInQueue(Program.readytaskqueue);
            }
            else
            {
                task = Program.taskhandler.FirstTaskInQueue(Program.readytaskqueue);
            }

            Program.taskhandler.RemoveTaskFromQueue(task);
            _event.task = task;

            task.state = Program.TASKRUNNINGSTATE;
            Program.cpu.state = Program.BUSY;
            Program.cpu.runningtask = task;

            part = Program.parthandler.FirstPartInQueue(task.partqueue);
            Program.parthandler.RemovePartFromQueue(part);
            task.partnumber--;

            task.readyend = Program.systemtime;
            task.readytime += (task.readyend - task.readystart);
            task.readyend = task.readystart = 0;

            Program.cpu.schedulenumber++;
            Program.cpu.busytime += part.size;
            task.cputime += part.size;

            e=Program.eventhandler.CreateEvent(Program.systemtime+part.size,Program.parthandler.IsEmptyPartQueue(task.partqueue)==Program.TRUE?Program.TASKRUNEXITEVENT:Program.TASKIOREQUESTEVENT);
            Program.eventhandler.AddEventToQueue(e, Program.eventqueue);

            Program.parthandler.AddPartToQueue(part, task.exitpartqueue);
            task.exitpartnumber++;

            return;
        }
        public void TaskTimeoutEventHandle(Event _event)
        {
            Console.Write("\n\n timeouteventhandle!!");
        }
        public void TaskIoRequestEventHandle(Event _event)
        {
            _Task task;
            Event e;

            task = Program.cpu.runningtask;
            _event.task = task;

            Program.cpu.state = Program.FREE;
            Program.cpu.runningtask = null;
            task.state = Program.TASKBLOCKEDSTATE;

            task.ioreadystart = Program.systemtime;
            task.ioreadyend = 0;
            Program.taskhandler.AddTaskToQueue(task, Program.blockedtaskqueue);

            if (Program.taskhandler.IsEmptyTaskQueue(Program.readytaskqueue) == Program.FALSE)
            {
                if ((e = Program.eventhandler.CreateEvent(Program.systemtime, Program.TASKSCHEDULEEVENT)) != null)
                {
                    Program.eventhandler.FrontAddEventToQueue(e, Program.eventqueue);
                }
            }

            if (Program.device.state == Program.FREE)
            {
                if ((e = Program.eventhandler.CreateEvent(Program.systemtime, Program.TASKIOSCHEDULEEVENT)) != null)
                {
                    Program.eventhandler.FrontAddEventToQueue(e, Program.eventqueue);
                }
            }
            return;
        }
        public void TaskIoScheduleEventHandle(Event _event)
        {
            _Task task;
            Event e;
            Part part;

            if ((Program.device.state == Program.BUSY) || (Program.taskhandler.IsEmptyTaskQueue(Program.blockedtaskqueue)) == Program.TRUE)
            {
                return;
            }

            if (Program.device.policy == Program.HPF)
            {
                task = Program.taskhandler.HpTaskInQueue(Program.blockedtaskqueue);
            }
            else
            {
                task = Program.taskhandler.FirstTaskInQueue(Program.blockedtaskqueue);
            }
            Program.taskhandler.RemoveTaskFromQueue(task);

            task.state = Program.TASKIOINGSTATE;
            Program.device.state = Program.BUSY;
            Program.device.ioingtask = task;
            _event.task = task;

            part = Program.parthandler.FirstPartInQueue(task.partqueue);
            Program.parthandler.RemovePartFromQueue(part);
            task.partnumber--;

            task.ioreadyend = Program.systemtime;
            task.ioreadytime += (task.ioreadyend - task.ioreadystart);
            task.ioreadyend = task.ioreadystart = 0;

            Program.device.ionumber++;
            Program.device.busytime += part.size;
            task.iotime += part.size;

            e = Program.eventhandler.CreateEvent(Program.systemtime + part.size, Program.parthandler.IsEmptyPartQueue(task.partqueue) == Program.TRUE ? Program.TASKIOEXITEVENT : Program.TASKIOCOMPLETEEVENT);
            Program.eventhandler.AddEventToQueue(e, Program.eventqueue);

            Program.parthandler.AddPartToQueue(part, task.exitpartqueue);
            task.exitpartnumber++;

            return;
        }
        public void TaskIoCompleteEventHandle(Event _event)
        {
            _Task task;
            Event e;

            task = Program.device.ioingtask;
            _event.task = task;
            Program.device.state = Program.FREE;
            Program.device.ioingtask = null;

            task.state = Program.TASKREADYSTATE;
            task.readystart = Program.systemtime;
            Program.taskhandler.AddTaskToQueue(task, Program.readytaskqueue);

            if (Program.taskhandler.IsEmptyTaskQueue(Program.blockedtaskqueue) == Program.FALSE)
            {
                if ((e = Program.eventhandler.CreateEvent(Program.systemtime, Program.TASKIOSCHEDULEEVENT)) != null)
                {
                    Program.eventhandler.FrontAddEventToQueue(e, Program.eventqueue);
                }
            }

            if (Program.cpu.state == Program.FREE)
            {
                if ((e = Program.eventhandler.CreateEvent(Program.systemtime, Program.TASKSCHEDULEEVENT)) != null)
                {
                    Program.eventhandler.FrontAddEventToQueue(e, Program.eventqueue);
                }
            }
            return;
        }
        public void TaskRunExitEventHandle(Event _event)
        {
            _Task task;
            Event e;

            task = Program.cpu.runningtask;
            _event.task = task;

            task.state = Program.TASKEXITSTATE;
            task.exittime = Program.systemtime;
            Program.taskhandler.AddTaskToQueue(task, Program.exittaskqueue);

            if (Program.taskhandler.IsEmptyTaskQueue(Program.readytaskqueue) == Program.FALSE)
            {
                if ((e = Program.eventhandler.CreateEvent(Program.systemtime, Program.TASKSCHEDULEEVENT)) != null)
                {
                    Program.eventhandler.FrontAddEventToQueue(e, Program.eventqueue);
                }
            }
            return;
        }
        public void TaskIoExitEventHandle(Event _event)
        {
            _Task task;
            Event e;

            task = Program.device.ioingtask;
            _event.task = task;

            Program.device.state = Program.FREE;
            Program.device.ioingtask = null;

            task.state = Program.TASKEXITSTATE;
            task.exittime = Program.systemtime;
            Program.taskhandler.AddTaskToQueue(task, Program.exittaskqueue);

            if (Program.taskhandler.IsEmptyTaskQueue(Program.readytaskqueue) == Program.FALSE)
            {
                if ((e = Program.eventhandler.CreateEvent(Program.systemtime, Program.TASKSCHEDULEEVENT)) != null)
                {
                    Program.eventhandler.FrontAddEventToQueue(e, Program.eventqueue);
                }
            }
            return;
        }
    }
}
