using System.Text.RegularExpressions;

namespace TwitchInteraction
{
    class MessageParser
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
            var regex = new Regex(":(?<user>.*)!(.*)@(?<host>.*) PRIVMSG #(?<channel>.*) :(?<text>.*)");
            var match = regex.Match(message);

            if (!match.Success)
                return false;

            var groups = match.Groups;

            msg.RawMessage = message;
            msg.User = groups["user"].Value;
            msg.Host = groups["host"].Value;
            msg.Channel = groups["channel"].Value;
            msg.Text = groups["text"].Value;

            return true;
        }
    }
}