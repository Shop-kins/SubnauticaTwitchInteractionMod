using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using TwitchInteraction.CrowdControl;
using TwitchInteraction.Player_Events;

namespace TwitchInteraction
{
    internal class CrowdControlEventManager
    {

        //MAP OF CROWDCONTROL KEYS TO EVENT DICT KEYS
        private static readonly Dictionary<string, string> EventNameDict = new Dictionary<string, string>()
        {
            { "kill", EventCodes.KILL},
            { "heal", EventCodes.HEAL },
            { "toggle_day_night", EventCodes.DAY_NIGHT },
            { "open_pda", EventCodes.PDA_OPEN },
            { "fill_o2", EventCodes.OXYGEN_FILL },
            { "spawn_random_creature", EventCodes.RANDOM_CREATURE },
            { "junk", EventCodes.JUNK_FILL },
            { "reaper", EventCodes.LEVIATHAN},
            { "resource_roulette", EventCodes.BASIC_ITEM },
            { "blueprint_roulette", EventCodes.RANDOM_BLUEPRINT },
            { "eat", EventCodes.FILL_SURVIVAL },
            { "clear_hotbar", EventCodes.HOTBAR_CLEAR },
            { "hotbar_shuffle", EventCodes.HOTBAR_SHUFFLE },
            { "take_battery", EventCodes.BATTERY_STEAL },
            { "take_equipment", EventCodes.EQUIPMENT_STEAL },
            { "kill_enemies", EventCodes.KILL_ENEMIES },
            { "advanced_roulette", EventCodes.CRAFTED_ITEM },
            { "random_mouse", EventCodes.MOUSE_SENSITIVITY },
            { "invert_controls", EventCodes.INVERT_CONTROLS },
            { "disable_controls", EventCodes.DISABLE_CONTROLS },
            { "random_fov", EventCodes.FOV },
            { "ohko", EventCodes.ONE_HIT_MODE },
            { "go_really_fast", EventCodes.SPEED },
            { "custom_beacon", EventCodes.BEACON },
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
                var request = JsonConvert.DeserializeObject<CrowdControlRequest>(raw);

                // Do the thing
                var eventName = EventNameDict[request.code];
                var status = 0;

                if (request.id == 0 && request.type == 255)
                {
                    // Test message, ignore
                    return;
                }

                if (TimerCooldown.IsInitialised())
                {
                    try
                    {
                        // Check to see if the event can fire currently fire, or is on cooldown.
                        if (!EventLookup.IsRunningOrCooldown(eventName))
                        {
                            // Not running or on cooldown, activate it if the type is "start"
                            if (request.type == 1)
                            {
                                EventLookup.Lookup(eventName, request.viewer);
                            }                            
                        }
                        else
                        {
                            // Event in use, retry.
                            status = 3;
                        }
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
                response.id = request.id;
                response.status = status;
                response.message = "Effect: " + request.code + ": " + response.status;

                var stringBuilder = new StringBuilder();
                var stringWriter = new StringWriter(stringBuilder);
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    var jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(jsonTextWriter, response);
                }

                var resJson = stringBuilder.ToString();
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