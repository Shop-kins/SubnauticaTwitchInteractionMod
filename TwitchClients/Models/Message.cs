namespace TwitchInteraction
{
    public class Message
    {
        public string RawMessage { get; set; }

        public string User { get; set; }

        public string Host { get; set; }

        public string Channel { get; set; }

        public string Text { get; set; }
    }
}