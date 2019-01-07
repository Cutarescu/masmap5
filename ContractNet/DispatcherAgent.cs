using ActressMas;
using System;

namespace ContractNet
{
    public class DispatcherAgent : Agent
    {

        public override void Act(Message message)
        {
            string action, parameters;
            Utils.ParseMessage(message.Content, out action, out parameters);

            Console.WriteLine("   [{0}]: received {1} from [{2}]", this.Name, action, message.Sender);

            //dispatcherul primeste action, taskname si elementele care trebuie procesate
            switch (action)
            {
                case "[Extra-work]":
                    //ask a processor which can do this work
                    // for pentru toti ProcessorAgents
                    // Send  [Check-load] catre toti, mai putin de la care a primit.

                    //if no answer generate HelperAgents
                    //TODO: AICI TREBUIE DE VAZUT ?
                    //TODO: DE TRATAT ACEST CAZ
                    var helperAgent = new HelperAgents();
                    this.Environment.Add(helperAgent, "helperAgent");
                    helperAgent.Start();
                    break;

                case "[Capacity]":
                    //daca am primit aceasta actiune trimitem: 
                    //taskname si elementele care trebuie procesate catre ProcessorAgent de la care am primit acest mesaj

                    //
                    break;
            }

        }
    }
}
