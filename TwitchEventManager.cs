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
                EventLookup.Lookup(e.Text);
            }
            if (e.Host == BitsHost())
            {
                string message = EventLookup.Lookup(e.Text, Int32.Parse(e.Text.Split(':')[0]));
                if(message != "" && MainPatcher.TextChannel != null)
                    MainPatcher.TextChannel.SendMessageAsync(e.User + " " + message, MainPatcher.cts);
            }
        }

        public static void ConnectChatOnDisconnect(Object sender, Object a)
        {
            MainPatcher.otherclient.ConnectAsync("oauth:" + MainPatcher.secrets.access_token, MainPatcher.secrets.botname, MainPatcher.cts);
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
