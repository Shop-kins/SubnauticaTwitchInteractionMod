using System;
using System.Threading;
using UnityEngine;
using TwitchInteraction.InputPatch;

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
            Player.main.oxygenMgr.AddOxygen(Player.main.GetOxygenAvailable() + 3);            
        }

        private static float initialMouseSens;
        private static Timer randomMouseSensTimer;
        private static bool randomMouseSensTimerRunning = false;

        public static void RandomMouseSens()
        {

            if (!randomMouseSensTimerRunning)
            {
                // Only update this when there is no timer running
                initialMouseSens = GameInput.GetMouseSensitivity();
            }

            System.Random random = new System.Random();
            GameInput.SetMouseSensitivity((float) random.NextDouble());

            if (randomMouseSensTimerRunning)
            {
                // Stop the timer and immediately apply the next effect to prevent overlaps
                randomMouseSensTimer.Change(Timeout.Infinite, Timeout.Infinite);
                randomMouseSensTimer.Dispose();
            }

            randomMouseSensTimer = new Timer(async (e) =>
            {
                GameInput.SetMouseSensitivity(initialMouseSens);
                randomMouseSensTimerRunning = false;
            }, null, TimeSpan.FromMinutes(1), Timeout.InfiniteTimeSpan);
            randomMouseSensTimerRunning = true;
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

        private static Timer invertControlsTimer;
        private static bool invertControlsTimerRunning = false;

        public static void InvertControls()
        {
            InputPatch.InputPatch.invertKeyboardAxisX = true;
            InputPatch.InputPatch.invertKeyboardAxisY = true;
            InputPatch.InputPatch.invertKeyboardAxisZ = true;
            InputPatch.InputPatch.invertMouseAxisX = true;
            InputPatch.InputPatch.invertMouseAxisY = true;

            if (invertControlsTimerRunning)
            {
                // Stop the timer and immediately apply the next effect to prevent overlaps
                invertControlsTimer.Change(Timeout.Infinite, Timeout.Infinite);
                invertControlsTimer.Dispose();
            }

            invertControlsTimer = new Timer(async (e) =>
            {
                InputPatch.InputPatch.invertKeyboardAxisX = false;
                InputPatch.InputPatch.invertKeyboardAxisY = false;
                InputPatch.InputPatch.invertKeyboardAxisZ = false;
                InputPatch.InputPatch.invertMouseAxisX = false;
                InputPatch.InputPatch.invertMouseAxisY = false;
                invertControlsTimerRunning = false;
            }, null, TimeSpan.FromMinutes(1), Timeout.InfiniteTimeSpan);
            invertControlsTimerRunning = true;
        }

        private static Timer disableControlsTimer;
        private static bool disableControlsTimerRunning = false;

        public static void DisableControls()
        {
            InputPatch.InputPatch.controlsEnabled = false;

            if (disableControlsTimerRunning)
            {
                // Stop the timer and immediately apply the next effect to prevent overlaps
                disableControlsTimer.Change(Timeout.Infinite, Timeout.Infinite);
                disableControlsTimer.Dispose();
            }

            disableControlsTimer = new Timer(async (e) =>
            {
                InputPatch.InputPatch.controlsEnabled = true;
                disableControlsTimerRunning = false;
            }, null, TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan);
            disableControlsTimerRunning = true;
        }

    }
}

