using System.Text.RegularExpressions;
using System;
using System.Text.Json;

namespace TwitchInteraction
{
    class PubSubParser
    {
        /// <summary>
        /// Tries to parse raw message into message object.
        /// </summary>
        /// <param name="message">Raw message received from a server</param>
        /// <param name="msg">Output message object when successfully parsed</param>
        /// <returns></returns>
        public bool TryParsePrivateMessage(string message, out Message msg)
        {
            msg = new Message();
            
            var redemption = JsonSerializer.Deserialize<ChannelPointRedemptionMessage>(message);
            var regex = new Regex(".*login\":\"(?<user>.*?)\".*title\":\"(?<title>.*?)\".*");
            var regexBits = new Regex(".*login\":\"(?<user>.*?)\".*chat_message\":\"(?<title>.*?)\".*");

            var match = regex.Match("");
            //FOR SOME REASON YOU NEED THIS IN A TRY CATCH OR IT DOESN'T FUNCTION
            try
            {
                match = regex.Match(redemption.Data.Message);
                msg.Host = redemption.Data.Topic;
                if (!match.Success)
                {
                    match = regexBits.Match(redemption.Data.Message);
                    msg.Host = redemption.Data.Topic;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("THIS IS ERROR" + e.Message);
            }

            if (!match.Success)
                return false;

            var groups = match.Groups;

            msg.RawMessage = message;
            msg.User = groups["user"].Value;
            msg.Channel = groups["user"].Value;
            msg.Text = groups["title"].Value;

            return true;
        }
    }
}