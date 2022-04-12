using System;

namespace TwitchInteraction.Player_Events.Models
{
    public class EventInfo
    {
        public Action Action;
        public Action<string> ActionWithUsername;
        public Boolean RequiresUsername;

        public int BitCost;
        public int CooldownSeconds;

        public EventInfo(Action action, int bitCost, int cooldownSeconds)
        {
            Action = action;
            BitCost = bitCost;
            CooldownSeconds = cooldownSeconds;
            RequiresUsername = false;
        }

        public EventInfo(Action<string> action, int bitCost, int cooldownSeconds)
        {
            ActionWithUsername = action;
            BitCost = bitCost;
            CooldownSeconds = cooldownSeconds;
            RequiresUsername = true;
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
