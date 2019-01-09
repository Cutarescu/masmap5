using ActressMas;
using System;
using System.IO;
using System.Threading;

namespace ContractNet
{
    public class DistributorAgent : Agent
    {
        int[][] inputData = new int[2][];
        int taskCount = 1;
        public DistributorAgent(int[][] inputData)
        {
            this.inputData = inputData;
        }

        public DistributorAgent()
        {
            inputData[0] = new int[] { 3, 6, 7, 3, 2, 8, 0, 6, 3, 7 };
            inputData[1] = new int[] { 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2, 5, 3, 2 };

        }

        public override void Setup()
        {
            while (true)
            {
                String file = "database_big.txt";
                StreamReader dataStream = new StreamReader(file);
                string datasample;
                while ((datasample = dataStream.ReadLine()) != null)
                {
                    //Console.WriteLine("[{0}]: Input Data = {1}", this.Name, datasample);
                    Send(Utils.processorAgents[Utils.RandNoGen.Next(Utils.processorAgents.Count)], string.Format("[Work] [Task{0}] {1} {2}", taskCount++, 1, datasample));
                    //Thread.Sleep(100);
                }
            }
        }

        public override void Act(Message message)
        {
            string action, parameters;
            Utils.ParseMessage(message.Content, out action, out parameters);

            Console.WriteLine("[{0}]: received {1} from [{2}]", this.Name, action, message.Sender);
        }
    }
}




