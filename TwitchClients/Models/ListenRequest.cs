namespace TwitchInteraction
{
    public class ListenRequest
    {
        public string type { get; set; }

        public string nonce { get; set; }

        public ListenRequestData data { get; set; }
    }
}