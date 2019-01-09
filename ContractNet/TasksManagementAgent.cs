using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;

namespace ContractNet
{
    class TasksManagementAgent : Agent
    {
        Dictionary<string, Task> taskStatus = new Dictionary<string, Task>();

        public override void Act(Message message)
        {

            //parse message: result, Task, noOfSubtasks
            string result, parameters, taskName, valueOfPartWork;
            Utils.ParseMessageTasksManagement(message.Content, out result, out taskName, out valueOfPartWork, out parameters);

            Console.WriteLine("[{0}]: received from [{1}] result {2}, task name = \'{3}\' and valueOfPartWork {4}", this.Name,  message.Sender, result, taskName, valueOfPartWork);
            
            double partWork = Convert.ToDouble(valueOfPartWork);
            int intResult = Int32.Parse(result);
            //if already 1 task exist then this one has a subtask
            //Console.WriteLine("{1}+++{0} ", intResult, taskName);
            if (taskStatus.ContainsKey(taskName))
            {
                taskStatus[taskName].result += intResult;
                taskStatus[taskName].partWorkDone += partWork;
            } else
            {
                Task newTask = new Task(taskName, intResult, partWork);
                taskStatus[taskName] = newTask;
            }
            //Console.WriteLine("{1}==={0} ", taskStatus[taskName].result, taskName);

            //# if noOfSubtasks = 0 => task is finish
            //# else is in progress
            if (isEqualToOne(taskStatus[taskName].partWorkDone))
            {
                Console.WriteLine("--{0} is finish with the result: {1}", taskName, taskStatus[taskName].result);
                int i = 0; i++;
            }
        }

        private bool isEqualToOne(double partWorkDone)
        {
            return (Math.Abs(1 - partWorkDone) < 1e-12);
        }
    }
}
