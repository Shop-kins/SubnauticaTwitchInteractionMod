using System;
using TwitchInteraction.Player_Events;

namespace TwitchInteraction
{
    class TwitchEventManager
    {
        public static void ChatMessageReceived(object sender, Message e)
        {
            Console.WriteLine("Received Chat Message");
            if (e.Text.Trim() == "{random}")
            {
                MainPatcher.TextChannel.SendMessageAsync(EventLookup.FourRandomFunZone(), MainPatcher.cts);
            } else if(e.Text.StartsWith("{"))
            {
                string Text = e.Text.Trim();
                string TextCleaned = Text.Substring(1, Text.Length - 2);
                int? BitCost = EventLookup.getBitCost(TextCleaned);
                if(BitCost != null)
                    MainPatcher.TextChannel.SendMessageAsync(TextCleaned + " costs " + BitCost + " Bits", MainPatcher.cts);
            }
        }

        public static void PubSubMessageReceived(object sender, Message e)
        {
            Console.WriteLine("Received Pub Sub Message:");
            if (e.Host == ChannelPointsHost())
            {
                EventLookup.Lookup(e.Text);
            }
            if (e.Host == BitsHost())
            {

                EventLookup.Lookup(e.Text, Int32.Parse(e.Text.Split(':')[0]));
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
