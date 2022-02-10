using System.Text.RegularExpressions;
using System;
using Oculus.Newtonsoft.Json;

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
            
            var redemption = JsonConvert.DeserializeObject<ChannelPointRedemptionMessage>(message);
            var regex = new Regex(".*login\":\"(?<user>.*?)\".*title\":\"(?<title>.*?)\".*");
            var regexBits = new Regex(".*user_name\":\"(?<user>.*?)\".*chat_message\":\"(?<title>.*?)\".*\"bits_used\":(?<bits>.*?),.*");

            var match = regex.Match("");
            //FOR SOME REASON YOU NEED THIS IN A TRY CATCH OR IT DOESN'T FUNCTION
            try
            {
                match = regex.Match(redemption.data.message);
                msg.Host = redemption.data.topic;
                if(match.Success)
                    msg.Text = match.Groups["title"].Value;
                if (!match.Success)
                {
                    match = regexBits.Match(redemption.data.message);
                    msg.Host = redemption.data.topic;
                    if(match.Success)
                        msg.Text = match.Groups["bits"].Value + ":" + match.Groups["title"].Value;
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
            msg.Channel = MainPatcher.secrets.username;

            return true;
        }
    }
}