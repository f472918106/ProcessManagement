using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManagement.Models;

namespace ProcessManagement.Controllers
{
    public class TaskHandler
    {
        public _Task CreateTask(int id,int createtime,int partnumber)
        {
            _Task task = new _Task();

            if (task == null)
            {
                return null;
            }

            task.id = id;
            task.state = Program.TASKREADYSTATE;
            task.priority = Program.NEXTPRIORITY;
            task.createtime = createtime;
            task.exittime = task.turnaroundtime = 0;

            task.partnumber = partnumber;
            Program.parthandler.InitPartQueue(task.partqueue);
            Program.parthandler.AddPartsToQueue(partnumber, task.partqueue);

            task.exitpartnumber = 0;
            Program.parthandler.InitPartQueue(task.exitpartqueue);

            task.readystart = createtime;
            task.readyend = 0;
            task.ioreadystart = task.ioreadyend = 0;

            task.cputime = task.iotime = task.readytime = task.ioreadytime = 0;
            task.schedulenumber = task.timeoutnumber = task.ionumber = 0;

            task.cpuratio = task.ioratio = task.readyratio = task.ioreadyratio = 0.0;

            task.prev_Task = task.next_Task = task;

            return task;            
        }
        public void DestroyTask(_Task task)
        {
            Program.parthandler.ClearPartQueue(task.partqueue);
            Program.parthandler.ClearPartQueue(task.exitpartqueue);
            IDisposable disposable = task as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        public void InitTaskQueue(_Task queue)
        {
            Program.parthandler.InitPartQueue(queue.partqueue);
            Program.parthandler.InitPartQueue(queue.exitpartqueue);
            queue.prev_Task = queue.next_Task = queue;
        }
        public void ClearTaskQueue(_Task queue)
        {
            _Task task;
            while (IsEmptyTaskQueue(queue) == Program.FALSE)
            {
                task = FirstTaskInQueue(queue);
                RemoveTaskFromQueue(task);
                DestroyTask(task);
            }
            InitTaskQueue(queue);
        }
        public int SizeofTaskQueue(_Task queue)
        {
            int n;
            _Task task;
            for (n = 0, task = queue.next_Task; task != queue; ++n, task = task.next_Task) ;
            return n;
        }
        public int IsEmptyTaskQueue(_Task queue)
        {
            if (queue.next_Task == queue)
            {
                return Program.TRUE;
            }
            else
            {
                return Program.FALSE;
            }
        }
        public _Task FirstTaskInQueue(_Task queue)
        {
            if (IsEmptyTaskQueue(queue) == Program.TRUE)
            {
                return null;
            }
            else
            {
                return queue.next_Task;
            }
        }
        public _Task HpTaskInQueue(_Task queue)
        {
            _Task task, t;
            if (IsEmptyTaskQueue(queue) == Program.TRUE)
            {
                return null;
            }
            for (task = queue.next_Task, t = task.next_Task; t != queue; t = t.next_Task)
            {
                if (t.priority > task.priority)
                {
                    task = t;
                }
            }
            return task;
        }
        public void RemoveTaskFromQueue(_Task task)
        {
            task.prev_Task.next_Task = task.next_Task;
            task.next_Task.prev_Task = task.prev_Task;
            task.next_Task = task.prev_Task = task;
        }
        public void AddTaskToQueue(_Task task,_Task queue)
        {
            task.prev_Task = queue.prev_Task;
            task.next_Task = queue;
            queue.prev_Task.next_Task = task;
            queue.prev_Task = task;
        }
        public void PrintTask(_Task task)
        {
            Console.Write("\n\n[id={0}, priority={1}, ", task.id, task.priority);
            switch (task.state)
            {
                case Program.TASKREADYSTATE:
                    Console.Write("READY,   ");
                    break;
                case Program.TASKRUNNINGSTATE:
                    Console.Write("RUNNING, ");
                    break;
                case Program.TASKBLOCKEDSTATE:
                    Console.Write("BLOCKED, ");
                    break;
                case Program.TASKEXITSTATE:
                    Console.Write("EXIT,    ");
                    break;
                default:
                    Console.Write("Unknown, ");
                    break;
            }
            Console.Write("create={0}, exit={1}] ", task.createtime, task.exittime);
            Console.Write("\n Time[cpu={0}, io={1}, ", task.cputime, task.iotime);
            Console.Write("ready={0}, ioready={1}] ", task.readytime, task.ioreadytime);
            Console.Write("\n Part[{0}, {1}]==", task.partnumber, task.exitpartnumber);
            Program.parthandler.PrintPartQueue(task.partqueue);
            Program.parthandler.PrintPartQueue(task.exitpartqueue);

            Console.Write("\n Ratio[cpu={0}, io={1}, ", task.cpuratio, task.ioratio);
            Console.Write("rea={0}, ior={1}]", task.readyratio, task.ioreadyratio);

            Console.Write("\n Number[schedule={0}, io={1},timeout={2}] ", task.schedulenumber, task.ionumber, task.timeoutnumber);
        }
        public void PrintTaskQueue(_Task queue)
        {
            _Task task;
            for (task = queue.next_Task; task != queue; task = task.next_Task)
            {
                PrintTask(task);
            }
        }
    }
}
