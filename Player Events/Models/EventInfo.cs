using System;

namespace TwitchInteraction.Player_Events.Models
{
    public class EventInfo
    {
        public Action Action;
        public int BitCost;
        public int CooldownSeconds;

        public EventInfo(Action action, int bitCost, int cooldownSeconds)
        {
            Action = action;
            BitCost = bitCost;
            CooldownSeconds = cooldownSeconds;
        }
    }

    public class TimedEventInfo : EventInfo
    {
        public Action TimedAction;
        public int TimerLength;

        public TimedEventInfo(Action action, int bitCost, int cooldownSeconds, Action timedAction, int timerLength) : base(action, bitCost, cooldownSeconds)
        {
            TimedAction = timedAction;
            TimerLength = timerLength;
        }

    }

}
