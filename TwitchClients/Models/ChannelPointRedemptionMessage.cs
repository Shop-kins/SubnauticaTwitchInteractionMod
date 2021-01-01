using System.Text.Json.Serialization;

namespace TwitchInteraction
{
    public class ChannelPointRedemptionMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("data")]
        public ChannelPointRedemptionMessageData Data { get; set; }
    }
}