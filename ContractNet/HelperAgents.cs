using ActressMas;
using System;

namespace ContractNet
{
    public class HelperAgents : Agent
    {
        private int[] workLoad;
        private string taskName;
        private double valueOfPartWork;

        public HelperAgents(int[] workLoad, string taskName, double valueOfPartWork)
        {
            this.workLoad = workLoad;
            this.taskName = taskName;
            this.valueOfPartWork = valueOfPartWork;

        }

        public override void Setup()
        {
            Console.WriteLine("Create [{0}] with parameters : taskName={1} partToWork={2} elements=[{3}]", this.Name, taskName, valueOfPartWork, Utils.ParseArrayToString(workLoad));

            int results = doTheWork(workLoad, workLoad.Length);

            Send("tasksManagement", string.Format("{0} {1} {2}", results, taskName, valueOfPartWork));

            this.Stop();

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
