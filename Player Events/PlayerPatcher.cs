using HarmonyLib;
using System;
using TwitchInteraction.Player_Events.Models;
using UnityEngine;

namespace TwitchInteraction.Player_Events
{

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    internal class Player_Update_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            TimerCooldown.Update();
            try
            {

                while (EventLookup.TimedActionsQueue.Count > 0)
                {
                    // Execute all the timed event cleanup code BEFORE any of the other events
                    Tuple<string, string, TimedEventInfo> localAction = EventLookup.TimedActionsQueue[0];
                    localAction.Item3.TimedAction.Invoke();
                    EventLookup.TimedActionsQueue.RemoveAt(0);
                }

                if (EventLookup.ActionQueue.Count > 0)
                {
                    Tuple<string, string, EventInfo> localEventInfo = null;
                    int eventIndex = -1;

                    for (int i = 0; i < EventLookup.ActionQueue.Count; i++)
                    {
                        Tuple<string, string, EventInfo> data = EventLookup.ActionQueue[i];
                        if (!EventLookup.IsRunningOrCooldown(data.Item1))
                        {
                            // Safe to use, doesnt have cooldown / is currently in use
                            localEventInfo = data;
                            eventIndex = i;
                        }
                    }

                    if (eventIndex >= 0)
                    {
                        EventLookup.ActionQueue.RemoveAt(eventIndex);
                        TimerCooldown.RemoveQueueText(localEventInfo.Item1);
                        if (localEventInfo.Item3 is TimedEventInfo)
                        {
                            // If its a timed event, pass that info to the TimerCooldown
                            TimedEventInfo timedEventInfo = (TimedEventInfo)localEventInfo.Item3;
                            TimerCooldown.AddCooldown(localEventInfo.Item1, new Tuple<string, string, TimedEventInfo>(localEventInfo.Item1, localEventInfo.Item2, timedEventInfo));
                            // Don't start the cooldown yet, cooldown starts after the cleanup code
                        }
                        else
                        {
                            // Otherwise quickly make it pop up in the UI
                            TimerCooldown.AddCooldown(localEventInfo.Item1, 1, localEventInfo);
                            EventLookup.Cooldowns.Add(localEventInfo.Item1, Time.time);
                            TimerCooldown.AddCooldownText(localEventInfo.Item1, localEventInfo.Item3.CooldownSeconds, localEventInfo);
                        }

                        if (localEventInfo.Item3.RequiresUsername)
                        {
                            localEventInfo.Item3.ActionWithUsername.Invoke(localEventInfo.Item2);
                        } else
                        {
                            localEventInfo.Item3.Action.Invoke();
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to invoke action " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Start")]
    internal class Player_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            TimerCooldown.Initialise();
        }
    }

}
