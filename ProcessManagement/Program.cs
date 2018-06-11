using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManagement.Controllers;
using ProcessManagement.Models;

namespace ProcessManagement
{
    class Program
    {
        public const int FALSE = 0;
        public const int TRUE = 1;
        public const int FREE = 0;
        public const int BUSY = 1;
        public const int CPUPART = 0;
        public const int IOPART = 1;
        public const int MAXPARTNUMBER = 10;
        public static Random random = new Random();
        public static int rand = random.Next(0, 500);
        public static int NEXTPARTNUMBER = (rand % MAXPARTNUMBER + 1);
        public const int MAXCPUPARTSIZE = 20;
        public static int NEXTCPUPARTSIZE = (rand % MAXCPUPARTSIZE + 1);
        public const int MAXIOPARTSIZE = 50;
        public static int NEXTIOPARTSIZE = (rand % MAXIOPARTSIZE + 1);
        public const int FCFS = 1;
        public const int HPF = 2;
        public const int FCFSRR = 3;
        public const int HPFRR = 4;
        public const int MAXPRIORITY = 10;
        public static int NEXTPRIORITY = (rand % MAXPRIORITY + 1);
        public const int TASKCREATEEVENT = 1;
        public const int TASKSCHEDULEEVENT = 2;
        public const int TASKTIMEOUTEVENT = 3;
        public const int TASKIOREQUESTEVENT = 4;
        public const int TASKIOSCHEDULEEVENT = 5;
        public const int TASKIOCOMPLETEEVENT = 6;
        public const int TASKRUNEXITEVENT = 7;
        public const int TASKIOEXITEVENT = 8;
        public const int MAXTASKCREATETIMEINTERVAL = 20;
        public static int NEXTTASKCREATETIMEINTERVAL = (rand % MAXTASKCREATETIMEINTERVAL + 1);
        public const int TASKREADYSTATE = 1;
        public const int TASKRUNNINGSTATE = 2;
        public const int TASKIOINGSTATE = 3;
        public const int TASKBLOCKEDSTATE = 4;
        public const int TASKEXITSTATE = 5;
        public static CpuHandler cpuhandler = new CpuHandler();
        public static DeviceHandler devicehandler = new DeviceHandler();
        public static _EventHandler eventhandler = new _EventHandler();
        public static HandleHandler handlehandler = new HandleHandler();
        public static PartHandler parthandler = new PartHandler();
        public static TaskHandler taskhandler = new TaskHandler();
        public static Event eventqueue = new Event(), exiteventqueue = new Event();
        public static _Task readytaskqueue=new _Task(), blockedtaskqueue=new _Task(), exittaskqueue=new _Task();
        public static CPU cpu = new CPU();
        public static Device device = new Device();
        public static int taskid = 1;
        public static int systemtime = 0;
        static void Main(string[] args)
        {
            int tasknumber, i;
            int cpupolicy, iopolicy;
            int createtime;
            Event _event;

            Console.WriteLine("please input operation:");
            string[] inputStr = (Console.ReadLine()).Split(new char[1] { ' ' });
            tasknumber = Convert.ToInt32(inputStr[0]);
            if (inputStr[1] == "FCFS")
            {
                cpupolicy = FCFS;
            }
            else if (inputStr[1] == "HPF")
            {
                cpupolicy = HPF;
            }
            else if (inputStr[1] == "FCFSRR")
            {
                cpupolicy = FCFSRR;
            }
            else if (inputStr[1] == "HPFRR")
            {
                cpupolicy = HPFRR;
            }
            else
            {
                cpupolicy = FCFS;
            }

            if (inputStr[2] == "HPF")
            {
                iopolicy = HPF;
            }
            else
            {
                iopolicy = FCFS;
            }

            cpuhandler.InitCpu(cpu, cpupolicy);
            devicehandler.InitDevice(device, iopolicy);

            eventhandler.InitEventQueue(eventqueue);
            eventhandler.InitEventQueue(exiteventqueue);

            taskhandler.InitTaskQueue(readytaskqueue);
            taskhandler.InitTaskQueue(blockedtaskqueue);
            taskhandler.InitTaskQueue(exittaskqueue);

            createtime = systemtime;
            for (i = 0; i < tasknumber; ++i)
            {
                createtime += NEXTTASKCREATETIMEINTERVAL;
                _event = eventhandler.CreateEvent(createtime, TASKCREATEEVENT);
                eventhandler.AddEventToQueue(_event, eventqueue);
            }

            while (eventhandler.IsEmptyEventQueue(eventqueue) == FALSE)
            {
                _event = eventhandler.FirstEventInQueue(eventqueue);
                eventhandler.RemoveEventFromQueue(_event);
                systemtime = _event.time;

                switch (_event.type)
                {
                    case TASKCREATEEVENT:
                        handlehandler.TaskCreateEventHandle(_event);
                        break;
                    case TASKSCHEDULEEVENT:
                        handlehandler.TaskScheduleEventHandle(_event);
                        break;
                    case TASKTIMEOUTEVENT:
                        handlehandler.TaskTimeoutEventHandle(_event);
                        break;
                    case TASKIOREQUESTEVENT:
                        handlehandler.TaskIoRequestEventHandle(_event);
                        break;
                    case TASKIOSCHEDULEEVENT:
                        handlehandler.TaskIoScheduleEventHandle(_event);
                        break;
                    case TASKIOCOMPLETEEVENT:
                        handlehandler.TaskIoCompleteEventHandle(_event);
                        break;
                    case TASKRUNEXITEVENT:
                        handlehandler.TaskRunExitEventHandle(_event);
                        break;
                    case TASKIOEXITEVENT:
                        handlehandler.TaskIoExitEventHandle(_event);
                        break;
                    default:
                        break;
                }
                eventhandler.AddEventToQueue(_event, exiteventqueue);
            }

            Console.Write("\n\nEXIT eventqueue");
            eventhandler.PrintEventQueue(exiteventqueue);
            Console.Write("\n\nEXIT taskqueue");
            taskhandler.PrintTaskQueue(exittaskqueue);

            cpu.freetime = systemtime - cpu.busytime;
            Console.WriteLine();
            cpuhandler.PrintCpu(cpu);

            device.freetime = systemtime - device.busytime;
            Console.WriteLine();
            devicehandler.PrintDevice(device);

            eventhandler.ClearEventQueue(exiteventqueue);
            eventhandler.ClearEventQueue(eventqueue);

            taskhandler.ClearTaskQueue(readytaskqueue);
            taskhandler.ClearTaskQueue(exittaskqueue);
            taskhandler.ClearTaskQueue(blockedtaskqueue);

            Console.Read();
        }
    }
}
