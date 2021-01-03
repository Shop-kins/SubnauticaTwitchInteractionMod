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
            { "Rip Riley [Integration]", new EventInfo(DangerZone.KillPlayer, 200)},
            { "Heal Riley [Integration]", new EventInfo(FunZone.HealPlayer, 10) },
            { "Toggle Day/Night [Integration]", new EventInfo(FunZone.ToggleDayNight, 5) },
            { "Open PDA [Integration]", new EventInfo(FunZone.openPDA, 5) },
            { "Turn on the big gun [Integration]", new EventInfo(DangerZone.EnableGun, 50) },
            { "Fill Oxygen [Integration]", new EventInfo(FunZone.FillOxygen, 5) },
            { "Random Mouse Sensitivity [Integration]", new EventInfo(FunZone.RandomMouseSens, 5) },
            { "Hide HUD [Integration]", new EventInfo(FunZone.hideHUD, 10) },
            { "Show HUD [Integration]", new EventInfo(FunZone.showHUD, 10) },
            { "Find a new home [Integration]", new EventInfo(FunZone.LifePodWarp_Shallows, 10) }
        };

        public static string getBitCosts()
        {
            string message = "";
            foreach(KeyValuePair<string, EventInfo> pair in EventDictionary)
            {
                var costText = pair.Key + " costs " + pair.Value.BitCost + " bits ||| ";
                message += costText;
            }
            return message;
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
                Event.Value.Action.Invoke();
            }
        }
   
    }
}
