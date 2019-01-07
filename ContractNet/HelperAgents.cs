using ActressMas;
using System;

namespace ContractNet
{
    public class HelperAgents : Agent
    {
        private int[] workLoad;
        private string taskName;

        public HelperAgents(int[] workLoad, string taskName)
        {
            this.workLoad = workLoad;
            this.taskName = taskName;
        }

        public override void Setup()
        {
            Console.WriteLine("Create [{0}]:", this.Name);

            int results = doTheWork(workLoad, workLoad.Length);

            Send("tasksManagement", string.Format("{0} {1} {2}", results, taskName, 0));

        }

        private int doTheWork(int[] inputArray, int capacity)
        {
            int sum = 0;
            for (int i = 0; i < capacity; i++)
            {
                sum += inputArray[i];
            }

            return sum;
        }

    }
}
