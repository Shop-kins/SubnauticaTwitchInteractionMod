using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;
using System;
using System.Collections.Generic;
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
        public void AuthWithTwitch()
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
        [Slider("Kill Riley Bit Cost", 1, 1000, DefaultValue = 500, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Kill_BitCost { get; set; } = 500;
        [Slider("Kill Riley Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Kill_Cooldown { get; set; } = 60;

        [Slider("Heal Riley Bit Cost", 1, 1000, DefaultValue = 50, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Heal_BitCost { get; set; } = 50;
        [Slider("Heal Riley Cooldown", 5, 180, DefaultValue = 15, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Heal_Cooldown { get; set; } = 15;
        [Slider("Day/Night Riley Bit Cost", 1, 1000, DefaultValue = 50, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int DayNight_BitCost { get; set; } = 50;
        [Slider("Day/Night Riley Cooldown", 5, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int DayNight_Cooldown { get; set; } = 30;
        [Slider("Open PDA Bit Cost", 1, 1000, DefaultValue = 50, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int PDA_BitCost { get; set; } = 50;
        [Slider("Open PDA Riley Cooldown", 5, 180, DefaultValue = 15, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int PDA_Cooldown { get; set; } = 15;
        [Slider("Activate Gun Bit Cost", 1, 1000, DefaultValue = 200, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Gun_BitCost { get; set; } = 200;
        [Slider("Activate Gun Cooldown", 5, 180, DefaultValue = 300, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Gun_Cooldown { get; set; } = 300;
        [Slider("Refill O2 Bit Cost", 1, 1000, DefaultValue = 50, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int O2_BitCost { get; set; } = 50;
        [Slider("Refill O2 Cooldown", 5, 180, DefaultValue = 15, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int O2_Cooldown { get; set; } = 15;
        [Slider("Find a New Home Bit Cost", 1, 1000, DefaultValue = 150, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int NewHome_BitCost { get; set; } = 150;
        [Slider("Find a New Home Cooldown", 5, 180, DefaultValue = 180, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int NewHome_Cooldown { get; set; } = 180;
        [Slider("Teleport Riley Bit Cost", 1, 1000, DefaultValue = 400, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Teleport_BitCost { get; set; } = 400;
        [Slider("Teleport Riley Cooldown", 5, 180, DefaultValue = 180, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Teleport_Cooldown { get; set; } = 180;
        [Slider("Give Tooth Bit Cost", 1, 1000, DefaultValue = 1000, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Tooth_BitCost { get; set; } = 1000;
        [Slider("Give Tooth Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Tooth_Cooldown { get; set; } = 60;
        [Slider("Cow or Reaper? Yes. Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Spawn_BitCost { get; set; } = 100;
        [Slider("Cow or Reaper? Yes. Cooldown", 5, 180, DefaultValue = 180, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Spawn_Cooldown { get; set; } = 180;
        [Slider("Junk Fill Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Junk_BitCost { get; set; } = 100;
        [Slider("Junk Fill Cooldown", 5, 180, DefaultValue = 180, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Junk_Cooldown { get; set; } = 180;
        [Slider("Spawn Reaper Bit Cost", 1, 1000, DefaultValue = 150, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Reaper_BitCost { get; set; } = 150;
        [Slider("Spawn Reaper Cooldown", 5, 180, DefaultValue = 180, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Repear_Cooldown { get; set; } = 180;
        [Slider("Resource Roulette Bit Cost", 1, 1000, DefaultValue = 30, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Resource_BitCost { get; set; } = 30;
        [Slider("Resourse Roulette Cooldown", 5, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Resource_Cooldown { get; set; } = 30;
        [Slider("Random Blueprint Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Blueprint_BitCost { get; set; } = 100;
        [Slider("Random Blueprint Cooldown", 5, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Blueprint_Cooldown { get; set; } = 30;
        [Slider("Early Breakfast Bit Cost", 1, 1000, DefaultValue = 20, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Eat_BitCost { get; set; } = 20;
        [Slider("Early Breakfast Cooldown", 5, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Eat_Cooldown { get; set; } = 30;
        [Slider("Find a Really New Home Bit Cost", 1, 1000, DefaultValue = 300, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int ReallyNewHome_BitCost { get; set; } = 300;
        [Slider("Find a Really New Home Cooldown", 5, 300, DefaultValue = 300, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int ReallyNewHome_Cooldown { get; set; } = 300;
        [Slider("Play Tooth Sound Bit Cost", 1, 1000, DefaultValue = 500, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int ToothSound_BitCost { get; set; } = 10;
        [Slider("Play Tooth Sound Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int ToothSound_Cooldown { get; set; } = 5;
        [Slider("Clear Hotbar Slot Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int ClearHotbar_BitCost { get; set; } = 100;
        [Slider("Clear Hotbar Slot Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int ClearHotbar_Cooldown { get; set; } = 60;
        [Slider("Shuffle Hotbar Bit Cost", 1, 1000, DefaultValue = 250, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int ShuffleHotbar_BitCost { get; set; } = 250;
        [Slider("Shuffle Hotbar Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int ShuffleHotbar_Cooldown { get; set; } = 60;
        [Slider("Steal a Battery Bit Cost", 1, 1000, DefaultValue = 150, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int StealBat_BitCost { get; set; } = 150;
        [Slider("Steal a Battery Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int StealBat_Cooldown { get; set; } = 60;
        [Slider("Steal Equipment Cooldown", 1, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int StealEquip_Cooldown { get; set; } = 60;
        [Slider("Steal Equipment Bit Cost", 5, 1000, DefaultValue = 250, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int StealEquip_BitCost { get; set; } = 250;
        [Slider("Kill the Bad Things Cooldown", 1, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int KillBad_Cooldown { get; set; } = 60;
        [Slider("Kill the Bad Things Bit Cost", 5, 1000, DefaultValue = 150, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int KillBad_BitCost { get; set; } = 150;
        [Slider("Go Back Home Bit Cost", 1, 1000, DefaultValue = 150, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int GoHome_BitCost { get; set; } = 150;
        [Slider("Go Back Home Cooldown", 5, 180, DefaultValue = 180, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int GoHome_Cooldown { get; set; } = 180;
        [Slider("Crafted Roulette Cooldown", 1, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Crafted_Cooldown { get; set; } = 30;
        [Slider("Crafted Roulette Bit Cost", 5, 1000, DefaultValue = 60, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Crafted_BitCost { get; set; } = 60;
        [Slider("Random Mouse Sensitivity Bit Cost", 1, 1000, DefaultValue = 200, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Mouse_BitCost { get; set; } = 200;
        [Slider("Random Mouse Sensitivity Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Mouse_Cooldown { get; set; } = 60;
        [Slider("Hide HUD Bit Cost", 1, 1000, DefaultValue = 50, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int HUD_BitCost { get; set; } = 50;
        [Slider("Hide HUD Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int HUD_Cooldown { get; set; } = 60;
        [Slider("Invert Controls Bit Cost", 1, 1000, DefaultValue = 200, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Invert_BitCost { get; set; } = 200;
        [Slider("Invert Controls Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Invert_Cooldown { get; set; } = 60;
        [Slider("Disable Controls Bit Cost", 1, 1000, DefaultValue = 200, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Disable_BitCost { get; set; } = 200;
        [Slider("Disable Controls Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Disable_Cooldown { get; set; } = 60;
        [Slider("Light? What is Light? Cooldown", 1, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Light_Cooldown { get; set; } = 60;
        [Slider("Light? What is Light? Bit Cost", 5, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Light_BitCost { get; set; } = 100;
        [Slider("Random FOV Bit Cost", 1, 1000, DefaultValue = 1000, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int FOV_BitCost { get; set; } = 1000;
        [Slider("Random FOV Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int FOV_Cooldown { get; set; } = 60;
        [Slider("Restore Ship Bit Cost", 1, 1000, DefaultValue = 30, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Ship_BitCost { get; set; } = 30;
        [Slider("Restore Ship Cooldown", 5, 180, DefaultValue = 30, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Ship_Cooldown { get; set; } = 30;
        [Slider("One Hit KO Bit Cost", 1, 1000, DefaultValue = 500, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int OHKO_BitCost { get; set; } = 500;
        [Slider("One Hit KO Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int OHKO_Cooldown { get; set; } = 60;
        [Slider("Go Really Fast Bit Cost", 1, 1000, DefaultValue = 100, Step = 10, Format = "{0:F0} bits"), OnChange(nameof(UpdateEventsData))]
        public int Fast_BitCost { get; set; } = 100;
        [Slider("Go Really Fast Cooldown", 5, 180, DefaultValue = 60, Step = 10, Format = "{0:F0} seconds"), OnChange(nameof(UpdateEventsData))]
        public int Fast_Cooldown { get; set; } = 60;

        public List<ConfigEventInfo> PopulateEventList()
        {
            var list = new List<ConfigEventInfo>();
            list.Add(new ConfigEventInfo("Rip Riley[Integration]", this.Kill_BitCost, this.Kill_Cooldown));
            list.Add(new ConfigEventInfo("Heal Riley [Integration]", this.Heal_BitCost, this.Heal_Cooldown));
            list.Add(new ConfigEventInfo("Toggle Day/Night [Integration]", this.DayNight_BitCost, this.DayNight_Cooldown));
            list.Add(new ConfigEventInfo("Open PDA [Integration]", this.PDA_BitCost, this.PDA_Cooldown));
            list.Add(new ConfigEventInfo("Turn on the big gun [Integration]", this.Gun_BitCost, this.Gun_Cooldown));
            list.Add(new ConfigEventInfo("Fill Oxygen [Integration]", this.O2_BitCost, this.O2_Cooldown));
            list.Add(new ConfigEventInfo("Find a new home [Integration]", this.NewHome_BitCost, this.NewHome_Cooldown));
            list.Add(new ConfigEventInfo("Player Teleport [Integration]", this.Teleport_BitCost, this.Teleport_Cooldown));
            list.Add(new ConfigEventInfo("Give that beautiful tooth [Integration]", this.Tooth_BitCost, this.Tooth_Cooldown));
            list.Add(new ConfigEventInfo("Cow or Reaper? Yes. [Integration]", this.Spawn_BitCost, this.Spawn_Cooldown));
            list.Add(new ConfigEventInfo("Fill him up with junk [Integration]", this.Junk_BitCost, this.Junk_Cooldown));
            list.Add(new ConfigEventInfo("Get your pet reaper to hang out [Integration]", this.Reaper_BitCost, this.Repear_Cooldown));
            list.Add(new ConfigEventInfo("Resource Roulette [Integration]", this.Resource_BitCost, this.Resource_Cooldown));
            list.Add(new ConfigEventInfo("Blueprint Roulette [Integration]", this.Blueprint_BitCost, this.Blueprint_Cooldown));
            list.Add(new ConfigEventInfo("An early breakfast [Integration]", this.Eat_BitCost, this.Eat_Cooldown));
            list.Add(new ConfigEventInfo("Find a really new home [Integration]", this.ReallyNewHome_BitCost, this.ReallyNewHome_Cooldown));
            list.Add(new ConfigEventInfo("Play tooth drop sound [Integration]", this.ToothSound_BitCost, this.ToothSound_Cooldown));
            list.Add(new ConfigEventInfo("Clear a hotbar slot [Integration]", this.ClearHotbar_BitCost, this.ClearHotbar_Cooldown));
            list.Add(new ConfigEventInfo("Shuffle the hotbar [Integration]", this.ShuffleHotbar_BitCost, this.ShuffleHotbar_Cooldown));
            list.Add(new ConfigEventInfo("Steal a battery [Integration]", this.StealBat_BitCost, this.StealBat_Cooldown));
            list.Add(new ConfigEventInfo("Steal some equipment [Integration]", this.StealEquip_BitCost, this.StealEquip_Cooldown));
            list.Add(new ConfigEventInfo("Kill bad things [Integration]", this.KillBad_BitCost, this.KillBad_Cooldown));
            list.Add(new ConfigEventInfo("Go back home [Integration]", this.GoHome_BitCost, this.GoHome_Cooldown));
            list.Add(new ConfigEventInfo("Crafted Roulette [Integration]", this.Crafted_BitCost, this.Crafted_Cooldown));
            list.Add(new ConfigEventInfo("Random Mouse Sensitivity [Integration]", this.Mouse_BitCost, this.Mouse_Cooldown));
            list.Add(new ConfigEventInfo("Hide HUD [Integration]", this.HUD_BitCost, this.HUD_Cooldown));
            list.Add(new ConfigEventInfo("Invert Controls [Integration]", this.Invert_BitCost, this.Invert_Cooldown));
            list.Add(new ConfigEventInfo("Disable Controls [Integration]", this.Disable_BitCost, this.Disable_Cooldown));
            list.Add(new ConfigEventInfo("Light? What is light? [Integration]", this.Light_BitCost, this.Light_Cooldown));
            list.Add(new ConfigEventInfo("Random FOV [Integration]", this.FOV_BitCost, this.FOV_Cooldown));
            list.Add(new ConfigEventInfo("What explosion? [Integration]", this.Ship_BitCost, this.Ship_Cooldown));
            list.Add(new ConfigEventInfo("Be careful Riley [Integration]", this.OHKO_BitCost, this.OHKO_Cooldown));
            list.Add(new ConfigEventInfo("Go REALLY fast [Integration]", this.Fast_BitCost, this.Fast_Cooldown));

            return list;
        }

        public void UpdateEventsData()
        {
            var newList = this.PopulateEventList();
            EventLookup.ConfigureEventCost(newList);
        }
    }
}
