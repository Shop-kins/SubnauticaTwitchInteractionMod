using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using TwitchInteraction.Player_Events.Models;

namespace TwitchInteraction.Player_Events
{

    public static class EventCodes
    {
        private static readonly String CHARACTER = "Robin";

        public static readonly String KILL = $"Rip {CHARACTER} [Integration]";
        public static readonly String HEAL = $"Heal {CHARACTER} [Integration]";
        public static readonly String DAY_NIGHT = "Toggle Day/Night [Integration]";
        public static readonly String PDA_OPEN = "Open PDA [Integration]";
        public static readonly String OXYGEN_FILL = "Fill Oxygen [Integration]";
        public static readonly String RANDOM_CREATURE = "Random Creature? Yes. [Integration]";
        public static readonly String JUNK_FILL = "Fill up with junk [Integration]";
        public static readonly String LEVIATHAN = "Get your pet shrimp to hang out [Integration]";
        public static readonly String BASIC_ITEM = "Resource Roulette [Integration]";
        public static readonly String RANDOM_BLUEPRINT = "Blueprint Roulette [Integration]";
        public static readonly String FILL_SURVIVAL = "An early breakfast [Integration]";
        public static readonly String HOTBAR_CLEAR = "Clear a hotbar slot [Integration]";
        public static readonly String HOTBAR_SHUFFLE = "Shuffle the hotbar [Integration]";
        public static readonly String BATTERY_STEAL = "Steal a battery [Integration]";
        public static readonly String EQUIPMENT_STEAL = "Steal some equipment [Integration]";
        public static readonly String KILL_ENEMIES = "Kill bad things [Integration]";
        public static readonly String CRAFTED_ITEM = "Crafted Roulette [Integration]";
        public static readonly String BEACON = "Put your name on the map! [Integration]";
        public static readonly String MOUSE_SENSITIVITY = "Random Mouse Sensitivity [Integration]";
        public static readonly String INVERT_CONTROLS = "Invert Controls [Integration]";
        public static readonly String DISABLE_CONTROLS = "Disable Controls [Integration]";
        public static readonly String FOV = "Random FOV [Integration]";
        public static readonly String ONE_HIT_MODE = $"Be careful {CHARACTER} [Integration]";
        public static readonly String SPEED = "Go REALLY fast [Integration]";
        public static readonly String RESPAWN_PLAYER = $"Go HOME {CHARACTER} [Integration]";
        public static readonly String DROP_LIFEPOD = "Replace The lifepod [Integration]";

    }

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
            { EventCodes.KILL, new EventInfo(DangerZone.KillPlayer, 500, 60)},
            { EventCodes.HEAL, new EventInfo(FunZone.HealPlayer, 50, 15) },
            { EventCodes.DAY_NIGHT, new EventInfo(FunZone.ToggleDayNight, 50, 30) },
            { EventCodes.PDA_OPEN, new EventInfo(FunZone.openPDA, 50, 15) },
            { EventCodes.OXYGEN_FILL, new EventInfo(FunZone.FillOxygen, 50, 15) },
            { EventCodes.RANDOM_CREATURE, new EventInfo(FunZone.randomSummon, 100, 180) },
            { EventCodes.JUNK_FILL, new EventInfo(FunZone.junkFill, 100, 180) },
            { EventCodes.LEVIATHAN, new EventInfo(DangerZone.summonShrimp, 150, 180) },
            { EventCodes.BASIC_ITEM, new EventInfo(FunZone.randomItem, 30, 30) },
            { EventCodes.RANDOM_BLUEPRINT, new EventInfo(FunZone.randomBlueprintUnlock, 100, 30) },
            { EventCodes.FILL_SURVIVAL, new EventInfo(FunZone.fillFoodWater, 20, 30) },
            { EventCodes.HOTBAR_CLEAR, new EventInfo(FunZone.ClearRandomQuickSlot, 100, 60) },
            { EventCodes.HOTBAR_SHUFFLE, new EventInfo(FunZone.RandomizeQuickSlots, 250, 60) },
            { EventCodes.BATTERY_STEAL, new EventInfo(FunZone.RemoveRandomBattery, 150, 60) },
            { EventCodes.EQUIPMENT_STEAL, new EventInfo(FunZone.DumpEquipment, 250, 60) },
            { EventCodes.KILL_ENEMIES, new EventInfo(FunZone.killBadThings, 150, 60) },
            { EventCodes.CRAFTED_ITEM, new EventInfo(FunZone.randomAdvancedResources, 60, 30) },
            { EventCodes.BEACON, new EventInfo(FunZone.SpawnUserBeacon, 50, 15) },
            { EventCodes.RESPAWN_PLAYER, new EventInfo(DangerZone.TeleportPlayerLifepod, 150, 180) },
            { EventCodes.DROP_LIFEPOD, new EventInfo(FunZone.LifepodForceDrop, 150, 180) },
            // Parameter: ID, Action, BitCost, CooldownSeconds, TimedAction (Cleanup), TimerDuration
            { EventCodes.MOUSE_SENSITIVITY, new TimedEventInfo(FunZone.RandomMouseSens, 200, 60, FunZone.CleanupRandomMouseSens, 15) },
            { EventCodes.INVERT_CONTROLS, new TimedEventInfo(FunZone.InvertControls, 200, 60, FunZone.NormalControls, 60) },
            { EventCodes.DISABLE_CONTROLS, new TimedEventInfo(FunZone.DisableControls, 200, 60, FunZone.EnableControls, 10) },
            { EventCodes.FOV, new TimedEventInfo(FunZone.fovRandom, 1000, 60, FunZone.fovNormal, 60) },
            { EventCodes.ONE_HIT_MODE, new TimedEventInfo(DangerZone.ActivateOHKO, 500, 60, DangerZone.DeactiveOHKO, 60) },
            { EventCodes.SPEED, new TimedEventInfo(FunZone.EnableSonicMode, 100, 60, FunZone.DisableFastMode, 60) },
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
