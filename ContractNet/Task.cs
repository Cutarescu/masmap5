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
        public int noOfSubtasks;
        public int result;

        public Task()
        {
            name = "default";
            noOfSubtasks = 0;
            result = 0;
        }

        public Task(String name, int noOfSubtasks, int result)
        {
            this.name = name;
            this.noOfSubtasks = noOfSubtasks;
            this.result = result;
        }
    }
}
