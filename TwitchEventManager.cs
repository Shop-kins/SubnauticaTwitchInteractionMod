using System;
using TwitchInteraction.Player_Events;

namespace TwitchInteraction
{
    class TwitchEventManager
    {
        public static void ChatMessageReceived(object sender, Message e)
        {
            if(e.Text.Trim() == ("{costs}"))
            {
                MainPatcher.TextChannel.SendMessageAsync(EventLookup.getBitCosts(), MainPatcher.cts);
            }
        }

        public static void PubSubMessageReceived(object sender, Message e)
        {
            if (e.Host == ChannelPointsHost())
            {
                EventLookup.Lookup(e.Text, e.User);
            }
            if (e.Host == BitsHost())
            {
                EventLookup.Lookup(e.Text, e.User, Int32.Parse(e.Text.Split(':')[0]));
            }
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
