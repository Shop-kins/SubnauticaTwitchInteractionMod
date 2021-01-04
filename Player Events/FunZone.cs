using System;
using System.Collections.Generic;
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
            GameInput.SetMouseSensitivity((float)random.NextDouble());

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

        private static Timer hideHudTimer;
        private static bool hideHudTimerRunning = false;
        public static void hideHUD()
        {
            if (hideHudTimerRunning)
            {
                // Stop the timer and immediately apply the next effect to prevent overlaps
                hideHudTimer.Change(Timeout.Infinite, Timeout.Infinite);
                hideHudTimer.Dispose();
            }
            HideForScreenshots.Hide(HideForScreenshots.HideType.Mask | HideForScreenshots.HideType.HUD);

            hideHudTimer = new Timer(async (e) =>
            {
                HideForScreenshots.Hide(HideForScreenshots.HideType.None);
                hideHudTimerRunning = false;
            }, null, TimeSpan.FromMinutes(1), Timeout.InfiniteTimeSpan);
            hideHudTimerRunning = true;
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
            string[] creatures = { "shocker", "ghostleviathan", "biter", "blighter", "boneshark", "crabsnake", "crabsquid", "crash", "lavalizard", "mesmer", "reaperleviathan", "seadragon", "sandshark", "stalker", "warper", "bladderfish", "boomerang", "ghostrayred", "cutefish", "eyeye", "garryfish", "gasopod", "ghostrayblue", "holefish", "hoopfish", "hoverfish", "jellyray", "lavaboomerang", "oculus", "peeper", "rabbitray", "lavaeyeye", "reefback", "reginald", "seatreader", "spadefish", "spinefish", "bleeder", "shuttlebug", "cavecrawler", "floater", "lavalarva", "rockgrub", "jumper" };

            DevConsole.SendConsoleCommand("spawn " + creatures[random.Next(creatures.Length)]);
        }

        public static void fillFoodWater()
        {
            Survival component = Player.main.GetComponent<Survival>();
            component.food += 124 - component.food;
            component.water += 100 - component.water;
        }

        public static void randomBlueprintUnlock()
        {
            System.Random random = new System.Random();

            TechType[] blueprintTech = { TechType.BaseBioReactor, TechType.Constructor, TechType.Exosuit, TechType.BaseMoonpool, TechType.BaseNuclearReactor, TechType.PropulsionCannon, TechType.Seamoth, TechType.StasisRifle, TechType.ThermalPlant, TechType.Transfuser, TechType.Workbench, TechType.Techlight, TechType.LEDLight, TechType.CyclopsHullBlueprint, TechType.CyclopsBridgeBlueprint, TechType.CyclopsEngineBlueprint, TechType.CyclopsDockingBayBlueprint, TechType.Seaglide, TechType.Beacon, TechType.BatteryCharger, TechType.BaseObservatory, TechType.FiltrationMachine, TechType.CoffeeVendingMachine, TechType.BaseMapRoom, TechType.BaseLadder };
            int randomNum = random.Next(blueprintTech.Length);

            int counter = 0;
            while (CrafterLogic.IsCraftRecipeUnlocked(blueprintTech[randomNum]) || counter > 50)
            {
                randomNum = random.Next(blueprintTech.Length);
                counter++;
            }

            if (CraftData.IsAllowed(blueprintTech[randomNum]) && KnownTech.Add(blueprintTech[randomNum], true))
            {
                ErrorMessage.AddDebug("Unlocked " + Language.main.Get(blueprintTech[randomNum].AsString(false)));
            }
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
            TechType[] listofstuff = { TechType.AcidMushroomSpore, TechType.Lead, TechType.Diamond, TechType.Magnetite, TechType.UraniniteCrystal, TechType.SeaTreaderPoop, TechType.BloodOil, TechType.SmallFanSeed, TechType.PosterAurora, TechType.DepletedReactorRod, TechType.Magnesium, TechType.MercuryOre };
            for (int i = 0; i < 48; i++)
            {
                CraftData.AddToInventory(listofstuff[random.Next(listofstuff.Length)], 1, false, false);
            }           
        }

        public static void playToothSound()
        {
            System.Random random = new System.Random();

            FMODUWE.PlayOneShot(CraftData.GetPrefabForTechType(TechType.Stalker).GetComponent<Stalker>().loseToothSound, new Vector3(Player.main.transform.position.x - random.Next(-8, 8), Player.main.transform.position.y - random.Next(-8, 7), Player.main.transform.position.z - random.Next(-7, 8)), 1f);
         

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

        private static Timer enableFilmicModeTimer;
        private static bool enableFilmicModeTimerRunning = false;

        public static void EnableFilmicMode()
        {
            UwePostProcessingManager.SetColorGradingMode(2);

            if (enableFilmicModeTimerRunning)
            {
                // Stop the timer and immediately apply the next effect to prevent overlaps
                enableFilmicModeTimer.Change(Timeout.Infinite, Timeout.Infinite);
                enableFilmicModeTimer.Dispose();
            }

            enableFilmicModeTimer = new Timer(async (e) =>
            {
                UwePostProcessingManager.SetColorGradingMode(0);
                enableFilmicModeTimerRunning = false;
            }, null, TimeSpan.FromMinutes(1), Timeout.InfiniteTimeSpan);
            enableFilmicModeTimerRunning = true;
        }

        public static void ClearRandomQuickSlot()
        {
            QuickSlots quickSlots = Inventory.main.quickSlots;

            // Find all slots which have items in them
            List<int> activeSlots = new List<int>();
            for (int i = 0; i < quickSlots.slotCount; i++)
            {
                if (quickSlots.GetSlotItem(i) != null)
                {
                    activeSlots.Add(i);
                }
            }

            if (activeSlots.Count == 0)
            {
                // Prevent OutOfBounds errors
                return;
            }

            System.Random random = new System.Random();
            int randomSlotID = activeSlots[random.Next(0, activeSlots.Count)];
            quickSlots.Unbind(randomSlotID);
        }

        public static void RandomizeQuickSlots()
        {
            QuickSlots quickSlots = Inventory.main.quickSlots;

            // Make a list of all slots (including empty slots)
            List<InventoryItem> allSlots = new List<InventoryItem>();
            for (int i = 0; i < quickSlots.slotCount; i++)
            {
                allSlots.Add(quickSlots.GetSlotItem(i));
            }

            // Shuffle the list
            System.Random random = new System.Random();
            int n = allSlots.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                InventoryItem value = allSlots[k];
                allSlots[k] = allSlots[n];
                allSlots[n] = value;
            }

            // Set the quick slots
            for (int i = 0; i < quickSlots.slotCount; i++)
            {
                quickSlots.binding[i] = allSlots[i];
            }
            // Notify the game that the slots have changed
            for (int i = 0; i < quickSlots.slotCount; i++)
            {
                quickSlots.NotifyBind(i, allSlots[i] != null);
            }

        }

        public static void RemoveRandomBattery()
        {
            PlayerTool[] playerTools = Inventory.main.gameObject.GetAllComponentsInChildren<PlayerTool>();
            List<EnergyMixin> toolMixins = new List<EnergyMixin>();

            foreach(PlayerTool playerTool in playerTools)
            {
                EnergyMixin toolEnergyMixin = playerTool.GetComponent<EnergyMixin>();
                if (toolEnergyMixin != null && toolEnergyMixin.HasItem())
                {
                    toolMixins.Add(toolEnergyMixin);
                }
            }

            if (toolMixins.Count == 0)
            {
                // Prevent OutOfBounds errors
                return;
            }

            System.Random random = new System.Random();
            int randomMixin = random.Next(0, toolMixins.Count);
            EnergyMixin energyMixin = toolMixins[randomMixin];

            InventoryItem storedBattery = energyMixin.batterySlot.storedItem;
            energyMixin.batterySlot.RemoveItem();
            Inventory.main.ForcePickup(storedBattery.item);

        }

    }
}

