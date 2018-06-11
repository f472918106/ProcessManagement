using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManagement.Models;

namespace ProcessManagement.Controllers
{
    public class CpuHandler
    {
        public void InitCpu(CPU cpu,int policy)
        {
            cpu.state = Program.FREE;
            cpu.policy = policy;
            cpu.runningtask = null;

            cpu.busytime = cpu.freetime = 0;

            cpu.schedulenumber = 0;
            cpu.utilization = 0.0;
        }
        public void PrintCpu(CPU cpu)
        {
            Console.Write("\nCPU:");
            switch (cpu.policy)
            {
                case Program.FCFS:
                    Console.Write("FCFS, ");
                    break;
                case Program.HPF:
                    Console.Write("HPF, ");
                    break;
                case Program.FCFSRR:
                    Console.Write("FCRR, ");
                    break;
                case Program.HPFRR:
                    Console.Write("HRPR, ");
                    break;
                default:
                    Console.Write("Unknown, ");
                    break;
            }
            Console.Write("busy={0}, free={1}, schedulenumber={2}, utilization={3}", cpu.busytime, cpu.freetime, cpu.schedulenumber, cpu.utilization);
        }
    }
}
