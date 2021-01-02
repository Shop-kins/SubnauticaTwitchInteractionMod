using System;
using TwitchLib.Client.Events;
using UnityEngine;
using TwitchLib.Api.Models.Undocumented.Chatters;
using TwitchLib.PubSub.Events;
using System.Collections;
using System.Collections.Generic;

namespace TwitchInteraction
{
    class TwitchEventManager
    {
        public static void ChatMessageReceived(object sender, Message e)
        {
            Console.WriteLine("Received Chat Message");
        }

        public static void PubSubMessageReceived(object sender, Message e)
        {
            Console.WriteLine("Received Pub Sub Message");
            Player_Events.EventLookup.Lookup(e.Text);
        }

        private static string ChannelPointsHost()
        {
            return "channel-points-channel-v1." + MainPatcher.secrets.nick_id;
        }

        private static string BitsHost()
        {
            return "channel-bits-events-v2." + MainPatcher.secrets.nick_id;
        }

    }
}
