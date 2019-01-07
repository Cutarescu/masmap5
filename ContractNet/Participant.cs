using ActressMas;
using System;

namespace ContractNet
{
    public class Participant : Agent
    {
        float probabilityToSendProposal = 0.8F;
        float probabilityToSolveProposal = 0.5F;

        public override void Act(Message message)
        {
            string action, parameters;
            Utils.ParseMessage(message.Content, out action, out parameters);

            Console.WriteLine("   [{0}]: received {1} from [{2}]", this.Name, action, message.Sender);

            switch (action)
            {
                case "[Call-for-proposals]":
                    if (Utils.EventOccurs(probabilityToSendProposal))
                    {
                        Send(message.Sender, "[Propose]");
                    }
                    else
                    {
                        Send(message.Sender, "[Refuse]");
                        this.Stop();
                    }
                    break;

                case "[Accept-proposal]":
                    if (Utils.EventOccurs(probabilityToSolveProposal))
                    {
                        Send(message.Sender, "[Inform-done]");
                        this.Stop();
                    }
                    else
                    {
                        Send(message.Sender, "[Failure]");
                        this.Stop();
                    }
                    break;

                case "[Reject-proposal]":
                    Console.WriteLine("   [{0}]: [{1}] rejected my proposal. Communication ended.",
                        this.Name, message.Sender);
                    this.Stop();
                    break;
            }

        }
    }
}
