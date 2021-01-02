using System;
using System.Collections.Generic;
using System.Linq;
using TwitchInteraction.Player_Events.Models;

namespace TwitchInteraction.Player_Events
{
    public static class EventLookup
    {

        //MAP OF FUNZONE AND DANGER ZONE KEYS TO THEIR APPROPRIATE FUNCTIONS
        private static readonly Dictionary<string, EventInfo> EventDictionary = new Dictionary<string, EventInfo>(){
            { "Rip Riley [Integration]", new EventInfo(DangerZone.KillPlayer, 200)  }
        };

        //LIST OF FUNZONE KEYS
        private static readonly List<string> FunList = new List<string>() {
            ""
            };


        public static int? getBitCost(string EventText)
        {
            if (EventDictionary.Keys.Contains(EventText))
                return EventDictionary[EventText].BitCost;
            return null;
        }

        public static void Lookup(string EventText)
        {
            if (EventDictionary.Keys.Contains(EventText))
                EventDictionary[EventText].Action.Invoke();
        }

        public static void Lookup(string EventText, int bits)
        {
            KeyValuePair<string, EventInfo> Event = EventDictionary.FirstOrDefault(it => EventText.Contains(it.Key));
            if (!Event.Equals(default(KeyValuePair<string, EventInfo>)) && bits >= Event.Value.BitCost)
            {
                Event.Value.BitCost += 5;
                Event.Value.Action.Invoke();
            }
        }

        //can get the same one twice w/e tho
        public static string FourRandomFunZone()
        {
            var Rand = new Random();
         
            var RandomObjects = new List<string>();

            for (int i = 0; i < 4; i++)
            {
                var Index = Rand.Next(FunList.Count);

                RandomObjects.Add(FunList[Index]);
            }
            return string.Join(",", RandomObjects);
        }
   
    }
}
