using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManagement.Models;

namespace ProcessManagement.Controllers
{
    public class DeviceHandler
    {
        public void InitDevice(Device device,int policy)
        {
            device.state = Program.FREE;
            device.policy = policy;
            device.ioingtask = null;
            device.busytime = device.freetime = 0;
            device.ionumber = 0;
            device.utilization = 0.0;
        }
        public void PrintDevice(Device device)
        {
            Console.Write("\nDEVICE:");
            switch (device.policy)
            {
                case Program.FCFS:
                    Console.Write("FCFS ");
                    break;
                case Program.HPF:
                    Console.Write("HPF ");
                    break;
                default:
                    Console.Write("Unknown ");
                    break;
            }
            Console.Write("busy={0}, free={1}, ionumber={2}, utilization={3}", device.busytime, device.freetime, device.ionumber, device.utilization);
        }
    }
}
