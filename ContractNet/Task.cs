using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractNet
{
    class Task
    {
        public string name;
        public int result;
        public double partWorkDone;

        public Task()
        {
            name = "default";
            result = 0;
            partWorkDone = 0;
        }

        public Task(String name, int result, double partWorkDone)
        {
            this.name = name;
            this.result = result;
            this.partWorkDone = partWorkDone;
        }
    }
}
