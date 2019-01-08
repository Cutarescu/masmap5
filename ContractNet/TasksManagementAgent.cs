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
            string result, parameters, taskName, noOfSubtasks;
            Utils.ParseMessageTasksManagement(message.Content, out result, out taskName, out noOfSubtasks, out parameters);

            Console.WriteLine("[{0}]: received from [{1}] result {2}, task name= \'{3}\' and subtasks {4}", this.Name,  message.Sender, result, taskName, noOfSubtasks);
            
            int status = Int32.Parse(noOfSubtasks);
            int intResult = Int32.Parse(result);
            //if already 1 task exist then this one has a subtask
            if (taskStatus.ContainsKey(taskName))
            {
                taskStatus[taskName].noOfSubtasks += status;
                taskStatus[taskName].result += intResult;
            } else
            {
                Task newTask = new Task(taskName, status, intResult);
                taskStatus[taskName] = newTask;
            }

            //# if noOfSubtasks = 0 => task is finish
            //# else is in progress
            if (taskStatus[taskName].noOfSubtasks == 0)
            {
                Console.WriteLine("--{0} is finish with the result: {1}", taskName, taskStatus[taskName].result);
            } else
            {
                //1 subtask is finish
                if (status == 0)
                {
                    taskStatus[taskName].noOfSubtasks--;
                    if (taskStatus[taskName].noOfSubtasks == 0)
                    {
                        Console.WriteLine("**{0} is finish with the result: {1}", taskName, taskStatus[taskName].result);
                    }
                }
            }

        }
    }
}
