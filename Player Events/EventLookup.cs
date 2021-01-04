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
            { "Rip Riley [Integration]", new EventInfo(DangerZone.KillPlayer, 300)},
            { "Heal Riley [Integration]", new EventInfo(FunZone.HealPlayer, 50) },
            { "Toggle Day/Night [Integration]", new EventInfo(FunZone.ToggleDayNight, 50) },
            { "Open PDA [Integration]", new EventInfo(FunZone.openPDA, 50) },
            { "Turn on the big gun [Integration]", new EventInfo(DangerZone.EnableGun, 50) },
            { "Fill Oxygen [Integration]", new EventInfo(FunZone.FillOxygen, 50) },
            { "Random Mouse Sensitivity [Integration]", new EventInfo(FunZone.RandomMouseSens, 100) },
            { "Hide HUD [Integration]", new EventInfo(FunZone.hideHUD, 50) },
            { "Show HUD [Integration]", new EventInfo(FunZone.showHUD, 50) },
            { "Find a new home [Integration]", new EventInfo(FunZone.LifePodWarp_Shallows, 100) },
            { "Player Teleport [Integration]", new EventInfo(DangerZone.TeleportPlayer, 200) },
            { "Give that beautiful tooth [Integration]", new EventInfo(FunZone.giveTooth, 10000) },
            { "Cow or Reaper? Yes. [Integration]", new EventInfo(FunZone.randomSummon, 100) },
            { "Fill him up with junk [Integration]", new EventInfo(FunZone.junkFill, 50) },
            { "Get your pet reaper to hang out [Integration]", new EventInfo(DangerZone.summonReaper, 150) },
            { "Resource Roulette [Integration]", new EventInfo(FunZone.randomItem, 100) },
            { "Invert Controls [Integration]", new EventInfo(FunZone.InvertControls, 200) },
            { "Disable Controls [Integration]", new EventInfo(FunZone.DisableControls, 100) }
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

        public static string Lookup(string EventText, int bits)
        {
            KeyValuePair<string, EventInfo> Event = EventDictionary.FirstOrDefault(it => EventText.Contains(it.Key));
            if (!Event.Equals(default(KeyValuePair<string, EventInfo>)))
            {
                if (bits >= Event.Value.BitCost)
                {
                    Event.Value.Action.Invoke();
                    return Event.Key + ":Activated";
                } else
                {
                    return Event.Key + ":Not enough Bits";
                }
            }
            return "";
            
        }
   
    }
}
