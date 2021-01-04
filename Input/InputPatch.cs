using HarmonyLib;
using TwitchInteraction.Player_Events;
using UnityEngine;

namespace TwitchInteraction.InputPatch
{

    public class InputPatch
    {
        public static bool invertKeyboardAxisX = false, invertKeyboardAxisY = false, invertKeyboardAxisZ = false;
        public static bool invertMouseAxisX = false, invertMouseAxisY = false;
        public static bool controlsEnabled = true;
    }

    [HarmonyPatch(typeof(GameInput))]
    [HarmonyPatch("GetMoveDirection")]
    internal class KeyboardAxisPatch
    {

        [HarmonyPostfix]
        public static void Postfix(ref Vector3 __result)
        {
            if (!InputPatch.controlsEnabled)
            {
                __result = Vector3.zero;
            }

            if (InputPatch.invertKeyboardAxisX)
            {
                __result.x *= -1;
            }
            if (InputPatch.invertKeyboardAxisY)
            {
                __result.y *= -1;
            }
            if (InputPatch.invertKeyboardAxisZ)
            {
                __result.z *= -1;
            }
        }

    }

    [HarmonyPatch(typeof(GameInput))]
    [HarmonyPatch("GetLookDelta")]
    internal class MouseAxisPatch
    {

        [HarmonyPostfix]
        public static void Postfix(ref Vector2 __result)
        {
            if (!InputPatch.controlsEnabled)
            {
                __result = Vector2.zero;
            }

            if (InputPatch.invertMouseAxisX)
            {
                __result.x *= -1;
            }
            if (InputPatch.invertMouseAxisY)
            {
                __result.y *= -1;
            }
        }

    }

    [HarmonyPatch(typeof(GameInput))]
    [HarmonyPatch("GetAnalogValueForButton")]
    internal class AnalogValue_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref float __result, GameInput.Button button)
        {
            if (!InputPatch.controlsEnabled)
            {
                __result = 0;
            }
        }
    }

    [HarmonyPatch(typeof(GameInput))]
    [HarmonyPatch("GetInputStateForButton")]
    internal class InputState_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref GameInput.InputState __result, GameInput.Button button)
        {
            if (!InputPatch.controlsEnabled)
            {
                __result = new GameInput.InputState
                {
                    flags = GameInput.InputStateFlags.Up,
                    timeDown = 0f
                };
            }
        }
    }

    [HarmonyPatch(typeof(GameInput))]
    [HarmonyPatch("GetButtonDown")]
    internal class ButtonDown_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, GameInput.Button button)
        {
            if (!InputPatch.controlsEnabled)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(GameInput))]
    [HarmonyPatch("GetButtonHeld")]
    internal class ButtonHeld_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, GameInput.Button button)
        {
            if (!InputPatch.controlsEnabled)
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(GameInput))]
    [HarmonyPatch("GetButtonUp")]
    internal class ButtonUp_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref bool __result, GameInput.Button button)
        {
            if (!InputPatch.controlsEnabled)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(GameInput))]
    [HarmonyPatch("GetButtonHeldTime")]
    internal class ButtonTime_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref float __result, GameInput.Button button)
        {
            if (!InputPatch.controlsEnabled)
            {
                __result = 0;
            }
        }
    }

}