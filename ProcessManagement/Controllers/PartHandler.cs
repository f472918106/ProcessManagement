using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManagement.Models;

namespace ProcessManagement.Controllers
{
    public class PartHandler
    {
        public Part CreatePart(int type,int size)
        {
            Part part=new Part();
            if (part == null)
            {
                return null;
            }
            part.type = type;
            part.size = size;
            part.prev_Part = part.next_Part = part;
            return part;
        }
        public void DestroyPart(Part part)
        {
            IDisposable disposable = part as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        public void InitPartQueue(Part queue)
        {
            queue.next_Part = queue.prev_Part = queue;
        }
        public void ClearPartQueue(Part queue)
        {
            Part part;
            while (IsEmptyPartQueue(queue) == Program.FALSE)
            {
                part = FirstPartInQueue(queue);
                RemovePartFromQueue(part);
                DestroyPart(part);
            }
            InitPartQueue(queue);
        }
        public int IsEmptyPartQueue(Part queue)
        {
            if (queue.next_Part == queue)
            {
                return Program.TRUE;
            }
            else
            {
                return Program.FALSE;
            }
        }
        public int SizeofPartQueue(Part queue)
        {
            Part part;
            int n;
            for (n = 0, part = queue.next_Part; part != queue; ++n, part = part.next_Part) ;
            return n;
        }
        public void PrintPart(Part part)
        {
            switch (part.type)
            {
                case Program.CPUPART:
                    Console.Write("[c");
                    break;
                case Program.IOPART:
                    Console.Write("[i");
                    break;
                default:
                    Console.Write("[n");
                    break;
            }
            Console.Write("{0}]", part.size);
        }
        public void PrintPartQueue(Part queue)
        {
            Part part;
            for (part = queue.next_Part; part != queue; part = part.next_Part)
            {
                PrintPart(part);
            }
        }
        public Part FirstPartInQueue(Part queue)
        {
            if (IsEmptyPartQueue(queue) == Program.TRUE)
            {
                return null;
            }
            else
            {
                return queue.next_Part;
            }
        }
        public Part LastPartInQueue(Part queue)
        {
            if (IsEmptyPartQueue(queue) == Program.TRUE)
            {
                return null;
            }
            else
            {
                return queue.prev_Part;
            }
        }
        public void RemovePartFromQueue(Part part)
        {
            part.prev_Part.next_Part = part.next_Part;
            part.next_Part.prev_Part = part.prev_Part;
            part.prev_Part = part.next_Part = part;
        }
        public void AddPartToQueue(Part part,Part queue)
        {
            part.prev_Part = queue.prev_Part;
            part.next_Part = queue;
            queue.prev_Part.next_Part = part;
            queue.prev_Part = part;
        }
        public void AddPartsToQueue(int partnumber,Part queue)
        {
            int i;
            Part part;
            for (i = 0; i < partnumber; ++i)
            {
                part = CreatePart(i % 2 == 0 ? Program.CPUPART : Program.IOPART, i % 2 == 0 ? Program.NEXTCPUPARTSIZE : Program.NEXTIOPARTSIZE);
                if (part != null)
                {
                    AddPartToQueue(part, queue);
                }
            }
        }
    }
}
