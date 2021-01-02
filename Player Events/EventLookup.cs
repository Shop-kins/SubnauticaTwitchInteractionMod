using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchInteraction.Player_Events
{
    public static class EventLookup
    {

        private static readonly Dictionary<string, Action> EventDictionary = new Dictionary<string, Action>(){
            { "Rip Riley [Integration]", DangerZone.KillPlayer  }
        };

        public static void Lookup(string EventText)
        {
            if (EventDictionary.Keys.Contains(EventText))
                EventDictionary[EventText].Invoke();
        }
   
    }
}
