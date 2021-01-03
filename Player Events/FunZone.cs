using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public static void EnableGun()
        {
            StoryGoalCustomEventHandler.main.gunDisabled = false;

        }

        public static void FillOxygen()
        {
            Player.main.oxygenMgr.AddOxygen(Player.main.GetOxygenAvailable() + 3);            
        }

        public static void RandomMouseSens()
        {
            System.Random random = new System.Random();
            GameInput.SetMouseSensitivity((float)random.NextDouble());
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
    }
}

