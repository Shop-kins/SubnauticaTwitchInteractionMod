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

        public static void RandomMouseSens()
        {
            initialMouseSens = GameInput.GetMouseSensitivity();

            System.Random random = new System.Random();
            GameInput.SetMouseSensitivity((float)random.NextDouble());
        }

        public static void CleanupRandomMouseSens()
        {
            GameInput.SetMouseSensitivity(initialMouseSens);
        }

        public static void hideHUD()
        {
            HUDHandler.Hide(HideForScreenshots.HideType.Mask | HideForScreenshots.HideType.HUD);
        }

        public static void showHUD()
        {
            if (MiscSettings.fieldOfView < 40)
            {
                HUDHandler.Hide(HideForScreenshots.HideType.None);
                HUDHandler.Hide(HideForScreenshots.HideType.Mask);
            } else
            {
                HUDHandler.Hide(HideForScreenshots.HideType.None);
            }
           
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

            TechType[] blueprintTech = { TechType.BaseBioReactor, TechType.RocketBase, TechType.Constructor, TechType.Exosuit, TechType.BaseMoonpool, TechType.BaseNuclearReactor, TechType.PropulsionCannon, TechType.Seamoth, TechType.StasisRifle, TechType.ThermalPlant, TechType.Transfuser, TechType.Workbench, TechType.Techlight, TechType.LEDLight, TechType.CyclopsHullBlueprint, TechType.CyclopsBridgeBlueprint, TechType.CyclopsEngineBlueprint, TechType.CyclopsDockingBayBlueprint, TechType.Seaglide, TechType.Beacon, TechType.BatteryCharger, TechType.BaseObservatory, TechType.FiltrationMachine, TechType.CoffeeVendingMachine, TechType.BaseMapRoom, TechType.BaseLadder };
            int randomNum = random.Next(blueprintTech.Length);

            int counter = 0;
            while (CrafterLogic.IsCraftRecipeUnlocked(blueprintTech[randomNum]) && counter < 50)
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

        public static void randomAdvancedResources()
        {
            System.Random random = new System.Random();
            string[] resources = { "bleach", "enameledglass", "fibermesh", "glass", "lubricant", "plasteelingot", "silicone", "titaniumingot", "titanium", "aerogel", "benzene", "hydrochloricacid", "polyaniline", "aramidfibers"};

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

        private static float initialFOV;
        public static void fovRandom()
        {
            initialFOV = MiscSettings.fieldOfView;


            // this is bad
            // i did it because atto said to
            // blame him
            // it works tho
            // the weird random number thing that is 
            // I completely agree with doing random fov
            System.Random random = new System.Random();

            int lowRandNum = random.Next(5, 45);
            int highRandNum = random.Next(85, 150);

            double randCoinFlip = random.NextDouble();
            int randNum;
            if (randCoinFlip > 0.5)
            {
                randNum = highRandNum;
            }
            else
            {
                randNum = lowRandNum;
            }

            //ErrorMessage.AddMessage(randNum.ToString());
            if (randNum < 40)
            {
                HideForScreenshots.Hide(HideForScreenshots.HideType.Mask);
            }
            MiscSettings.fieldOfView = randNum;
            if (SNCameraRoot.main != null)
            {
                SNCameraRoot.main.SyncFieldOfView();
            }
        }

        public static void fovNormal()
        {
            MiscSettings.fieldOfView = initialFOV;
            if (SNCameraRoot.main != null)
            {
                SNCameraRoot.main.SyncFieldOfView();
            }

            HideForScreenshots.Hide(HideForScreenshots.HideType.None);
        }


        public static void killBadThings()
        {
            ReaperLeviathan[] Reapers = GameObject.FindObjectsOfType<ReaperLeviathan>();

            foreach (var r in Reapers)
            {
                GameObject.Destroy(r.gameObject);
                //If you're here it means you've found my secret
                DevConsole.SendConsoleCommand("spawn cutefish");
            }

            SeaDragon[] seaDragons = GameObject.FindObjectsOfType<SeaDragon>();

            foreach (var r in seaDragons)
            {
                GameObject.Destroy(r.gameObject);
                //The people in call thought it would be funny to replace the bad things with cuddlefish
                DevConsole.SendConsoleCommand("spawn cutefish");
            }

            GhostLeviathan[] ghostLeviathans = GameObject.FindObjectsOfType<GhostLeviathan>();

            foreach (var r in ghostLeviathans)
            {
                GameObject.Destroy(r.gameObject);
                //I agreed
                DevConsole.SendConsoleCommand("spawn cutefish");
            }
            Warper[] warpers = GameObject.FindObjectsOfType<Warper>();

            foreach (var r in warpers)
            {
                GameObject.Destroy(r.gameObject);
                //But also it would be very easy to just comment out the code that adds them :p
                DevConsole.SendConsoleCommand("spawn cutefish");
            }

            CrabSquid[] crabSquids = GameObject.FindObjectsOfType<CrabSquid>();

            foreach ( var r in crabSquids)
            {
                GameObject.Destroy(r.gameObject);
                DevConsole.SendConsoleCommand("spawn cutefish");
            }

        }

        public static void returnToShallows()
        {
            Vector3 spawnPos = RandomStart.main.GetRandomStartPoint();
            spawnPos.y = -2;
            Player.main.SetPosition(spawnPos);

        }

        public static void InvertControls()
        {
            InputPatch.InputPatch.invertKeyboardAxisX = true;
            InputPatch.InputPatch.invertKeyboardAxisY = true;
            InputPatch.InputPatch.invertKeyboardAxisZ = true;
            InputPatch.InputPatch.invertMouseAxisX = true;
            InputPatch.InputPatch.invertMouseAxisY = true;
        }

        public static void NormalControls()
        {
            InputPatch.InputPatch.invertKeyboardAxisX = false;
            InputPatch.InputPatch.invertKeyboardAxisY = false;
            InputPatch.InputPatch.invertKeyboardAxisZ = false;
            InputPatch.InputPatch.invertMouseAxisX = false;
            InputPatch.InputPatch.invertMouseAxisY = false;
        }

        public static void DisableControls()
        {
            InputPatch.InputPatch.controlsEnabled = false;
        }

        public static void EnableControls()
        {
            InputPatch.InputPatch.controlsEnabled = true;
        }

        public static void EnableFilmicMode()
        {
            UwePostProcessingManager.SetColorGradingMode(2);
        }

        public static void DisableFilmicMode()
        {
            UwePostProcessingManager.SetColorGradingMode(0);
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

            foreach (PlayerTool playerTool in playerTools)
            {
                EnergyMixin toolEnergyMixin = playerTool.GetComponent<EnergyMixin>();

                // This is a tool, not something like a floater
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

        public static void DumpEquipment()
        {
        	int equipmentCount = 0;

            // Count the hotbar tools
            List<ItemsContainer.ItemGroup> itemGroups = new List<ItemsContainer.ItemGroup>(Inventory.main.quickSlots.container._items.Values);
            List<InventoryItem> hotbarTools = new List<InventoryItem>();
            for (int i = 0; i < Inventory.main.quickSlots.binding.Length; i++)
            {
                if (Inventory.main.quickSlots.binding[i] != null)
                {
                	equipmentCount++;
                	hotbarTools.Add(Inventory.main.quickSlots.binding[i]);
                }
            }


            // These are hardcoded in the Inventory class to, so why bother
            string[] inventoryEquipmentSlots = new string[]
            {
                "Head",
                "Body",
                "Gloves",
                "Foots",
                "Chip1",
                "Chip2",
                "Tank"
            };

            List<string> equipment = new List<string>();

            // Count all equipment (O2 tank, rebreather, etc.)
            foreach (string equipmentSlot in inventoryEquipmentSlots)
            {
                InventoryItem equipmentItem;
                Inventory.main.equipment.equipment.TryGetValue(equipmentSlot, out equipmentItem);
                if (equipmentItem == null)
                {
                    continue;
                }
                equipment.Add(equipmentSlot);
                equipmentCount++;
            }

            System.Random random = new System.Random();
            int randomEquipment = random.Next(0, equipmentCount);

            if (randomEquipment < hotbarTools.Count) {
            	// Drop a hotbar tool
                Inventory.main.InternalDropItem(hotbarTools[randomEquipment].item, true);
            } else {
                // Drop a piece of equipment
                string equipmentSlot = equipment[randomEquipment - hotbarTools.Count];

                InventoryItem equipmentItem;
                Inventory.main.equipment.equipment.TryGetValue(equipmentSlot, out equipmentItem);

                // This is basically the Equipment.RemoveItem function, but a little modified
                Inventory.main.equipment.equipment[equipmentSlot] = null;
                TechType equipmentType = equipmentItem.item.GetTechType();
                Inventory.main.equipment.UpdateCount(equipmentType, false);
                Equipment.SendEquipmentEvent(equipmentItem.item, 1, Inventory.main.equipment.owner, equipmentSlot);
                Inventory.main.equipment.NotifyUnequip(equipmentSlot, equipmentItem);
                equipmentItem.container = null;
                
                // Put the equipment in the inventory
                Inventory.main._container.UnsafeAdd(equipmentItem);

                // Imediately dump it again
                Inventory.main.InternalDropItem(equipmentItem.item, true);
            }

        }

    }
}

