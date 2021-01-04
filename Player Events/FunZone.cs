using System;
using System.Threading;
using UnityEngine;

namespace TwitchInteraction.Player_Events
{
    //Add functions to the EventLookup.cs class
    class FunZone // These events are mild->medium inconveniences that can be cheap or used in free votes
    {
        public static void HealPlayer()
        {
            Player.main.liveMixin.AddHealth(777f);
        }

        public static void ToggleDayNight()
        {

            DayNightCycle dayNight = DayNightCycle.main;
            bool flag = dayNight.IsDay();
            if (!flag)
            {
                dayNight.timePassedAsDouble += 1200.0 - DayNightCycle.main.timePassed % 1200.0 + 600.0;
                
                dayNight.dayNightCycleChangedEvent.Trigger(true);
            }
            else
            {
                dayNight.timePassedAsDouble += 1200.0 - DayNightCycle.main.timePassed % 1200.0;
                dayNight.dayNightCycleChangedEvent.Trigger(false);
            }

        }

        public static void openPDA()
        {

            PDA pda = Player.main.GetPDA();
            if (!pda.Open())
            {
                return;
            }
        }

        public static void FillOxygen()
        {
            Player.main.oxygenMgr.AddOxygen(Player.main.GetOxygenCapacity() - Player.main.GetOxygenAvailable());
        }

        public static void RandomMouseSens()
        {

            float currentSens = GameInput.GetMouseSensitivity();
            System.Random random = new System.Random();
            GameInput.SetMouseSensitivity((float)random.NextDouble());

            var timer = new Timer(async (e) =>
            {
                GameInput.SetMouseSensitivity(currentSens);
            }, null, TimeSpan.FromMinutes(1), Timeout.InfiniteTimeSpan);

        }

        public static void hideHUD()
        {
            HideForScreenshots.Hide(HideForScreenshots.HideType.Mask | HideForScreenshots.HideType.HUD);
        }

        public static void showHUD()
        {
            HideForScreenshots.Hide(HideForScreenshots.HideType.None);
        }
        
        public static void LifePodWarp_Shallows()
        {
            Vector3 newHome = RandomStart.main.GetRandomStartPoint();
            EscapePod.main.transform.position = newHome;
            EscapePod.main.anchorPosition = newHome;            
        }

        public static void giveTooth()
        {
            DevConsole.SendConsoleCommand("item stalkertooth");

        }


        public static void randomSummon()
        {
            System.Random random = new System.Random();
            string[] creatures = { "shocker", "biter", "blighter", "boneshark", "crabsnake", "crabsquid", "crash", "lavalizard", "mesmer", "reaperleviathan", "seadragon", "sandshark", "stalker", "warper", "bladderfish", "boomerang", "ghostrayred", "cutefish", "eyeye", "garryfish", "gasopod", "ghostrayblue", "holefish", "hoopfish", "hoverfish", "jellyray", "lavaboomerang", "oculus", "peeper", "rabbitray", "lavaeyeye", "reefback", "reginald", "seatreader", "spadefish", "spinefish", "bleeder", "shuttlebug", "cavecrawler", "floater", "lavalarva", "rockgrub", "jumper" };

            DevConsole.SendConsoleCommand("spawn " + creatures[random.Next(creatures.Length)]);
        }

        public static void randomItem()
        {
            System.Random random = new System.Random();
            string[] resources = { "acidmushroom", "seatreaderpoop", "bloodoil", "coralchunk", "crashpowder", "copper", "creepvinepiece", "creepvineseedcluster", "sulphur", "whitemushroom", "diamond", "treemushroompiece", "gaspod", "jellyplant", "gold", "kyanite", "lead", "lithium", "magnetite", "scrapmetal", "nickel", "pinkmushroom", "quartz", "aluminumoxide", "salt", "silver", "smallmelon", "purplerattle", "stalkertooth", "jeweleddiskpiece", "titanium", "uraninitecrystal" };

            DevConsole.SendConsoleCommand("item " + resources[random.Next(resources.Length)]);
        }

        public static void junkFill()
        {
            System.Random random = new System.Random();
            TechType[] listofstuff = { TechType.AcidMushroomSpore, TechType.Lead, TechType.Diamond, TechType.Magnetite, TechType.UraniniteCrystal, TechType.SeaTreaderPoop, TechType.BloodOil, TechType.SmallFanSeed, TechType.PosterAurora, TechType.ToyCar, TechType.DepletedReactorRod, TechType.Magnesium, TechType.MercuryOre };
            for(int i = 0; i < 48; i++)
            {
                CraftData.AddToInventory(listofstuff[random.Next(listofstuff.Length)], 1, false, false);
            }
        }

    }
}

