using Harmony;
using System;

namespace TwitchInteraction.Player_Events
{

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    internal class Player_Update_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            try
            {
                Action localAction;
                EventLookup.ActionQueue.TryDequeue(out localAction);
                localAction?.Invoke();
            } catch (Exception e)
            {
                Console.WriteLine("Failed to invoke action " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
