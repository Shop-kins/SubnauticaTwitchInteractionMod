using System;
using System.Collections.Generic;
using System.Linq;
using TwitchInteraction.Player_Events.Models;
using System.Collections.Concurrent;

namespace TwitchInteraction.Player_Events
{
    public static class EventLookup
    {
        public static ConcurrentQueue<Action> ActionQueue = new ConcurrentQueue<Action>();

        //MAP OF FUNZONE AND DANGER ZONE KEYS TO THEIR APPROPRIATE FUNCTIONS
        private static readonly Dictionary<string, EventInfo> EventDictionary = new Dictionary<string, EventInfo>(){
            { "Rip Riley [Integration]", new EventInfo(DangerZone.KillPlayer, 500)},
            { "Heal Riley [Integration]", new EventInfo(FunZone.HealPlayer, 50) },
            { "Toggle Day/Night [Integration]", new EventInfo(FunZone.ToggleDayNight, 50) },
            { "Open PDA [Integration]", new EventInfo(FunZone.openPDA, 50) },
            { "Turn on the big gun [Integration]", new EventInfo(DangerZone.EnableGun, 200) },
            { "Fill Oxygen [Integration]", new EventInfo(FunZone.FillOxygen, 50) },
            { "Random Mouse Sensitivity [Integration]", new EventInfo(FunZone.RandomMouseSens, 200) },
            { "Hide HUD [Integration]", new EventInfo(FunZone.hideHUD, 50) },
            { "Find a new home [Integration]", new EventInfo(FunZone.LifePodWarp_Shallows, 150) },
            { "Player Teleport [Integration]", new EventInfo(DangerZone.TeleportPlayer, 400) },
            { "Give that beautiful tooth [Integration]", new EventInfo(FunZone.giveTooth, 10000) },
            { "Cow or Reaper? Yes. [Integration]", new EventInfo(FunZone.randomSummon, 100) },
            { "Fill him up with junk [Integration]", new EventInfo(FunZone.junkFill, 100) },
            { "Get your pet reaper to hang out [Integration]", new EventInfo(DangerZone.summonReaper, 150) },
            { "Resource Roulette [Integration]", new EventInfo(FunZone.randomItem, 30) },
            { "Blueprint Roulette [Integration]", new EventInfo(FunZone.randomBlueprintUnlock, 100) },
            { "Disable Controls [Integration]", new EventInfo(FunZone.DisableControls, 200) },
            { "Invert Controls [Integration]", new EventInfo(FunZone.InvertControls, 200) },
            { "An early breakfast [Integration]", new EventInfo(FunZone.fillFoodWater, 20) },
            { "Find a really new home [Integration]", new EventInfo(DangerZone.TeleportLifepod, 300) },
            { "Play tooth drop sound [Integration]", new EventInfo(FunZone.playToothSound, 10) },
            { "Light? What is light? [Integration]", new EventInfo(FunZone.EnableFilmicMode, 100) },
            { "Clear a hotbar slot [Integration]", new EventInfo(FunZone.ClearRandomQuickSlot, 100) },
            { "Shuffle the hotbar [Integration]", new EventInfo(FunZone.RandomizeQuickSlots, 150) },
            { "Steal a battery [Integration]", new EventInfo(FunZone.RemoveRandomBattery, 150) }
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
                ActionQueue.Enqueue(EventDictionary[EventText].Action);
        }

        public static void Lookup(string EventText, int bits)
        {
            KeyValuePair<string, EventInfo> Event = EventDictionary.FirstOrDefault(it => EventText.Contains(it.Key));
            if (!Event.Equals(default(KeyValuePair<string, EventInfo>)) && bits > Event.Value.BitCost)
            {
                ActionQueue.Enqueue(Event.Value.Action);
            }
            
        }
   
    }
}
