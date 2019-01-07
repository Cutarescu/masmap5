using ActressMas;
using System;
using System.Linq;

namespace ContractNet
{
    public class ProcessorAgent : Agent
    {
        int maxWorkLoad = Utils.defaultMaxWorkload;
        private int workLoad = 0;

        public ProcessorAgent(int maxWorkLoad)
        {
            this.maxWorkLoad = maxWorkLoad;
        }

        public override void Setup()
        {
            Console.WriteLine("[{0}]: Work load = {1}", this.Name, this.maxWorkLoad);
        }

        public override void Act(Message message)
        {
            string action, parameters, taskName;
            Utils.ParseMessageWithActionTaskname(message.Content, out action, out taskName, out parameters);

            Console.WriteLine("[{0}]: received {1} from [{2}] parameters: {3}, task name= {4}.", this.Name, action, message.Sender, parameters, taskName);

            int capacity = this.maxWorkLoad - this.workLoad;
            switch (action)
            {
                case "[Work]":
                    int[] inputArray = Utils.ParseStringToArray(parameters);
                    // check if can do the work
                    if (capacity > 0)
                    {
                        //work to do is inputArray.Length;
                        //take the work only the capacity
                        int results = 0;
                        if (capacity < inputArray.Length)
                        {
                            //send extra elements & Task to dispatcher
                            int[] extraWorkLoad = inputArray.Where((source, index) => index >= capacity).ToArray();
                            Send("dispatcherAgent", string.Format("{0} {1} {2}", "[Extra-work]", taskName, Utils.ParseArrayToString(extraWorkLoad)));

                            this.workLoad += capacity; 
                            results = doTheWork(inputArray, capacity);
                            this.workLoad -= capacity;

                            //send TasksManagement result, Task, noOfSubtasks
                            //Send("tasksManagement", string.Format("{0} {1} {2}", results, taskName, 0));
                        }
                        else
                        {
                            this.workLoad += inputArray.Length;
                            results = doTheWork(inputArray, inputArray.Length);
                            this.workLoad -= inputArray.Length;
                            //send TasksManagement result, Task, noOfSubtasks
                            Send("tasksManagement", string.Format("{0} {1} {2}", results, taskName, 0));
                        }
                    }
                    else
                    {
                        //send [Extra-work], Task&elements to dispatcher
                        Send("dispatcherAgent", string.Format("{0} {1} {2}", "[Extra-work]", taskName, Utils.ParseArrayToString(inputArray)));
                    }

                    break;

                case "[Check-load]":
                    Send("dispatcherAgent", string.Format("{0} {1}", "[Capacity]", (capacity > 0 ? capacity : 0) ));
                    break;

            }
        }

        public int doTheWork(int[] inputArray, int capacity)
        {
            int sum = 0;
            for (int i=0; i<capacity; i++)
            {
                sum += inputArray[i];
            }

            return sum;
        }
    }
}




