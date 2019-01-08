using ActressMas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractNet
{
    public class DispatcherAgent : Agent
    {

        Dictionary<string, int[]> taskAssociatedWorkload = new Dictionary<string, int[]>();
        Dictionary<string, int> taskResponseCount = new Dictionary<string, int>();
        Dictionary<string, List<string>> taskRespondingAgents = new Dictionary<string, List<string>>();
        Dictionary<string, List<int>> taskAgentsCapacity = new Dictionary<string, List<int>>();

        public override void Act(Message message)
        {
            string action, parameters, taskName;
            Utils.ParseMessageWithActionTaskname(message.Content, out action, out taskName, out parameters);

            Console.WriteLine("   [{0}]: received {1} from [{2}]", this.Name, action, message.Sender);

            //dispatcherul primeste action, taskname si elementele care trebuie procesate
            switch (action)
            {
                case "[Extra-work]":
                    taskAssociatedWorkload[taskName] = Utils.ParseStringToArray(parameters);
                    taskResponseCount[taskName] = 0;
                    taskAgentsCapacity[taskName] = new List<int>();
                    taskRespondingAgents[taskName] = new List<string>();
                    List<string> otherProcessorAgents = Utils.processorAgents.Where(agentName => agentName != message.Sender).ToList();
                    Broadcast( otherProcessorAgents, string.Format("[Check-load] {0}", taskName));
                    break;

                case "[Capacity]":
                    addResponse(message.Sender, Convert.ToInt32(parameters), taskName);
                    if (taskResponseCount[taskName] == Utils.NoOfProcessorAgent - 1)
                    {
                        if (taskAgentsCapacity[taskName].Sum() != 0)
                        {
                            for (int i = 0; i < Utils.NoOfProcessorAgent; i++){
                                string maxAgent = taskRespondingAgents[taskName][i];
                                int capacity = taskAgentsCapacity[taskName][i];
                                if(capacity > 0 && taskAssociatedWorkload[taskName].Length > 0)
                                {
                                    int[] workLoad = taskAssociatedWorkload[taskName].Where((source, index) => index < capacity).ToArray();
                                    Send("ProcessorAgent1", string.Format("[Work] {0} {1}", taskName, Utils.ParseArrayToString(workLoad)));
                                    taskAssociatedWorkload[taskName] = taskAssociatedWorkload[taskName].SkipWhile((source, index) => index < capacity).ToArray();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            computeWithHelpers(taskName);
                            clearMaps(taskName);
                        }
                        else
                        {
                            computeWithHelpers(taskName);
                            clearMaps(taskName);
                        }
                    }
                    break;
            }

        }

        private void clearMaps(string taskName)
        {
            taskAssociatedWorkload.Remove(taskName);
            taskResponseCount.Remove(taskName);
            taskRespondingAgents.Remove(taskName);
            taskAgentsCapacity.Remove(taskName);
        }

        private void computeWithHelpers(string taskName)
        {
            while (taskAssociatedWorkload[taskName].Length > 0)
            {
                int[] workLoad = taskAssociatedWorkload[taskName].Where((source, index) => index < Utils.helperAgentWorkload).ToArray();
                var helperAgent = new HelperAgents(workLoad, taskName);
                this.Environment.Add(helperAgent, "helperAgent");
                helperAgent.Start();
                taskAssociatedWorkload[taskName] = taskAssociatedWorkload[taskName].SkipWhile((source, index) => index < Utils.helperAgentWorkload).ToArray();
            }
        }

        private void addResponse(string agentName, int capacity, string taskName)
        {
            int insertIndex = 0;
            for(int i = 0; i < taskResponseCount[taskName]; i++)
            {
                insertIndex = i;
                if (capacity > taskAgentsCapacity[taskName][i])
                    break;
            }
            taskAgentsCapacity[taskName].Insert(insertIndex, capacity);
            taskRespondingAgents[taskName].Insert(insertIndex, agentName);
            taskResponseCount[taskName]++;
        }

    }
}
