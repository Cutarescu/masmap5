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
        Dictionary<string, string> taskWorkPart = new Dictionary<string, string>();
        Dictionary<string, List<string>> taskRespondingAgents = new Dictionary<string, List<string>>();
        Dictionary<string, List<int>> taskAgentsCapacity = new Dictionary<string, List<int>>();

        public override void Act(Message message)
        {
            string action, parameters, taskName, valueOfPartWork;
            Utils.ParseMessageProcessorAgent(message.Content, out action, out taskName, out valueOfPartWork, out parameters);

            Console.WriteLine("   [{0}]: received {1} from [{2}]", this.Name, action, message.Sender);

            //dispatcherul primeste action, taskname si elementele care trebuie procesate
            switch (action)
            {
                case "[Extra-work]":
                    taskAssociatedWorkload[taskName] = Utils.ParseStringToArray(parameters);
                    taskResponseCount[taskName] = 0;
                    taskAgentsCapacity[taskName] = new List<int>();
                    taskRespondingAgents[taskName] = new List<string>();
                    taskWorkPart[taskName] = valueOfPartWork;
                    List<string> otherProcessorAgents = Utils.processorAgents.Where(agentName => agentName != message.Sender).ToList();
                    Broadcast( otherProcessorAgents, string.Format("[Check-load] {0} {1}", taskName, valueOfPartWork));
                    break;

                case "[Capacity]":
                    addResponse(message.Sender, Convert.ToInt32(parameters), taskName);
                    if (taskResponseCount[taskName] == Utils.NoOfProcessorAgent - 1)
                    {
                        if (taskAgentsCapacity[taskName].Sum() > Utils.helperAgentWorkload)
                        {
                            //sortare
                            string maxAgent = getMaxAgentCapacity(taskName);
                            int[] workLoad = taskAssociatedWorkload[taskName];
                            Send(maxAgent, string.Format("[Work] {0} {1} {2}", taskName, valueOfPartWork, Utils.ParseArrayToString(workLoad)));

                            //computeWithHelpers(taskName);
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

        private string getMaxAgentCapacity(string taskName)
        {
            int max = -1;
            string agentName = "ProcessorAgent1";
            for (int i = 0; i < taskAgentsCapacity[taskName].Count; i++)
            {
                if (max < taskAgentsCapacity[taskName][i])
                {
                    max = taskAgentsCapacity[taskName][i];
                    agentName = taskRespondingAgents[taskName][i];
                }
            }
            return agentName;
        }

        private void clearMaps(string taskName)
        {
            taskAssociatedWorkload.Remove(taskName);
            taskResponseCount.Remove(taskName);
            taskRespondingAgents.Remove(taskName);
            taskAgentsCapacity.Remove(taskName);
            taskWorkPart.Remove(taskName);
        }

        private void computeWithHelpers(string taskName)
        {
            
            double partWork = Utils.GetHowMuchIWork(Convert.ToDouble(taskWorkPart[taskName]), taskAssociatedWorkload[taskName].Length / Utils.helperAgentWorkload);
            while (taskAssociatedWorkload[taskName].Length > 0)
            {
                int[] workLoad = taskAssociatedWorkload[taskName].Where((source, index) => index < Utils.helperAgentWorkload).ToArray();
                var helperAgent = new HelperAgents(workLoad, taskName, partWork);
                this.Environment.Add(helperAgent, "helperAgent");
                helperAgent.Start();
                taskAssociatedWorkload[taskName] = taskAssociatedWorkload[taskName].SkipWhile((source, index) => index < Utils.helperAgentWorkload).ToArray();
            }
        }

        private void addResponse(string agentName, int capacity, string taskName)
        {

            taskAgentsCapacity[taskName].Add(capacity);
            taskRespondingAgents[taskName].Add(agentName);
            taskResponseCount[taskName]++;
        }

        /*
        int insertIndex = taskResponseCount[taskName];
        for(int i = 0; i< taskResponseCount[taskName]; i++)
        {
            if (capacity > taskAgentsCapacity[taskName][i])
                insertIndex = i;
                break;
        }
=============================================
        for (int i = 0; i<Utils.NoOfProcessorAgent; i++){

        int capacity = taskAgentsCapacity[taskName][i];
        if(capacity > 0 && taskAssociatedWorkload[taskName].Length > 0)
        {
            int[] workLoad = taskAssociatedWorkload[taskName].Where((source, index) => index < capacity).ToArray();
            Send("ProcessorAgent1", string.Format("[Work] {0} {1}", taskName, Utils.ParseArrayToString(workLoad)));
            taskAssociatedWorkload[taskName] = taskAssociatedWorkload[taskName].SkipWhile((source, index) => index<capacity).ToArray();
        }
        else
        {
            break;
        }*/

    }
}
