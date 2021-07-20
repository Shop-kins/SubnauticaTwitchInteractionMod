using HarmonyLib;
using System;

namespace TwitchInteraction.Player_Events
{
    public class OneHitKnockout {
        public static bool active = false;
    }

    [HarmonyPatch(typeof(LiveMixin))]
    [HarmonyPatch("TakeDamage")]
    internal class LiveMixin_TakeDamage_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(LiveMixin __instance, ref bool __result)
        {
            if((Object)__instance.gameObject.GetComponent<Player>() == (Object)null)
            {
                // Not the player taking damage, execute the function normally
                return true;
            }
            
            // Check if OHKO flag is active
            if (OneHitKnockout.active)
            {
                // Reuse the kill logic from the LiveMixin, to ensure that cutscenes are handled properly.
                __result = true;
                if (!__instance.IsCinematicActive())
                {
                    __instance.Kill();
                }
                else
                {
                    __instance.cinematicModeActive = true;
                    __instance.SyncUpdatingState();
                }

                return false;
            }

            return true;
        }
    }
}