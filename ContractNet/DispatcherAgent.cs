using ActressMas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContractNet
{
    public class DispatcherAgent : Agent
    {
        private int currentCount = 0;
        private int[] currentWorkload;
        private List<string> respondingAgents = new List<string>();
        private List<int> agentsCapacity = new List<int>();

        //Dictionary<string, int[]> taskAssociatedWorkload = new Dictionary<string, int[]>();
        //Dictionary<string, int> taskResponseCount = new Dictionary<string, int>();

        public override void Act(Message message)
        {
            string action, parameters, taskName;
            Utils.ParseMessageWithActionTaskname(message.Content, out action, out taskName, out parameters);

            Console.WriteLine("   [{0}]: received {1} from [{2}]", this.Name, action, message.Sender);

            //dispatcherul primeste action, taskname si elementele care trebuie procesate
            switch (action)
            {
                case "[Extra-work]":
                    //taskAssociatedWorkload.Add(taskName, Utils.ParseStringToArray(parameters));
                    currentWorkload = Utils.ParseStringToArray(parameters);
                    BroadcastAll("[Check-load]");
                    break;

                case "[Capacity]":
                    // in case a processor sends back a [Check-load] response, the capacity will be the second parameter ('taskname')
                    addResponse(message.Sender, Convert.ToInt32(taskName));
                    if (currentCount == Utils.NoOfProcessorAgent)
                    {
                        if (agentsCapacity.Sum() != 0)
                        {
                            for (int i = 0; i < Utils.NoOfProcessorAgent; i++){
                                string maxAgent = respondingAgents[i];
                                int capacity = agentsCapacity[i];
                                if(capacity > 0 && currentWorkload.Length > 0)
                                {
                                    int[] workLoad = currentWorkload.Where((source, index) => index < capacity).ToArray();
                                    Send("ProcessorAgent1", string.Format("[Work] {0} {1}", taskName, workLoad));
                                    currentWorkload = currentWorkload.SkipWhile((source, index) => index < capacity).ToArray();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            computeWithHelpers(taskName);
                        }
                        else
                        {
                            computeWithHelpers(taskName);
                        }
                    }
                    break;
            }

        }

        private void computeWithHelpers(string taskName)
        {
            while (currentWorkload.Length > 0)
            {
                int[] workLoad = currentWorkload.Where((source, index) => index < Utils.helperAgentWorkload).ToArray();
                var helperAgent = new HelperAgents(workLoad, taskName);
                this.Environment.Add(helperAgent, "helperAgent");
                helperAgent.Start();
                currentWorkload = currentWorkload.SkipWhile((source, index) => index < Utils.helperAgentWorkload).ToArray();
            }
        }

        private void addResponse(string agentName, int capacity)
        {
            int insertIndex = 0;
            for(int i = 0; i < currentCount; i++)
            {
                insertIndex = i;
                if (capacity > agentsCapacity[i])
                    break;
            }
            agentsCapacity.Insert(insertIndex, capacity);
            respondingAgents.Insert(insertIndex, agentName);
            currentCount++;
        }

    }
}
