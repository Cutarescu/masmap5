using ActressMas;
using System;

namespace ContractNet
{
    public class DistributorAgent : Agent
    {
        int[] inputData = {5,3,5,8};

        public DistributorAgent(int[] inputData)
        {
            this.inputData = inputData;
        }

        public DistributorAgent()
        {
        }

        public override void Setup()
        {
            string inputArray = Utils.ParseArrayToString(this.inputData); 
            Console.WriteLine("[{0}]: Input Data = {1}", this.Name, inputArray);
            Send("ProcessorAgent1", string.Format("[Work] [Task1] {0}", inputArray) );
        }

        public override void Act(Message message)
        {
            string action, parameters;
            Utils.ParseMessage(message.Content, out action, out parameters);

            Console.WriteLine("[{0}]: received {1} from [{2}]", this.Name, action, message.Sender);
        }
    }
}




