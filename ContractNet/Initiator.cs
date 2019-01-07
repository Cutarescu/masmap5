using ActressMas;
using System;

namespace ContractNet
{
    public class Initiator : Agent
    {
        float probabilityToAcceptProposal = 0.7F;

        public override void Setup()
        {
            Console.WriteLine("[{0}]: Sending proposal to participant.", this.Name);
            Send("participant", "[Call-for-proposals]");
        }

        public override void Act(Message message)
        {
            string action, parameters;
            Utils.ParseMessage(message.Content, out action, out parameters);

            Console.WriteLine("[{0}]: received {1} from [{2}]", this.Name, action, message.Sender);

            switch (action)
            {
                case "[Propose]":
                    if (Utils.EventOccurs(probabilityToAcceptProposal))
                    {
                        Send(message.Sender, "[Accept-proposal]");
                    }
                    else
                    {
                        Send(message.Sender, "[Reject-proposal]");
                        this.Stop();
                    }
                    break;

                case "[Refuse]":
                    Console.WriteLine("[{0}]: [{1}] refused to make a proposal. Communication ended.",
                        this.Name, message.Sender);
                    this.Stop();
                    break;

                case "[Inform-done]":
                    Console.WriteLine("[{0}]: [{1}] solved the problem. Communication ended.",
                        this.Name, message.Sender);
                    this.Stop();
                    break;

                case "[Failure]":
                    Console.WriteLine("[{0}]: [{1}] failed to solve the problem. Communication ended.",
                        this.Name, message.Sender);
                    this.Stop();
                    break;
            }
        }
    }
}




