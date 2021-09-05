using HarmonyLib;

namespace TwitchInteraction.Player_Events
{
    public class FastMovement
    {
        public static bool Active = false;
    }

    [HarmonyPatch(typeof(PlayerController))]
    [HarmonyPatch("SetMotorMode")]
    internal class PlayerController_SetMotorMode_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(PlayerController __instance)
        {
            if (FastMovement.Active)
            {
                __instance.underWaterController.swimDrag = 0.75f;
                __instance.underWaterController.forwardMaxSpeed = 50f;
                __instance.groundController.forwardMaxSpeed = 35f;
            }
        }
    }
}
