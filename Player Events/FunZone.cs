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
        public static void LifePodWarp_Shallows()
        {
            Vector3 newHome = RandomStart.main.GetRandomStartPoint();
            EscapePod.main.transform.position = newHome;
            EscapePod.main.anchorPosition = newHome;            
        }
    }
}
