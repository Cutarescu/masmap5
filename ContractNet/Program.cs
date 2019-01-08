using System.Threading;

namespace ContractNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = new ActressMas.Environment();

            for (int i = 1; i <= Utils.NoOfProcessorAgent; i++)
            {
                int agentWorkload = Utils.randomMinWorkload + Utils.RandNoGen.Next(Utils.randomMaxWorkload - Utils.randomMinWorkload);
                var processorAgent = new ProcessorAgent(agentWorkload);
                env.Add(processorAgent, string.Format("ProcessorAgent{0}", i));
                Utils.processorAgents.Add(string.Format("ProcessorAgent{0}", i));
                processorAgent.Start();
            }
            Thread.Sleep(100);

            var tasksManagement = new TasksManagement();
            env.Add(tasksManagement, "tasksManagement");
            tasksManagement.Start();
            Thread.Sleep(100);

            var distributorAgent = new DistributorAgent();
            env.Add(distributorAgent, "distributorAgent");
            distributorAgent.Start();

            var dispatcherAgent = new DispatcherAgent();
            env.Add(dispatcherAgent, "dispatcherAgent");
            dispatcherAgent.Start();
            //Thread.Sleep(100);

            env.WaitAll();
        }
    }
}
