using ActressMas;
using System;

namespace ContractNet
{
    public class HelperAgents : Agent
    {
        public override void Setup()
        {
            Console.WriteLine("Create [{0}]:", this.Name);

            //TODO: do the work.
            Send("dispatcherAgent", "[done]");
        }

    }
}
