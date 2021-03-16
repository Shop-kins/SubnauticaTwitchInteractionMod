using System;
using System.Collections.Generic;
using System.Text.Json;
using TwitchInteraction.CrowdControl;
using TwitchInteraction.Player_Events;

namespace TwitchInteraction
{
    internal class CrowdControlEventManager
    {

        //MAP OF CROWDCONTROL KEYS TO EVENT DICT KEYS
        private static readonly Dictionary<string, string> EventNameDict = new Dictionary<string, string>()
        {
            { "kill", "Rip Riley [Integration]" },
            { "heal", "Heal Riley [Integration]" },
            { "toggle_day_night", "Toggle Day/Night [Integration]" },
            { "open_pda", "Open PDA [Integration]" },
            { "gun", "Turn on the big gun [Integration]" },
            { "fill_o2", "Fill Oxygen [Integration]" },
            { "teleport_lifepod_shallows", "Find a new home [Integration]" },
            { "teleport_player", "Player Teleport [Integration]" },
            { "tooth", "Give that beautiful tooth [Integration]" },
            { "spawn_random_creature", "Cow or Reaper? Yes. [Integration]" },
            { "junk", "Fill him up with junk [Integration]" },
            { "reaper", "Get your pet reaper to hang out [Integration]" },
            { "resource_roulette", "Resource Roulette [Integration]" },
            { "blueprint_roulette", "Blueprint Roulette [Integration]" },
            { "eat", "An early breakfast [Integration]" },
            { "teleport_lifepod_deep", "Find a really new home [Integration]" },
            { "tooth_sound", "Play tooth drop sound [Integration]" },
            { "clear_hotbar", "Clear a hotbar slot [Integration]" },
            { "hotbar_shuffle", "Shuffle the hotbar [Integration]" },
            { "take_battery", "Steal a battery [Integration]" },
            { "take_equipment", "Steal some equipment [Integration]" },
            { "kill_enemies", "Kill bad things [Integration]" },
            { "teleport_player_shallows", "Go back home [Integration]" },
            { "advanced_roulette", "Crafted Roulette [Integration]" },
            { "random_mouse", "Random Mouse Sensitivity [Integration]" },
            { "hide_hud", "Hide HUD [Integration]"},
            { "invert_controls", "Invert Controls [Integration]" },
            { "disable_controls", "Disable Controls [Integration]" },
            { "filmic_mode", "Light? What is light? [Integration]" },
            { "random_fov", "Random FOV [Integration]" },
            { "restore_ship", "What explosion? [Integration]"  },
            { "ohko", "Be careful Riley [Integration]" }
        };

        internal static void MessageReceived(int sender, string message)
        {

            Console.WriteLine("Message received. Id: " + sender + ", message:" + message);
        }

        internal static void ClientConnected(CrowdControlClient c)
        {
            Console.WriteLine("Client connected");
            Console.WriteLine("Awaiting CC Command");

            c.Receive();            
        }

        internal static void ClientMessageReceived(CrowdControlClient c, string raw)
        {
            try { 
                Console.WriteLine("Message received: " + raw);
                // Decode the message from CC
                // { "id":1,"code":"kill","viewer":"sdk","type":1}
                var request = JsonSerializer.Deserialize<CrowdControlRequest>(raw);

                // Do the thing
                var eventName = EventNameDict[request.Code];
                var status = 0;

                if (TimerCooldown.IsInitialised())
                {
                    try
                    {
                        EventLookup.Lookup(eventName);
                    }
                    catch (Exception)
                    {
                        // On exception set failed flag
                        status = 1;
                    }
                } else
                {
                    // Game is not yet ready for events, retry until it is.
                    status = 3;
                }


                // Send the result to CC
                var response = new CrowdControlResponse();
                response.Id = request.Id;
                response.Status = status;
                response.Message = "Effect: " + request.Code + ": " + response.Status;

                var resJson = JsonSerializer.Serialize<CrowdControlResponse>(response);
                Console.WriteLine("Sending response: " + resJson);

                c.Send(resJson, false);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        internal static void ClientMessageSent(CrowdControlClient c, bool close)
        {
            Console.WriteLine("Message Sent.");
            // Await next command
            c.Receive();
        }
    }
}