using HarmonyLib;
using System;
using System.Collections.Generic;
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
                    Action localAction = EventLookup.TimedActionsQueue[0];
                    localAction();
                    EventLookup.TimedActionsQueue.RemoveAt(0);
                }

                if (EventLookup.ActionQueue.Count > 0)
                {
                    KeyValuePair<string, EventInfo> localEventInfo = new KeyValuePair<string, EventInfo>();
                    int eventIndex = -1;

                    for (int i = 0; i < EventLookup.ActionQueue.Count; i++)
                    {
                        KeyValuePair<string, EventInfo> keyValuePair = EventLookup.ActionQueue[i];
                        if (!EventLookup.IsRunningOrCooldown(keyValuePair.Key))
                        {
                            // Safe to use, doesnt have cooldown / is currently in use
                            localEventInfo = keyValuePair;
                            eventIndex = i;
                        }
                    }

                    if (eventIndex >= 0)
                    {
                        EventLookup.ActionQueue.RemoveAt(eventIndex);
                        TimerCooldown.RemoveQueueText(localEventInfo.Key);
                        if (localEventInfo.Value is TimedEventInfo)
                        {
                            // If its a timed event, pass that info to the TimerCooldown
                            TimedEventInfo timedEventInfo = (TimedEventInfo)localEventInfo.Value;
                            TimerCooldown.AddCooldown(localEventInfo.Key, timedEventInfo);
                            // Don't start the cooldown yet, cooldown starts after the cleanup code
                        }
                        else
                        {
                            // Otherwise quickly make it pop up in the UI
                            TimerCooldown.AddCooldown(localEventInfo.Key, 1, localEventInfo.Value);
                            EventLookup.Cooldowns.Add(localEventInfo.Key, Time.time);
                            TimerCooldown.AddCooldownText(localEventInfo.Key, localEventInfo.Value.CooldownSeconds, localEventInfo.Value);
                        }

                        localEventInfo.Value.Action.Invoke();
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
