using System;

namespace TwitchInteraction.Player_Events.Models
{
    class EventInfo
    {
        public Action Action;
        public int BitCost;

        public EventInfo(Action act, int cst)
        {
            Action = act;
            BitCost = cst;
        }
    }
}
