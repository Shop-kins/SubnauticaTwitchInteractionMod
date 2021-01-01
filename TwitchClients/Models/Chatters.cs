using System.Text.Json.Serialization;

namespace TwitchInteraction
{
    public class Chatters
    {
        [JsonPropertyName("broadcaster")]
        public string[] Brodcaster { get; set; }

        [JsonPropertyName("vips")]
        public string[] Vips { get; set; }

        [JsonPropertyName("moderators")]
        public string[] Moderators { get; set; }

        [JsonPropertyName("staff")]
        public string[] Staff { get; set; }

        [JsonPropertyName("admins")]
        public string[] Admins { get; set; }

        [JsonPropertyName("global_mods")]
        public string[] GlobalMods { get; set; }

        [JsonPropertyName("viewers")]
        public string[] Viewers { get; set; }
    }
}