using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Player.main.oxygenMgr.RemoveOxygen(-100);

        }

        public static void FillOxygen()
        {
            Player.main.oxygenMgr.AddOxygen(Player.main.GetOxygenAvailable() + 3);

            
        }

        public static void RandomMouseSens()
        {
            Random random = new Random();
            GameInput.SetMouseSensitivity((float)random.NextDouble());
        }
    }
}

