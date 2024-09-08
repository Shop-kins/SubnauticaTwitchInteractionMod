using System;
using System.Collections.Generic;
using System.Linq;
using TwitchInteraction.Player_Events.Models;

namespace TwitchInteraction.Player_Events
{
    public static class EventLookup
    {

        // Queue for events
        public static List<Tuple<string, string, EventInfo>> ActionQueue = new List<Tuple<string, string, EventInfo>>();

        // Queue for the cleanup code of timed events
        public static List<Tuple<string, string, TimedEventInfo>> TimedActionsQueue = new List<Tuple<string, string, TimedEventInfo>>();

        // List with currently running timed events
        public static List<string> RunningEventIDs = new List<string>();

        // List with currently running timed events
        public static Dictionary<string, float> Cooldowns = new Dictionary<string, float>();

        //MAP OF FUNZONE AND DANGER ZONE KEYS TO THEIR APPROPRIATE FUNCTIONS
        private static readonly Dictionary<string, EventInfo> EventDictionary = new Dictionary<string, EventInfo>() {
            // Parameter: ID, Action, BitCost, CooldownSeconds
            { "Rip Robin [Integration]", new EventInfo(DangerZone.KillPlayer, 500, 60)},
            { "Heal Robin [Integration]", new EventInfo(FunZone.HealPlayer, 50, 15) },
            { "Toggle Day/Night [Integration]", new EventInfo(FunZone.ToggleDayNight, 50, 30) },
            { "Open PDA [Integration]", new EventInfo(FunZone.openPDA, 50, 15) },
            { "Turn on the big gun [Integration]", new EventInfo(DangerZone.EnableGun, 200, 300) },
            { "Fill Oxygen [Integration]", new EventInfo(FunZone.FillOxygen, 50, 15) },
            { "Player Teleport [Integration]", new EventInfo(DangerZone.TeleportPlayer, 400, 180) },
            { "Cow or Reaper? Yes. [Integration]", new EventInfo(FunZone.randomSummon, 100, 180) },
            { "Fill him up with junk [Integration]", new EventInfo(FunZone.junkFill, 100, 180) },
            { "Get your pet shrimp to hang out [Integration]", new EventInfo(DangerZone.summonShrimp, 150, 180) },
            { "Resource Roulette [Integration]", new EventInfo(FunZone.randomItem, 30, 30) },
            { "Blueprint Roulette [Integration]", new EventInfo(FunZone.randomBlueprintUnlock, 100, 30) },
            { "An early breakfast [Integration]", new EventInfo(FunZone.fillFoodWater, 20, 30) },
            { "Clear a hotbar slot [Integration]", new EventInfo(FunZone.ClearRandomQuickSlot, 100, 60) },
            { "Shuffle the hotbar [Integration]", new EventInfo(FunZone.RandomizeQuickSlots, 250, 60) },
            { "Steal a battery [Integration]", new EventInfo(FunZone.RemoveRandomBattery, 150, 60) },
            { "Steal some equipment [Integration]", new EventInfo(FunZone.DumpEquipment, 250, 60) },
            { "Kill bad things [Integration]", new EventInfo(FunZone.killBadThings, 150, 60) },
            { "Crafted Roulette [Integration]", new EventInfo(FunZone.randomAdvancedResources, 60, 30) },
            { "Put your name on the map! [Integration]", new EventInfo(FunZone.SpawnUserBeacon, 50, 15) },
            // Parameter: ID, Action, BitCost, CooldownSeconds, TimedAction (Cleanup), TimerDuration
            { "Random Mouse Sensitivity [Integration]", new TimedEventInfo(FunZone.RandomMouseSens, 200, 60, FunZone.CleanupRandomMouseSens, 15) },
            { "Hide HUD [Integration]", new TimedEventInfo(FunZone.hideHUD, 50, 60, FunZone.showHUD, 60) },
            { "Invert Controls [Integration]", new TimedEventInfo(FunZone.InvertControls, 200, 60, FunZone.NormalControls, 60) },
            { "Disable Controls [Integration]", new TimedEventInfo(FunZone.DisableControls, 200, 60, FunZone.EnableControls, 10) },
            { "Random FOV [Integration]", new TimedEventInfo(FunZone.fovRandom, 1000, 60, FunZone.fovNormal, 60) },
            { "Be careful Robin [Integration]", new TimedEventInfo(DangerZone.ActivateOHKO, 500, 60, DangerZone.DeactiveOHKO, 60) },
            { "Go REALLY fast [Integration]", new TimedEventInfo(FunZone.EnableSonicMode, 100, 60, FunZone.DisableFastMode, 60) }
        };

        public static string getBitCosts()
        {
            string message = "";
            foreach (KeyValuePair<string, EventInfo> pair in EventDictionary)
            {
                var costText = pair.Key + " costs " + pair.Value.BitCost + " bits ||| ";
                message += costText;
            }
            return message;
        }

        public static void Lookup(string EventText, string User)
        {
            if (EventDictionary.Keys.Contains(EventText))
            {
                ActionQueue.Add(new Tuple<string, string, EventInfo>(EventText, User, EventDictionary[EventText]));
                TimerCooldown.AddNewEventText(EventText, User);
            }
        }

        public static Boolean IsRunningOrCooldown(string Key)
        {
            return (RunningEventIDs.Contains(Key) || Cooldowns.ContainsKey(Key));
        }

        public static void Lookup(string EventText, string User, int bits)
        {
            KeyValuePair<string, EventInfo> Event = EventDictionary.FirstOrDefault(it => EventText.IndexOf(it.Key, StringComparison.OrdinalIgnoreCase) >= 0);
            Console.WriteLine(Event.Key);
            if (!Event.Equals(default(KeyValuePair<string, EventInfo>)) && bits >= Event.Value.BitCost)
            {
                Console.WriteLine(Event.Key);
                ActionQueue.Add(new Tuple<string, string, EventInfo>(Event.Key, User, Event.Value));
                TimerCooldown.AddNewEventText(Event.Key, User, bits);
            }

        }

        public static void ConfigureEventCost(List<ConfigEventInfo> configInfo)
        {
            foreach (ConfigEventInfo i in configInfo)
            {
                if (EventDictionary.Keys.Contains(i.EventName))
                {
                    EventDictionary[i.EventName].BitCost = i.BitCost;
                    EventDictionary[i.EventName].CooldownSeconds = i.Cooldown;
                    Console.WriteLine("Updating " + i.EventName + " to cost " + i.BitCost + " with a cooldown of " + i.Cooldown);
                }
            }
        }
    }
}
