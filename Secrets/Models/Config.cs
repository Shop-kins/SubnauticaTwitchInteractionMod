using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using TwitchInteraction.Player_Events;
using UnityEngine;

namespace TwitchInteraction
{
    [Menu("Twitch Interaction - Requires Restart!!")]
    class Config : ConfigFile
    {
        [Choice("Client", new string[] { "twitch", "crowdcontrol" })]
        public string Client { get; set; } = "twitch";

        public string ClientId { get; set; }

        public string BotAccessToken { get; set; }

        public string BotRefreshToken { get; set; }

        public string UsernameToken { get; set; }

        public string UsernameId { get; set; }

        public string Username { get; set; }

        public string BotName { get; set; }

        [Toggle("Show Redemption Messages")]
        public bool ShowRedemptionMessages { get; set; } = false;

        [Toggle("Save Redemption Messages")]
        public bool SaveRedemptionMessages { get; set; } = false;

        [Button("Auth With Twitch (Requires Restart)")]
        public void AuthWithTwitch(ButtonClickedEventArgs e)
        {
            var lego = (new GameObject("SomeObjName")).AddComponent<TwitchOAuth>();
            lego.InitiateTwitchAuth(this);
        }
    }

    public class ConfigEventInfo
    {
        public ConfigEventInfo(string name, int cost, int cooldown)
        {
            this.EventName = name;
            this.BitCost = cost;
            this.Cooldown = cooldown;
        }

        public string EventName { get; set; }

        public int BitCost { get; set; }

        public int Cooldown { get; set; }
    }

    [Menu("Twitch Interaction Events")]
    [ConfigFile("events")]
    public class EventsFile : ConfigFile
    {
        [Slider("Kill Riley Bit Cost", 10, 1000, DefaultValue = 500, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Kill_BitCost { get; set; } = 500;
        [Slider("Kill Riley Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Kill_Cooldown { get; set; } = 60;

        [Slider("Heal Riley Bit Cost", 1, 1000, DefaultValue = 50, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Heal_BitCost { get; set; } = 50;
        [Slider("Heal Riley Cooldown", 10, 180, DefaultValue = 15, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Heal_Cooldown { get; set; } = 15;
        [Slider("Day/Night Riley Bit Cost", 1, 1000, DefaultValue = 50, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int DayNight_BitCost { get; set; } = 50;
        [Slider("Day/Night Riley Cooldown", 10, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int DayNight_Cooldown { get; set; } = 30;
        [Slider("Open PDA Bit Cost", 1, 1000, DefaultValue = 50, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int PDA_BitCost { get; set; } = 50;
        [Slider("Open PDA Riley Cooldown", 10, 180, DefaultValue = 15, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int PDA_Cooldown { get; set; } = 15;
        [Slider("Refill O2 Bit Cost", 1, 1000, DefaultValue = 50, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int O2_BitCost { get; set; } = 50;
        [Slider("Refill O2 Cooldown", 10, 180, DefaultValue = 15, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int O2_Cooldown { get; set; } = 15;
        [Slider("Random Creature? Yes. Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Spawn_BitCost { get; set; } = 100;
        [Slider("Random Creature? Yes. Cooldown", 10, 180, DefaultValue = 180, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Spawn_Cooldown { get; set; } = 180;
        [Slider("Junk Fill Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Junk_BitCost { get; set; } = 100;
        [Slider("Junk Fill Cooldown", 10, 180, DefaultValue = 180, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Junk_Cooldown { get; set; } = 180;
        [Slider("Spawn Shrimp Bit Cost", 1, 1000, DefaultValue = 150, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Reaper_BitCost { get; set; } = 150;
        [Slider("Spawn Shrimp Cooldown", 10, 180, DefaultValue = 180, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Repear_Cooldown { get; set; } = 180;
        [Slider("Resource Roulette Bit Cost", 1, 1000, DefaultValue = 30, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Resource_BitCost { get; set; } = 30;
        [Slider("Resourse Roulette Cooldown", 10, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Resource_Cooldown { get; set; } = 30;
        [Slider("Random Blueprint Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Blueprint_BitCost { get; set; } = 100;
        [Slider("Random Blueprint Cooldown", 10, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Blueprint_Cooldown { get; set; } = 30;
        [Slider("Early Breakfast Bit Cost", 1, 1000, DefaultValue = 20, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Eat_BitCost { get; set; } = 20;
        [Slider("Early Breakfast Cooldown", 10, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Eat_Cooldown { get; set; } = 30;
        [Slider("Clear Hotbar Slot Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int ClearHotbar_BitCost { get; set; } = 100;
        [Slider("Clear Hotbar Slot Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int ClearHotbar_Cooldown { get; set; } = 60;
        [Slider("Shuffle Hotbar Bit Cost", 1, 1000, DefaultValue = 250, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int ShuffleHotbar_BitCost { get; set; } = 250;
        [Slider("Shuffle Hotbar Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int ShuffleHotbar_Cooldown { get; set; } = 60;
        [Slider("Steal a Battery Bit Cost", 1, 1000, DefaultValue = 150, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int StealBat_BitCost { get; set; } = 150;
        [Slider("Steal a Battery Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int StealBat_Cooldown { get; set; } = 60;
        [Slider("Steal Equipment Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int StealEquip_Cooldown { get; set; } = 60;
        [Slider("Steal Equipment Bit Cost", 5, 1000, DefaultValue = 250, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int StealEquip_BitCost { get; set; } = 250;
        [Slider("Kill the Bad Things Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int KillBad_Cooldown { get; set; } = 60;
        [Slider("Kill the Bad Things Bit Cost", 5, 1000, DefaultValue = 150, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int KillBad_BitCost { get; set; } = 150;
        [Slider("Crafted Roulette Cooldown", 10, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Crafted_Cooldown { get; set; } = 30;
        [Slider("Crafted Roulette Bit Cost", 5, 1000, DefaultValue = 60, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Crafted_BitCost { get; set; } = 60;
        [Slider("Random Mouse Sensitivity Bit Cost", 1, 1000, DefaultValue = 200, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Mouse_BitCost { get; set; } = 200;
        [Slider("Random Mouse Sensitivity Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Mouse_Cooldown { get; set; } = 60;
        [Slider("Invert Controls Bit Cost", 1, 1000, DefaultValue = 200, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Invert_BitCost { get; set; } = 200;
        [Slider("Invert Controls Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Invert_Cooldown { get; set; } = 60;
        [Slider("Disable Controls Bit Cost", 1, 1000, DefaultValue = 200, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Disable_BitCost { get; set; } = 200;
        [Slider("Disable Controls Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Disable_Cooldown { get; set; } = 60;
        [Slider("Random FOV Bit Cost", 1, 1000, DefaultValue = 1000, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int FOV_BitCost { get; set; } = 1000;
        [Slider("Random FOV Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int FOV_Cooldown { get; set; } = 60;
        [Slider("One Hit KO Bit Cost", 1, 1000, DefaultValue = 500, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int OHKO_BitCost { get; set; } = 500;
        [Slider("One Hit KO Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int OHKO_Cooldown { get; set; } = 60;
        [Slider("Go Really Fast Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Fast_BitCost { get; set; } = 100;
        [Slider("Go Really Fast Cooldown", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Fast_Cooldown { get; set; } = 60;
        [Slider("Put your name on the map! Cooldown", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Beacon_Cooldown { get; set; } = 60;
        [Slider("Put your name on the map! bit cost", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Beacon_BitCost { get; set; } = 100;
        [Slider("Go HOME Robin! Cooldown", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Respawn_Cooldown { get; set; } = 60;
        [Slider("Go HOME Robin! bit cost", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Respawn_BitCost { get; set; } = 100;
        [Slider("Drop The lifepod! Cooldown", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Drop_Cooldown { get; set; } = 60;
        [Slider("Drop The lifepod! bit cost", 10, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Drop_BitCost { get; set; } = 100;



        public List<ConfigEventInfo> PopulateEventList()
        {
            var list = new List<ConfigEventInfo>();
            list.Add(new ConfigEventInfo(EventCodes.KILL, this.Kill_BitCost, this.Kill_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.HEAL, this.Heal_BitCost, this.Heal_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.DAY_NIGHT, this.DayNight_BitCost, this.DayNight_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.PDA_OPEN, this.PDA_BitCost, this.PDA_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.OXYGEN_FILL, this.O2_BitCost, this.O2_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.RANDOM_CREATURE, this.Spawn_BitCost, this.Spawn_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.JUNK_FILL, this.Junk_BitCost, this.Junk_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.LEVIATHAN, this.Reaper_BitCost, this.Repear_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.BASIC_ITEM, this.Resource_BitCost, this.Resource_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.RANDOM_BLUEPRINT, this.Blueprint_BitCost, this.Blueprint_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.FILL_SURVIVAL, this.Eat_BitCost, this.Eat_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.HOTBAR_CLEAR, this.ClearHotbar_BitCost, this.ClearHotbar_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.HOTBAR_SHUFFLE, this.ShuffleHotbar_BitCost, this.ShuffleHotbar_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.BATTERY_STEAL, this.StealBat_BitCost, this.StealBat_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.EQUIPMENT_STEAL, this.StealEquip_BitCost, this.StealEquip_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.KILL_ENEMIES, this.KillBad_BitCost, this.KillBad_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.CRAFTED_ITEM, this.Crafted_BitCost, this.Crafted_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.MOUSE_SENSITIVITY, this.Mouse_BitCost, this.Mouse_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.INVERT_CONTROLS, this.Invert_BitCost, this.Invert_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.DISABLE_CONTROLS, this.Disable_BitCost, this.Disable_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.FOV, this.FOV_BitCost, this.FOV_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.ONE_HIT_MODE, this.OHKO_BitCost, this.OHKO_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.SPEED, this.Fast_BitCost, this.Fast_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.BEACON, this.Beacon_BitCost, this.Beacon_Cooldown));
            list.Add(new ConfigEventInfo(EventCodes.RESPAWN_PLAYER, this.Respawn_Cooldown, this.Respawn_BitCost));
            list.Add(new ConfigEventInfo(EventCodes.DROP_LIFEPOD, this.Drop_Cooldown, this.Drop_BitCost));

            return list;
        }

        public void UpdateEventsData()
        {
            var newList = this.PopulateEventList();
            EventLookup.ConfigureEventCost(newList);
        }
    }

}
